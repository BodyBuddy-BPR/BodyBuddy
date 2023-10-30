using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IExerciseRecordsRepository
    {
        Task SaveExerciseRecords(ExerciseRecordsModel exerciseRecord);

    }
}
