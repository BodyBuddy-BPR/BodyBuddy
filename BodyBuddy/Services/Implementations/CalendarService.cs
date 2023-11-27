using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Models;
using BodyBuddy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<AppointmentDto>> GetAppointments()
        {
            List<AppointmentModel> appointments;

            appointments = await _calendarRepository.GetAppointments();

            return appointments.Select(model => _mapper.MapToDto(model)).ToList();
        }
    }
}
