using BodyBuddy.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories.Implementations
{
    public class StartupTestRepository : IStartupTestRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public StartupTestRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<StartupTest> GetStartupTestData()
        {
            return await _context.Table<StartupTest>().FirstOrDefaultAsync();
        }

        public async Task SaveStartupTestData(StartupTest startupTest)
        {
            if (startupTest.Id == 0)
                await _context.InsertAsync(startupTest);
            else
                await _context.UpdateAsync(startupTest);
        }
    }
}
