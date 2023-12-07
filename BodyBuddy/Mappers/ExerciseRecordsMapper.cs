using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Models;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Mappers
{
    public class ExerciseRecordsMapper
    {
        public ExerciseRecordsModel MapToDatabase(ExerciseRecordsDto exerciseRecordsDto)
        {
            return new ExerciseRecordsModel
            {
                Id = exerciseRecordsDto.Id,
                ExerciseId = exerciseRecordsDto.ExerciseId,
                Set = exerciseRecordsDto.Set,
                Weight = exerciseRecordsDto.Weight,
                Reps = exerciseRecordsDto.Reps,
                Date = DateHelper.ConvertToEpochTimeAtMidnightUtc(exerciseRecordsDto.Date)
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
                Date = DateHelper.ConvertToDateTime(exerciseRecordsModel.Date)
            };
        }

        // Supabase
        public ExerciseRecordSbModel MapToSbModel(ExerciseRecordsDto exerciseRecordsDto)
        {
            return new ExerciseRecordSbModel
            {
                ExerciseId = exerciseRecordsDto.ExerciseId,
                Set = exerciseRecordsDto.Set,
                Weight = exerciseRecordsDto.Weight,
                Reps = exerciseRecordsDto.Reps,
                Date = exerciseRecordsDto.Date
            };
        }

        public ExerciseRecordsModel MapToDatabaseFromSbModel(ExerciseRecordSbModel exerciseRecordSbModel)
        {
            return new ExerciseRecordsModel
            {
                ExerciseId = exerciseRecordSbModel.ExerciseId,
                Set = exerciseRecordSbModel.Set,
                Weight = exerciseRecordSbModel.Weight,
                Reps = exerciseRecordSbModel.Reps,
                Date = DateHelper.ConvertToEpochTimeAtMidnightUtc(exerciseRecordSbModel.Date)
            };
        }
    }
}
