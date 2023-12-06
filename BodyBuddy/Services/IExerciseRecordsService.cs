using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IExerciseRecordsService
    {
        Task SaveExerciseRecords(ExerciseRecordsDto exerciseRecordsDto);
        Task<List<ExerciseRecordsDto>> GetAllExerciseRecordsForExercise(int exerciseId);
    }
}
