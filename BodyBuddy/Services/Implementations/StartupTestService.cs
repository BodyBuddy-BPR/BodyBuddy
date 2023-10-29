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
        private IStartupTestRepository _repo;
        private StartupTestMapper mapper;
        public StartupTestService(IStartupTestRepository startupTestRepository, StartupTestMapper startupTestMapper)
        {
            _repo = startupTestRepository;
            mapper = startupTestMapper;
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
