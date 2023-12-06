using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public CalendarRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateEvent(AppointmentModel newEvent)
        {
            var lastItem = await _context.Table<AppointmentModel>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            newEvent.Id = lastItem?.Id + 1 ?? 1;

            await _context.InsertAsync(newEvent);
        }

        public async Task<List<AppointmentModel>> GetAppointments()
        {
            try
            {
                var appointments = await _context.Table<AppointmentModel>().ToListAsync();

                return appointments;
            }
            catch (Exception)
            {
                return new List<AppointmentModel>(); // Return an empty list
            }
        }
    }
}
