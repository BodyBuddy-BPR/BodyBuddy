using BodyBuddy.Authentication;
using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Repositories.Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class ExerciseRecordsService : IExerciseRecordsService
    {
        private readonly IExerciseRecordsRepository _exerciseRecordsRepository;
        private readonly IExerciseRecordSbRepository _exerciseRecordSbRepository;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly ExerciseRecordsMapper _mapper = new();

        public ExerciseRecordsService(IExerciseRecordsRepository exerciseRecordsRepository, IExerciseRecordSbRepository exerciseRecordSbRepository, IUserAuthenticationService userAuthenticationService)
        {
            _exerciseRecordsRepository = exerciseRecordsRepository;
            _exerciseRecordSbRepository = exerciseRecordSbRepository;
            _userAuthenticationService = userAuthenticationService;
        }

        public async Task SaveExerciseRecords(ExerciseRecordsDto exerciseRecordsDto)
        {
            exerciseRecordsDto.Date = DateHelper.Now;
            await _exerciseRecordsRepository.SaveExerciseRecords(_mapper.MapToDatabase(exerciseRecordsDto));

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            await _exerciseRecordSbRepository.AddExerciseRecord(_mapper.MapToSbModel(exerciseRecordsDto));
        }

        public async Task<List<ExerciseRecordsDto>> GetAllExerciseRecordsForExercise(int exerciseId)
        {
            var exerciseRecords = await _exerciseRecordsRepository.GetAllExerciseRecordsForExercise(exerciseId);
            return exerciseRecords.Select(exerciseRecord => _mapper.MapToDto(exerciseRecord)).ToList();
        }
    }
}
