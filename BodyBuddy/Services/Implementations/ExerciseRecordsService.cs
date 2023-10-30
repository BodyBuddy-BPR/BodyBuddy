using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Mappers;
using BodyBuddy.Models;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class ExerciseRecordsService : IExerciseRecordsService
    {
        private readonly IExerciseRecordsRepository _repo;
        private readonly ExerciseRecordsMapper _mapper;

        public ExerciseRecordsService(IExerciseRecordsRepository repo)
        {
            _repo = repo;
            _mapper = new ExerciseRecordsMapper();
        }
        public async Task SaveExerciseRecords(ExerciseRecordsDto exerciseRecordsDto)
        {
            await _repo.SaveExerciseRecords(_mapper.MapToDatabase(exerciseRecordsDto));
        }
    }
}
