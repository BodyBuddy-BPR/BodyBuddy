using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Mappers;
using BodyBuddy.Models;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class ExerciseRecordsService : IExerciseRecordsService
    {
        private readonly IExerciseRecordsRepository _repo;
        private readonly ExerciseRecordsMapper _mapper;
        private readonly DateHelper _dateHelper = new();

        public ExerciseRecordsService(IExerciseRecordsRepository repo)
        {
            _repo = repo;
            _mapper = new ExerciseRecordsMapper();
        }
        public async Task SaveExerciseRecords(ExerciseRecordsDto exerciseRecordsDto)
        {
            exerciseRecordsDto.Date = _dateHelper.Now;
            await _repo.SaveExerciseRecords(_mapper.MapToDatabase(exerciseRecordsDto));
        }

        public async Task<List<ExerciseRecordsDto>> GetAllExerciseRecordsForExercise(int exerciseId)
        {
            var exerciseRecords = await _repo.GetAllExerciseRecordsForExercise(exerciseId);
            return exerciseRecords.Select(exerciseRecord => _mapper.MapToDto(exerciseRecord)).ToList();
        }
    }
}
