namespace DgpaMapWebApi.DaliyProcess.Utils
{
    public class JobFilter
    {
        private readonly HashSet<string> _keywords;

        public JobFilter(string filePath)
        {
            // 讀取所有關鍵字並存入 HashSet
            _keywords = new HashSet<string>(
                File.ReadAllLines(filePath),
                StringComparer.OrdinalIgnoreCase  // 不區分大小寫
            );
        }

        public bool IsMatch(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            return _keywords.Any(keyword => text.Contains(keyword));
        }
    }
}
