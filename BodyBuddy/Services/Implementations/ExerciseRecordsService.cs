using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class ExerciseRecordsService : IExerciseRecordsService
    {
        private readonly IExerciseRecordsRepository _repo;
        private readonly ExerciseRecordsMapper _mapper = new();

        public ExerciseRecordsService(IExerciseRecordsRepository repo)
        {
            _repo = repo;
        }
        public async Task SaveExerciseRecords(ExerciseRecordsDto exerciseRecordsDto)
        {
            exerciseRecordsDto.Date = DateHelper.Now;
            await _repo.SaveExerciseRecords(_mapper.MapToDatabase(exerciseRecordsDto));
        }

        public async Task<List<ExerciseRecordsDto>> GetAllExerciseRecordsForExercise(int exerciseId)
        {
            var exerciseRecords = await _repo.GetAllExerciseRecordsForExercise(exerciseId);
            return exerciseRecords.Select(exerciseRecord => _mapper.MapToDto(exerciseRecord)).ToList();
        }
    }
}
