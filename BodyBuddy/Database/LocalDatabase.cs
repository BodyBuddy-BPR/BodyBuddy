using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Database
{
    public class LocalDatabase
    {
        SQLiteAsyncConnection _context;

        public LocalDatabase()
        {
        }

        async Task Init()
        {
            if (_context is not null)
                return;

            _context = new SQLiteAsyncConnection(SQLiteConstants.DatabasePath, SQLiteConstants.Flags);
            var result = await _context.CreateTableAsync<Exercise>();
        }

        #region Exercise
        public async Task<List<Exercise>> GetItemsAsync()
        {
            await Init();
            return await _context.Table<Exercise>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(Exercise item)
        {
            await Init();

            if (item.Id != 0)
                return await _context.UpdateAsync(item);
            else
            {
                item.Id = await GetNextItemId(); // Generate a unique Id
                return await _context.InsertAsync(item);
            }
        }

        private async Task<int> GetNextItemId()
        {
            var lastItem = await _context.Table<Exercise>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }

        public async Task<int> UpdateItemAsync(Exercise item)
        {
            await Init();
            return await _context.UpdateAsync(item);
        }

        public async Task<int> DeleteItemAsync(Exercise item)
        {
            await Init();
            return await _context.DeleteAsync(item);
        }
        #endregion


        #region WorkoutPlan
        public async Task<List<WorkoutPlan>> GetWorkoutPlansAsync()
        {
            await Init();
            return await _context.Table<WorkoutPlan>().ToListAsync();
        }

        public async Task<int> SaveWorkoutPlanAsync(WorkoutPlan item)
        {
            await Init();

            if (item.Id != 0)
                return await _context.UpdateAsync(item);
            else
            {
                item.Id = await GetNextWorkoutPlanId(); // Generate a unique Id
                return await _context.InsertAsync(item);
            }
        }

        private async Task<int> GetNextWorkoutPlanId()
        {
            var lastItem = await _context.Table<WorkoutPlan>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }

        public async Task<int> UpdateWorkoutPlanAsync(WorkoutPlan item)
        {
            await Init();
            return await _context.UpdateAsync(item);
        }

        public async Task<int> DeleteWorkoutPlanAsync(WorkoutPlan item)
        {
            await Init();
            return await _context.DeleteAsync(item);
        }
        #endregion
    }
}
