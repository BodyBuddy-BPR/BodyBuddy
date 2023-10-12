using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class ExerciseRecordsRepository : IExerciseRecordsRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public ExerciseRecordsRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<int> SaveExerciseRecords(ExerciseRecords exerciseRecord)
        {
            if (exerciseRecord.Id != 0)
                return await _context.UpdateAsync(exerciseRecord);
            else
            {
                exerciseRecord.Id = await GetNextExerciseRecordsId(); // Generate a unique Id
                return await _context.InsertAsync(exerciseRecord);
            }
        }
        private async Task<int> GetNextExerciseRecordsId()
        {
            var lastItem = await _context.Table<ExerciseRecords>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }
    }
}
