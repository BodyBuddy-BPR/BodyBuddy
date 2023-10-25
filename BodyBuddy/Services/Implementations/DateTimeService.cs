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
    }
}
