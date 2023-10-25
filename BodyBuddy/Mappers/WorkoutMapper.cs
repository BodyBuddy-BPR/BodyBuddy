using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;
using Microsoft.Maui.Layouts;

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
                PreMade = workoutModel.PreMade == 1
            };
        }
    }
}
