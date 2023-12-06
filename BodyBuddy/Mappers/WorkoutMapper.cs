using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Mappers
{
    public class WorkoutMapper
    {
        public WorkoutModel MapToDatabase(WorkoutDto workoutDto)
        {
            return new WorkoutModel()
            {
                Id = workoutDto.Id,
                Description = workoutDto.Description,
                Name = workoutDto.Name,
                PreMade = workoutDto.PreMade ? 1 : 0
            };
        }

        public WorkoutDto MapToDto(WorkoutModel workoutModel)
        {
            if (workoutModel == null)
                return new WorkoutDto();

            return new WorkoutDto()
            {
                Id = workoutModel.Id,
                Description = workoutModel.Description,
                Name = workoutModel.Name,
                PreMade = workoutModel.PreMade != 0
            };
        }

        //SbModels
        public WorkoutSbModel MapToSbModel(WorkoutDto workoutDto)
        {
            return new WorkoutSbModel()
            {
                Id = workoutDto.Id,
                Description = workoutDto.Description,
                Name = workoutDto.Name,
                PreMade = workoutDto.PreMade
            };
        }
    }
}
