using BodyBuddy.Authentication;
using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Repositories.Supabase;

namespace BodyBuddy.Services.Implementations
{
    public class IntakeService : IIntakeService
    {
        private readonly IIntakeRepository _intakeRepository;
        private readonly IIntakeSbRepository _intakeSbRepository;
        private readonly IUserAuthenticationService _userAuthenticationService;

        private readonly IntakeMapper _mapper = new();

        public IntakeService(IIntakeRepository intakeRepository, IIntakeSbRepository intakeSbRepository, IUserAuthenticationService userAuthenticationService)
        {
            _intakeRepository = intakeRepository;
            _intakeSbRepository = intakeSbRepository;
            _userAuthenticationService = userAuthenticationService;
        }
        public async Task<IntakeDto> GetIntakeTodayAsync()
        {
            return _mapper.MapToDto(await _intakeRepository.GetIntakeForDateAsync(DateHelper.GetCurrentDayAtMidnight()));
        }

        public async Task<IntakeDto> GetIntakeForDateAsync(long dateTimeUTC)
        {
            return _mapper.MapToDto(await _intakeRepository.GetIntakeForDateAsync(dateTimeUTC));
        }

        public async Task<List<IntakeDto>> GetAllIntakeDataAsync()
        {
            var intakeModels = await _intakeRepository.GetAllIntakeDataAsync();

            //Converting each intakeModel into intakeDto
            return intakeModels.Select(intakeModel => _mapper.MapToDto(intakeModel)).ToList();
        }

        public async Task SaveChangesAsync(IntakeDto intakeDetails)
        {
            await _intakeRepository.SaveChangesAsync(_mapper.MapToDatabase(intakeDetails));

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;
            await _intakeSbRepository.AddOrUpdateIntake(_mapper.MapToSbModel(intakeDetails));
        }

        public async Task ReplaceSQLiteDataWithRemoteData()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet || !_userAuthenticationService.IsUserLoggedIn())
                return;

            await _intakeRepository.ClearSQLiteData();

            var supabaseData = await _intakeSbRepository.GetAllForProfile();

            var intakeModels = supabaseData.Select(intake => _mapper.MapToDatabaseFromSb(intake)).ToList();

            if (intakeModels.Any())
                await _intakeRepository.AddListOfIntakeData(intakeModels);
        }
    }
}
