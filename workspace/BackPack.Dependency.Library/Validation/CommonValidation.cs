using System.Globalization;

namespace BackPack.Dependency.Library.Validation
{
    public static class CommonValidation
    {
        #region CheckDateFormat
        public static bool CheckDateFormat(string date)
        {
            bool response = DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out _);
            return response;
        }
        #endregion
    }
}
