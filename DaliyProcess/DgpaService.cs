namespace DgpaMapWebApi.DaliyProcess
{
    public class DgpaService
    {
        public DgpaService() { }

        public async Task<string> GetDgpaXml(string url)
        {
            // 创建HttpClient实例
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // GET请求
                    HttpResponseMessage response = await client.GetAsync(url);

                    // 確保請求成功
                    response.EnsureSuccessStatusCode();
                    // 回應内容
                    string responseData = await response.Content.ReadAsStringAsync();

                    return responseData;
                }
                catch (HttpRequestException e)
                {
                    // 处理请求异常
                    Console.WriteLine($"请求错误: {e.Message}");
                    return "";
                }
            }
        }
    }
}
