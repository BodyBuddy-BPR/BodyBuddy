using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IExerciseRecordsRepository
    {
        Task SaveExerciseRecords(ExerciseRecordsModel exerciseRecord);
        Task<List<ExerciseRecordsModel>> GetAllExerciseRecordsForExercise(int exerciseId);

        //Supabase
        Task ClearSQLiteData();
        Task AddListOfExerciseRecords(List<ExerciseRecordsModel> exerciseRecordsModels);
    }
}
