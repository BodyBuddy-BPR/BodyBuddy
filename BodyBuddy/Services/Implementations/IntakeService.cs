using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class IntakeService : IIntakeService
    {
        private readonly IIntakeRepository _repo;
        private readonly IntakeMapper _mapper = new();

        public IntakeService(IIntakeRepository intakeRepository)
        {
            _repo = intakeRepository;
        }
        public async Task<IntakeDto> GetIntakeAsync()
        {
            return _mapper.MapToDto(await _repo.GetCurrentDayIntakeAsync());
        }

		public async Task<IntakeDto> GetIntakeForDateAsync(int dateTimeUTC)
		{
			return _mapper.MapToDto(await _repo.GetIntakeForDateAsync(dateTimeUTC));
		}

        public async Task<List<IntakeDto>> GetAllIntakeDataAsync()
        {
            var intakeModels = await _repo.GetAllIntakeDataAsync();

            //Converting each intakeModel into intakeDto
            return intakeModels.Select(intakeModel => _mapper.MapToDto(intakeModel)).ToList();
        }

        public async Task SaveChangesAsync(IntakeDto intakeDetails)
        {
            await _repo.SaveChangesAsync(_mapper.MapToDatabase(intakeDetails));
        }
    }
}
