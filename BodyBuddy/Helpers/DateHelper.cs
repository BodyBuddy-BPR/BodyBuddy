using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Helpers
{
    public class DateHelper
    {
        private readonly string _datePreferencesKey = "LastFetchedDate";
        public DateTime Today => DateTime.Today;
        public DateTime Now => DateTime.Now;

        public bool IsNewDay()
        {
            var lastFetchedDate = GetLastFetchedDate();

            if (!(Today > lastFetchedDate.Date)) return false;

            UpdateDate();

            return true;
        }

        public DateTime ConvertToDateTime(long epoch)
        {
            DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epochStart.AddSeconds(epoch);
        }

        public long ConvertToEpochTime(DateTime dateTime)
        {
            DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - epochStart).TotalSeconds;
        }

        private DateTime GetLastFetchedDate()
        {
            return Preferences.Get(_datePreferencesKey, DateTime.MinValue);
        }

        private void UpdateDate()
        {
            Preferences.Set(_datePreferencesKey, Today);
        }
    }
}
