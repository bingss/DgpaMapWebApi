namespace DgpaMapWebApi.DaliyProcess.Extensions
{
    public static class StringExtensions
    {
        public static string LimitLength(this string input, int maxLength, bool trimWhitespace = true)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (maxLength < 0) throw new ArgumentException("最大長度不能小於0", nameof(maxLength));

            return input[..Math.Min(input.Trim().Length, maxLength)];
        }

    }
}
