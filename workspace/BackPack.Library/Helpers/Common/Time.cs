namespace BackPack.Library.Helpers.Common
{
    public static class Time
    {
        public static TimeToDay TimeToDay(string Time)
        {
            TimeToDay respnse = new();
            if (Time == null || Time == "")
            {
                return respnse;
            }
            else
            {
                string[] arrTime = Time.Split(':');
                _ = int.TryParse(arrTime[0].ToString(), out int Min);
                _ = int.TryParse(arrTime[1].ToString(), out int Sec);

                int TotalSec = (Min * 60) + Sec;
                int Days = TotalSec / 60 / 60 / 24;                

                if (Days > 0)
                {
                    int DaySec = Days * 60 * 60 * 24;
                    Min = (TotalSec - DaySec) / 60;
                }                

                int Hour = Min / 60;

                if (Hour > 0)
                {
                    Min = Min - (Hour * 60);
                }

                respnse.Days = Days;
                respnse.Hour = Hour;
                respnse.Min = Min;
                respnse.Sec = Sec;
                respnse.TotalSec = TotalSec;
                respnse.Time = Time;

                return respnse;
            }            
        }
    }

    public class TimeToDay
    {
        public int Days { get; set; }
        public int Hour { get; set; }
        public int Min { get; set; }
        public int Sec { get; set; }
        public int TotalSec { get; set; }
        public string? Time { get; set; }
    }
}
