using System.Globalization;

namespace BackPack.Library.Helpers.Common
{
    public static class Date
    {
        #region IsValidTargetDateFormat        
        public static bool IsValidTargetDateFormat(string TargetDate)
        {
            return DateTime.TryParseExact(TargetDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out _);
        }
        #endregion

        #region IsTargetDateGreater
        public static bool IsTargetDateGreater(string Date)
        {
            DateTime.TryParseExact(Date, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime TargetDate);
            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            return TargetDate > CurrentDate;
        }
        #endregion
    }
}
