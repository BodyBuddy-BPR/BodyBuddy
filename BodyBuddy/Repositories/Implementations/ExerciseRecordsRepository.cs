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

        public async Task SaveExerciseRecords(ExerciseRecordsModel exerciseRecord)
        {
            if (exerciseRecord.Id != 0)
                await _context.UpdateAsync(exerciseRecord);

            exerciseRecord.Id = await GetNextExerciseRecordsId(); // Generate a unique Id
            await _context.InsertAsync(exerciseRecord);
        }

        public async Task<List<ExerciseRecordsModel>> GetAllExerciseRecordsForExercise(int exerciseId)
        {
            return await _context.Table<ExerciseRecordsModel>().Where(x => x.ExerciseId == exerciseId)
                .ToListAsync();
        }

        private async Task<int> GetNextExerciseRecordsId()
        {
            var lastItem = await _context.Table<ExerciseRecordsModel>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }
    }
}
