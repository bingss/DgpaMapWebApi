namespace DgpaMapWebApi.DaliyProcess.Extensions
{
    public static class DateExtensions
    {
        public static DateOnly ToDateOnly(this string rocDate)
        {
            try
            {
                if (string.IsNullOrEmpty(rocDate) || rocDate.Length != 7)
                    throw new ArgumentException("Invalid ROC date format");

                int year = int.Parse(rocDate.Substring(0, 3)) + 1911;
                int month = int.Parse(rocDate.Substring(3, 2));
                int day = int.Parse(rocDate.Substring(5, 2));

                return new DateOnly(year, month, day);
            }
            catch
            {
                // 如果发生错误，返回今天的日期
                return DateOnly.FromDateTime(DateTime.Today);
            }
        }
    }
}
