using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IExerciseRecordsRepository
    {
        Task<int> SaveExerciseRecords(ExerciseRecords exerciseRecord);

    }
}
