using DgpaMapWebApi.Models;
using DgpaMapWebApi.Service;
using DgpaMapWebApi.DaliyProcess.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading;

namespace DgpaMapWebApi.DaliyProcess
{
    public class DailyProcess
    {

        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DgpaDbContext(serviceProvider.GetRequiredService<DbContextOptions<DgpaDbContext>>()))
            {

                if (context.Job.Any())
                {
                    return;
                }

                var jobParser = new JobXmlParser();

                //*檔案測試Xml*//
                //var jobList = jobParser.ParseXmlFile("C:\\Users\\user\\Desktop\\dgpa-map\\data\\1131208_TeatData - 複製.xml");
                //*檔案測試Xml*//

                //*網路請求Xml*//
                var dgpaHttpService = new DgpaService();
                string xmlContent = await dgpaHttpService.GetDgpaXml("https://web3.dgpa.gov.tw/WANT03FRONT/AP/WANTF00003.aspx?GETJOB=Y");
                var jobList = jobParser.ParseXmlString(xmlContent);
                //*網路請求*//

                //Console.WriteLine("HereMapApi請求開始");
                ////*HereMapApi請求*//
                var hereMapService = new HereMapService();
                var hereMapRequestTask
                    = jobList.Select(async job =>
                    {
                        try
                        {
                            var coordinates
                                = await hereMapService.GetCoordinatesAsync(job.WORK_ADDRESS, job.WORK_PLACE_TYPE);

                            //如第1次請求失敗則換個參數第2次
                            if (coordinates.latitude == (decimal?)24.25 && coordinates.longitude == (decimal?)119.65)
                            {
                                var secondAddress = $"{job.WORK_PLACE_TYPE}{job.ORG_NAME}";
                                coordinates = await hereMapService.GetCoordinatesAsync(secondAddress, job.WORK_PLACE_TYPE);
                                Console.WriteLine($"============={job.ORG_NAME}:{job.WORK_ADDRESS}請求第2次=============");
                            }
                            job.Coordinate_X = coordinates.latitude;
                            job.Coordinate_Y = coordinates.longitude;
                            Console.WriteLine($".{job.ORG_NAME}:坐標{job.Coordinate_X},{job.Coordinate_Y}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"hereMapService錯誤:{ex}");
                        }
                    });
                await Task.WhenAll(hereMapRequestTask);
                //*HereMapApi請求*//

                context.Job.AddRange(jobList);
                context.SaveChanges();

            }
        }

        public static async void UpdateJob(IServiceProvider serviceProvider)
        {
            using (var context = new DgpaDbContext(serviceProvider.GetRequiredService<DbContextOptions<DgpaDbContext>>()))
            {
                var existedJobUrlId = context.Job.Select(x => x.VIEW_URL).AsNoTracking().ToHashSet();
                //DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                //DateOnly yesterday = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
                //2.如資料庫DATE_FROM無今天職缺，代表還今天未更新，則嘗試抓xml，篩選DATE_FROM等於今天加入
                //if ( !context.Job.Any( x=> x.DATE_FROM == yesterday) ) { }


                //*網路請求Xml*//
                var dgpaHttpService = new DgpaService();
                string xmlContent = await dgpaHttpService.GetDgpaXml("https://web3.dgpa.gov.tw/WANT03FRONT/AP/WANTF00003.aspx?GETJOB=Y");
                //*網路請求*//

                //檢查有相同urlId視為已在資料庫內
                var jobParser = new JobXmlParser();
                

                var updateJobList = jobParser.ParseXmlString(xmlContent)
                                                        .Where(x => !existedJobUrlId.Contains(x.VIEW_URL));
                if (!updateJobList.Any())
                {
                    Console.WriteLine($"無需要Update資料");
                    return;
                }
                //foreach (var item in updateJobList)
                //{
                //    Console.WriteLine($"{item}");
                //}


                //*HereMapApi請求*//
                var hereMapService = new HereMapService();
                var hereMapRequestTask
                    = updateJobList.Select(async job =>
                    {
                        try
                        {
                            var coordinates
                                = await hereMapService.GetCoordinatesAsync(job.WORK_ADDRESS, job.WORK_PLACE_TYPE);

                            //如第1次請求失敗則換個參數第2次
                            if (coordinates.latitude == (decimal)24.25 && coordinates.longitude == (decimal)119.65)
                            {
                                var secondAddress = $"{job.WORK_PLACE_TYPE}{job.ORG_NAME}";
                                coordinates = await hereMapService.GetCoordinatesAsync(secondAddress, job.WORK_PLACE_TYPE);
                                job.IS_XYDoubt = true;
                                Console.WriteLine($"============={job.ORG_NAME}:{job.WORK_ADDRESS}請求第2次=============");
                            }
                            job.Coordinate_X = coordinates.latitude;
                            job.Coordinate_Y = coordinates.longitude;
                            Console.WriteLine($".{job.ORG_NAME}:坐標{job.Coordinate_X},{job.Coordinate_Y}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"hereMapService錯誤:{ex}");
                        }
                    });
                await Task.WhenAll(hereMapRequestTask);
                ////*HereMapApi請求*//
                UpdateDate today = new UpdateDate{ LastUpdateDate = DateOnly.FromDateTime(DateTime.Now) };
                context.UpdateDate.AddRange(today);
                context.Job.AddRange(updateJobList);
                context.SaveChanges();



            }
        }

        public static async void MoveExpiredJob(IServiceProvider serviceProvider, CancellationToken cancellationToken = default) 
        {
            Console.WriteLine($"MoveExpiredJob");
            //1.DATE_TO大於今天日期>>>>移到History並刪除
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            using (var context = new DgpaDbContext(serviceProvider.GetRequiredService<DbContextOptions<DgpaDbContext>>()))
            {
                if (!context.Job.Any())
                {
                    Console.WriteLine($"資料庫無資料");
                    return;
                }
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                // 找出過期的工作
                var expiredJobs = await context.Job
                    .Where(j => j.DATE_TO < today)
                    .ToListAsync(cancellationToken);
                if (expiredJobs.Count == 0)
                {
                    Console.WriteLine($"MoveExpiredJob無資料");
                    return;
                }
                // 轉換並儲存到歷史表
                var historyJobs = expiredJobs.Select(x => new HistoryJob
                {
                    JOB_ID = x.JOB_ID,
                    ORG_ID = x.ORG_ID,
                    ORG_NAME = x.ORG_NAME,
                    RANK_START = x.RANK_START,
                    RANK_END = x.RANK_END,
                    TITLE = x.TITLE,
                    SYSNAM = x.SYSNAM,
                    NUMBER_OF = x.NUMBER_OF,
                    RESERVE_NUM = x.RESERVE_NUM,
                    WORK_PLACE_TYPE = x.WORK_PLACE_TYPE,
                    DATE_FROM = x.DATE_FROM,
                    DATE_TO = x.DATE_TO,
                    IS_HANDICAP = x.IS_HANDICAP,
                    IS_ORIGINAL = x.IS_ORIGINAL,
                    IS_LOCAL_ORIGINAL = x.IS_LOCAL_ORIGINAL,
                    IS_TRANSFER = x.IS_TRANSFER,
                    IS_TRANING = x.IS_TRANING,
                    WORK_QUALITY = x.WORK_QUALITY,
                    WORK_ITEM = x.WORK_ITEM,
                    WORK_ADDRESS = x.WORK_ADDRESS,
                    CONTACT_METHOD = x.CONTACT_METHOD,
                    VIEW_URL = x.VIEW_URL,
                    Coordinate_X = x.Coordinate_X,
                    Coordinate_Y = x.Coordinate_Y
                }).ToList();

                await context.HistoryJob.AddRangeAsync(historyJobs, cancellationToken);
                // 刪除原始資料
                context.Job.RemoveRange(expiredJobs);
                // 儲存變更
                await context.SaveChangesAsync(cancellationToken);
                // 提交交易
                await transaction.CommitAsync(cancellationToken);
            }
        }
    }

}
