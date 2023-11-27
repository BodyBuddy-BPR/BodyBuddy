using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories
{
    public interface ICalendarRepository
    {
        Task<List<AppointmentModel>> GetAppointments();
    }
}
