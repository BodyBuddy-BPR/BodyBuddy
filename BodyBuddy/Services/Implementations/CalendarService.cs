using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Models;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly AppointmentMapper _mapper = new();

        public CalendarService(ICalendarRepository calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task CreateEvent(AppointmentDto newEvent)
        {
            await _calendarRepository.CreateEvent(_mapper.MapToModel(newEvent));
        }

        public async Task<List<AppointmentDto>> GetAppointments()
        {
            List<AppointmentModel> appointments;

            appointments = await _calendarRepository.GetAppointments();

            return appointments.Select(model => _mapper.MapToDto(model)).ToList();
        }
    }
}
