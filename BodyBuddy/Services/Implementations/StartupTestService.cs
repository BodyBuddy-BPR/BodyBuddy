using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Repositories;
using BodyBuddy.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services.Implementations
{
    public class StartupTestService : IStartupTestService
    {
        private readonly IStartupTestRepository _repo;
        private readonly StartupTestMapper mapper = new();
        public StartupTestService(IStartupTestRepository startupTestRepository)
        {
            _repo = startupTestRepository;
        }

        public async Task<StartupTestDto> GetStartupTestData()
        {
            var startupTestData = await _repo.GetStartupTestData();
            return mapper.MapToDto(startupTestData);
        }

        public void SaveStartupTestData(StartupTestDto startupTestDto)
        {
            _repo.SaveStartupTestData(mapper.MapToDatabase(startupTestDto));
        }
    }
}
