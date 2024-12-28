using DgpaMapWebApi.Dto;
using DgpaMapWebApi.Dtos;
using DgpaMapWebApi.Interface;
using DgpaMapWebApi.Models;
using DgpaMapWebApi.Parameter;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DgpaMapWebApi.Service
{
    public class JobService : IJobService
    {
        private readonly DgpaDbContext _dgpaDb;

        public JobService(DgpaDbContext dgpaDb)
        {
            _dgpaDb = dgpaDb;
        }

        public async Task<List<JobDto>> GetAllDataAsync()
        {
            var result = from x in _dgpaDb.Job
                         select new JobDto
                         {
                             ORG_NAME = x.ORG_NAME,
                             RANK_START = x.RANK_START,
                             RANK_END = x.RANK_END,
                             TITLE = x.TITLE,
                             SYSNAM = x.SYSNAM,
                             NUMBER_OF = x.NUMBER_OF,
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
                             Coordinate_Y = x.Coordinate_Y,
                             IS_XYDoubt = x.IS_XYDoubt,
                         };

            if (!result.Any())
            {
                return null;
            }
            return await result.ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _dgpaDb.Job.CountAsync();
        }

        public async Task<List<JobSelectDto>> GetQueryDataAsync(JobSelectParameter queryParas)
        {
            var queryResult = _dgpaDb.Job.AsQueryable();

            if (!string.IsNullOrEmpty(queryParas.orgName))
            {
                queryResult = queryResult.Where(x => x.ORG_NAME.Contains(queryParas.orgName));
            }

            if (!string.IsNullOrEmpty(queryParas.workPlace))
            {
                var workPlaceSet = new HashSet<string>(
                            queryParas.workPlace.Split(","),
                            StringComparer.OrdinalIgnoreCase
                        );
                queryResult = queryResult.Where(x => workPlaceSet.Contains(x.WORK_PLACE_TYPE));
            }

            if (!string.IsNullOrEmpty(queryParas.sysNames))
            {
                // 轉換成 HashSet
                var sysNameSet = new HashSet<string>(
                        queryParas.sysNames.Split(","),
                        StringComparer.OrdinalIgnoreCase
                    );
                queryResult = queryResult.Where(x => sysNameSet.Contains(x.SYSNAM));
            }
            if (queryParas.rankStart != null)
            {
                queryResult = queryResult.Where(x => x.RANK_START >= queryParas.rankStart);
            }
            if (queryParas.rankEnd != null)
            {
                queryResult = queryResult.Where(x => x.RANK_END <= queryParas.rankEnd);
            }

            if (!queryResult.Any())
            {
                return null;
            }


            return await queryResult.Select(x=> new JobSelectDto
            {
                ORG_NAME = x.ORG_NAME,
                RANK_START = x.RANK_START,
                RANK_END = x.RANK_END,
                TITLE = x.TITLE,
                SYSNAM = x.SYSNAM,
                NUMBER_OF = x.NUMBER_OF,
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
                Coordinate_Y = x.Coordinate_Y,
                IS_XYDoubt = x.IS_XYDoubt,

            }).ToListAsync();
        }

        public async Task<Dto.FeatureCollection> GetGeojsonDataAsync()
        {
            var queryResult =
                _dgpaDb.Job.Select((x) =>
                    new Feature
                    {
                        Geometry = new Geometry
                        {
                            Coordinates = new[] { x.Coordinate_Y, x.Coordinate_X }
                        },
                        Id = Guid.NewGuid(),
                        Properties = new JobDto
                                    {
                                        ORG_NAME = x.ORG_NAME,
                                        RANK_START = x.RANK_START,
                                        RANK_END = x.RANK_END,
                                        TITLE = x.TITLE,
                                        SYSNAM = x.SYSNAM,
                                        NUMBER_OF = x.NUMBER_OF,
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
                                        Coordinate_Y = x.Coordinate_Y,
                                        IS_XYDoubt = x.IS_XYDoubt,
                                    }
                    });

            if ( !queryResult.Any() )
            {
                return null;
            }
            return new Dto.FeatureCollection { Features = await queryResult.ToListAsync() };
        }

        public async Task<Dto.FeatureCollection> GetQueryGeojsonDataAsync(JobSelectParameter queryParas)
        {
            var queryResult = _dgpaDb.Job.AsQueryable();

            if (!string.IsNullOrEmpty(queryParas.orgName))
            {
                queryResult = queryResult.Where(x => x.ORG_NAME.Contains(queryParas.orgName));
            }

            if (!string.IsNullOrEmpty(queryParas.workPlace))
            {
                var workPlaceSet = new HashSet<string>(
                            queryParas.workPlace.Split(","),
                            StringComparer.OrdinalIgnoreCase
                        );
                queryResult = queryResult.Where(x => workPlaceSet.Contains(x.WORK_PLACE_TYPE));
            }

            if (!string.IsNullOrEmpty(queryParas.sysNames))
            {
                // 轉換成 HashSet
                var sysNameSet = new HashSet<string>(
                        queryParas.sysNames.Split(","),
                        StringComparer.OrdinalIgnoreCase
                    );
                queryResult = queryResult.Where(x => sysNameSet.Contains(x.SYSNAM));
            }
            if (queryParas.rankStart != null)
            {
                queryResult = queryResult.Where(x => x.RANK_START >= queryParas.rankStart);
            }
            if (queryParas.rankEnd != null)
            {
                queryResult = queryResult.Where(x => x.RANK_END <= queryParas.rankEnd);
            }

            if (!queryResult.Any())
            {
                return null;
            }

            return new Dto.FeatureCollection
            {
                Features = await queryResult.Select((x) => new Feature
                {
                    Geometry = new Geometry
                    {
                        Coordinates = new[] { x.Coordinate_Y, x.Coordinate_X }
                    },
                    Id = Guid.NewGuid(),
                    Properties = new JobDto
                        {
                            ORG_NAME = x.ORG_NAME,
                            RANK_START = x.RANK_START,
                            RANK_END = x.RANK_END,
                            TITLE = x.TITLE,
                            SYSNAM = x.SYSNAM,
                            NUMBER_OF = x.NUMBER_OF,
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
                            Coordinate_Y = x.Coordinate_Y,
                            IS_XYDoubt = x.IS_XYDoubt,
                        }
                }).ToListAsync()
            };
        }



    }
}
