using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Helpers
{
    public static class DateHelper
    {
        private const string _datePreferencesKey = "LastFetchedDate";
        public static DateTime Today => DateTime.Today;

        public static bool IsNewDay()
        {
            var lastFetchedDate = GetLastFetchedDate();

            if (!(Today > lastFetchedDate.Date)) return false;

            UpdateDate();

            return true;
        }

        public static DateTime ConvertToDateTime(long epoch)
        {
            DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epochStart.AddSeconds(epoch);
        }

        public static long ConvertToEpochTime(DateTime dateTime)
        {
            DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //Adding 1 hour for it to be at midnight
            return (long)(dateTime.ToUniversalTime() - epochStart).TotalSeconds+3600;
        }

        //Get current date at midnight in UTC, and convert it to a timestamp
        public static long GetCurrentDayAtMidnight()
        {
            DateTime currentDateTime = DateTime.UtcNow.Date;
            return (long)(currentDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private static DateTime GetLastFetchedDate()
        {
            return Preferences.Get(_datePreferencesKey, DateTime.MinValue);
        }

        private static void UpdateDate()
        {
            Preferences.Set(_datePreferencesKey, Today);
        }
    }
}
