using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface ICalendarService
    {
        Task CreateEvent(AppointmentDto newEvent);
        Task<List<AppointmentDto>> GetAppointments();
    }
}
