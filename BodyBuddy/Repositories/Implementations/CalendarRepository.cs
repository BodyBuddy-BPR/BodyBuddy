using BodyBuddy.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories.Implementations
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public CalendarRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<AppointmentModel>> GetAppointments()
        {
            try
            {
                var appointments = await _context.Table<AppointmentModel>().ToListAsync();

                return appointments;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return new List<AppointmentModel>(); // Return an empty list
            }
        }
    }
}
