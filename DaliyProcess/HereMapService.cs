using DgpaMapWebApi.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Threading.RateLimiting;
using static System.Net.WebRequestMethods;

namespace DgpaMapWebApi.DaliyProcess
{
    public class HereMapService
    {
        private readonly string _apiKey = "HereMapApiKey";
        private const string BASE_URL = "https://geocode.search.hereapi.com/v1/geocode";
        //private readonly HereMapSetting _settings;

        private readonly Queue<DateTime> _requestTimestamps = new();
        private readonly object _lock = new();
        private readonly int _rateLimit = 5;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(1.5);

        //public HereMapService(IOptions<HereMapSetting> settings)
        //{
            //Console.WriteLine($"--------{settings.Value.ApiKey}------------");
            //_apiKey = settings.Value.ApiKey;
        //}

        public async Task<(decimal latitude, decimal longitude)> GetCoordinatesAsync(string address, string city)
        {
            using var client = new HttpClient();
            // 構建 API 請求 URL
            string firstRequestUrl =
                $"{BASE_URL}?q={Uri.EscapeDataString(address)}&city={city}&in=countryCode:TWN&apiKey={_apiKey}";

            try
            {
                //限制請求速率，取得令牌才可繼續
                await WaitForNextRequestAsync();
                // 發送 GET 請求
                var response = await client.GetStringAsync(firstRequestUrl);

                // 解析 JSON 響應
                var jsonResult = JObject.Parse(response);


                // 檢查第一次是否有結果
                if (jsonResult["items"] != null && jsonResult["items"].HasValues)
                {
                    // 獲取第一個結果的坐標
                    var position = jsonResult["items"][0]["position"];

                    decimal latitude = (decimal)position["lat"];
                    decimal longitude = (decimal)position["lng"];

                    return (latitude, longitude);
                }
                else
                {
                    //throw new Exception("未找到坐標");
                    return ((decimal)24.25, (decimal)119.65);
                }
            }
            catch (HttpRequestException ex)
            {
                return ((decimal)24.25, (decimal)119.65);
            }
            catch (Exception ex)
            {
                throw new Exception($"{address},{city},坐標轉換錯誤: {ex.Message}");
            }
        }

        private async Task WaitForNextRequestAsync()
        {
            while (true)
            {
                lock (_lock)
                {
                    var now = DateTime.UtcNow;

                    // 移除過期的時間戳
                    while (_requestTimestamps.Count > 0 &&
                           now - _requestTimestamps.Peek() > _interval)
                    {
                        _requestTimestamps.Dequeue();
                    }

                    // 如果請求數未達上限
                    if (_requestTimestamps.Count < _rateLimit)
                    {
                        _requestTimestamps.Enqueue(now);
                        return;
                    }
                }

                // 等待一段時間後重試
                await Task.Delay(100);
            }
        }

    }
}
