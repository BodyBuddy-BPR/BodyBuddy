using BodyBuddy.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services
{
    public interface ICalendarService
    {
        Task CreateEvent(AppointmentDto newEvent);
        Task<List<AppointmentDto>> GetAppointments();
    }
}
