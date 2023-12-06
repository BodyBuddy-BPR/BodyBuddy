using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface ICalendarRepository
    {
        Task CreateEvent(AppointmentModel newEvent);
        Task<List<AppointmentModel>> GetAppointments();
    }
}
