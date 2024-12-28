using DgpaMapWebApi.DaliyProcess.Extensions;
using DgpaMapWebApi.Models;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DgpaMapWebApi.DaliyProcess.Utils
{
    public class JobXmlParser
    {

        private JobFilter _personKindFilter = new JobFilter("C:\\Users\\user\\Desktop\\dgpa-map\\data\\filters\\unqualified-list.txt");
        public JobXmlParser()
        {
        }

        public List<Job> ParseXmlString(string xmlContent)
        {
            var doc = XDocument.Parse(xmlContent);
            var jobPostings = new List<Job>();

            foreach (var rowElement in doc.Descendants("ROW"))
            {
                if (!IsRowElmentValid(rowElement))
                {
                    continue;
                }
                jobPostings.Add(ParseXmlRowElment(rowElement));
            }

            return jobPostings;
        }

        //public List<Task<Job>> ParseXmlFile(string filePath)
        public List<Job> ParseXmlFile(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);

            //HereMapHelper hereMapHelper = new HereMapHelper();

            //TYPE = ”簡任”,”薦任”,”委任”,”其他”
            var jobs = doc.Descendants("ROW")
                .Where(rowElement => IsRowElmentValid(rowElement))
                //.Select(async rowElement =>
                .Select(rowElement => ParseXmlRowElment(rowElement))
                .ToList();
            return jobs;

        }

        private bool IsRowElmentValid(XElement rowElement)
        {

            //TYPE = ”簡任”,”薦任”,”委任”,”其他”
            if (rowElement.Element("TYPE")?.Value == "其他") return false;
            if (_personKindFilter.IsMatch(rowElement.Element("TITLE")?.Value)) return false;

            return true;
        }

        private Job ParseXmlRowElment(XElement rowElement)
        {
            var address = rowElement.Element("WORK_ADDRESS")?.Value.Trim() ?? "";
            //var (latitude, longitude) = await hereMapHelper.GetCoordinatesAsync(address);
            //正取備取人數
            int offerNum = StringToInt(rowElement.Element("NUMBER_OF")?.Value.Trim(), 0);
            int reserveNum = StringToInt(rowElement.Element("RESERVE_NUM")?.Value.Trim(), 0);

            bool isHandicap = rowElement.Element("IS_HANDICAP")?.Value.Trim() == "Y";
            bool isOriginal = rowElement.Element("IS_ORIGINAL")?.Value.Trim() == "Y";
            bool isLocalOriginal = rowElement.Element("IS_LOCAL_ORIGINAL")?.Value.Trim() == "Y";
            bool isTransfer = rowElement.Element("IS_TRANSFER")?.Value.Trim() == "Y";
            bool isTraing = rowElement.Element("IS_TRANING")?.Value.Trim() == "Y";
            //職等起訖
            (int rankStart, int rankEnd) = ParseRankRange(rowElement.Element("RANK")?.Value.Trim() ?? "");
            string[] urlSplit = rowElement.Element("VIEW_URL")?.Value.Split("work_id=");
            string urlId = "";
            if (urlSplit.Length >= 2)
            {
                urlId = urlSplit[1].LimitLength(10);
            }


            return new Job
            {
                ORG_ID = rowElement.Element("ORG_ID")?.Value.LimitLength(10) ?? "",
                ORG_NAME = rowElement.Element("ORG_NAME")?.Value.LimitLength(50) ?? "",
                RANK_START = rankStart,
                RANK_END = rankEnd,
                TITLE = rowElement.Element("TITLE")?.Value.LimitLength(10) ?? "",
                SYSNAM = rowElement.Element("SYSNAM")?.Value.LimitLength(20) ?? "",
                NUMBER_OF = offerNum,
                RESERVE_NUM = reserveNum,
                WORK_PLACE_TYPE = ParseCity(rowElement.Element("WORK_PLACE_TYPE")?.Value),
                DATE_FROM = rowElement.Element("DATE_FROM")?.Value.Trim().ToDateOnly(),
                DATE_TO = rowElement.Element("DATE_TO")?.Value.Trim().ToDateOnly(),
                IS_HANDICAP = isHandicap,
                IS_ORIGINAL = isOriginal,
                IS_LOCAL_ORIGINAL = isLocalOriginal,
                IS_TRANSFER = isTransfer,
                IS_TRANING = isTraing,
                WORK_QUALITY = rowElement.Element("WORK_QUALITY")?.Value.Trim() ?? "",
                WORK_ITEM = rowElement.Element("WORK_ITEM")?.Value.Trim() ?? "",
                WORK_ADDRESS = rowElement.Element("WORK_ADDRESS")?.Value.Trim() ?? "",
                CONTACT_METHOD = rowElement.Element("CONTACT_METHOD")?.Value.Trim() ?? "",
                VIEW_URL = urlId,
                Coordinate_X = (decimal?)119.65,
                Coordinate_Y = (decimal?)24.25,
                IS_XYDoubt = false,
            };

        }

        private string ParseCity(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            //input範例:",61-嘉義縣,72-臺南市"，取得第一個縣市
            var firstCity = input.Trim(',')
                                .Split(',')
                                .FirstOrDefault()
                                ?.Split('-')
                                .LastOrDefault();

            return firstCity ?? string.Empty;
        }


        private (int min, int max) ParseRankRange(string rankStr)
        {
            try
            {
                var numbers = Regex.Matches(rankStr, @"\d+")
                                  .Select(m => int.Parse(m.Value))
                                  .ToList();
                if (!numbers.Any())
                    return (0, 0);

                return (numbers.First(), numbers.Last());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (0, 0);
            }
        }

        private int StringToInt(string str, int defaltInt)
        {
            int.TryParse(str, out defaltInt);
            return defaltInt;
        }
    }
}
