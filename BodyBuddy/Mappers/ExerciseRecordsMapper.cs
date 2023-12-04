using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;

namespace BodyBuddy.Mappers
{
    public class ExerciseRecordsMapper
    {
        private readonly DateHelper _dateHelper = new ();
        public ExerciseRecordsModel MapToDatabase(ExerciseRecordsDto exerciseRecordsDto)
        {
            return new ExerciseRecordsModel
            {
                Id = exerciseRecordsDto.Id,
                ExerciseId = exerciseRecordsDto.ExerciseId,
                Set = exerciseRecordsDto.Set,
                Weight = exerciseRecordsDto.Weight,
                Reps = exerciseRecordsDto.Reps,
                Date = _dateHelper.ConvertToEpochTime(exerciseRecordsDto.Date)
            };
        }

        public ExerciseRecordsDto MapToDto(ExerciseRecordsModel exerciseRecordsModel)
        {
            if (exerciseRecordsModel == null)
                return new ExerciseRecordsDto();

            return new ExerciseRecordsDto()
            {
                Id = exerciseRecordsModel.Id,
                ExerciseId = exerciseRecordsModel.ExerciseId,
                Set = exerciseRecordsModel.Set,
                Weight = exerciseRecordsModel.Weight,
                Reps = exerciseRecordsModel.Reps,
                Date = _dateHelper.ConvertToDateTime(exerciseRecordsModel.Date)
            };
        }
    }
}
