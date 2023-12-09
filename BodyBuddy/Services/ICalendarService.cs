using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface ICalendarService
    {
        /// <summary>
        /// Used to add an event into the database
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns></returns>
        Task CreateEvent(AppointmentDto newEvent);

        /// <summary>
        /// Getting appointments from local db
        /// </summary>
        /// <returns>List of created appointments</returns>
        Task<List<AppointmentDto>> GetAppointments();
    }
}
