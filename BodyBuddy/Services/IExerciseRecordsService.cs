using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IExerciseRecordsService
    {
        Task SaveExerciseRecords(ExerciseRecordsDto exerciseRecordsDto);
        Task<List<ExerciseRecordsDto>> GetAllExerciseRecordsForExercise(int exerciseId);
    }
}
