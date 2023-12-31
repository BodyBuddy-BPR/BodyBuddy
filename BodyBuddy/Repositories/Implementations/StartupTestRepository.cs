﻿using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class StartupTestRepository : IStartupTestRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public StartupTestRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<StartupTestModel> GetStartupTestData()
        {
            return await _context.Table<StartupTestModel>().FirstOrDefaultAsync();
        }

        public async Task SaveStartupTestData(StartupTestModel startupTest)
        {
            if (startupTest.Id == 0)
                await _context.InsertAsync(startupTest);
            else
                await _context.UpdateAsync(startupTest);
        }

        public async Task ClearSQLiteData()
        {
            await _context.DeleteAllAsync<StartupTestModel>();
        }
    }
}
