using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services.Implementations
{
    public class DateTimeService
    {
        public DateTime Today => DateTime.Today;

        public bool IsNewDay(DateTime lastCheckedDate)
        {
            return Today > lastCheckedDate.Date;
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
    }
}
