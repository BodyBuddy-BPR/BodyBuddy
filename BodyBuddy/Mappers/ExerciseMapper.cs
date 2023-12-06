using BodyBuddy.Dtos;
using BodyBuddy.Models;

namespace BodyBuddy.Mappers
{
    public class ExerciseMapper
    {
        private readonly Dictionary<string, string> _primaryMuscleToTargetAreaMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Shoulders", "Upper Body" },
            { "Chest", "Upper Body" },
            { "Traps", "Upper Body" },
            { "Neck", "Upper Body" },
            { "Biceps", "Arms" },
            { "Forearms", "Arms" },
            { "Triceps", "Arms" },
            { "Middle back", "Back" },
            { "Lower back", "Back" },
            { "Lats", "Back" },
            { "Abdominals", "Abs and Core" },
            { "Hamstrings", "Lower Body" },
            { "Adductors", "Lower Body" },
            { "Quadriceps", "Lower Body" },
            { "Glutes", "Lower Body" },
            { "Calves", "Lower Body" },
            { "Abductors", "Lower Body" },
        };



        /// <summary>
        /// Used to map primary muscles into target areas, to provide grouping
        /// </summary>
        /// <param name="primaryMuscle"></param>
        /// <returns>Target Area and "Unknown" if there is no mapping</returns>
        public string MapPrimaryMuscleToTargetArea(string primaryMuscle)
        {
            if (_primaryMuscleToTargetAreaMap.TryGetValue(primaryMuscle, out var targetArea))
            {
                return targetArea;
            }
            return "Unknown";
        }

        public ExerciseModel MapToDatabase(ExerciseDto exerciseDto)
        {
            return new ExerciseModel()
            {
                Id = exerciseDto.Id,
                Name = exerciseDto.Name,
                Force = exerciseDto.Force,
                Level = exerciseDto.Level,
                Mechanic = exerciseDto.Mechanic,
                Equipment = exerciseDto.Equipment,
                PrimaryMuscles = exerciseDto.PrimaryMuscles,
                SecondaryMuscles = exerciseDto.SecondaryMuscles,
                Instructions = exerciseDto.Instructions,
                Category = exerciseDto.Category,
                Images = exerciseDto.Images,
                WorkoutExerciseId = exerciseDto.WorkoutExerciseId,
                Sets = exerciseDto.Sets,
                Reps = exerciseDto.Reps,
            };
        }

        public ExerciseDto MapToDto(ExerciseModel exerciseModel)
        {
            if (exerciseModel == null)
                return new ExerciseDto();

            return new ExerciseDto()
            {
                Id = exerciseModel.Id,
                WorkoutId = exerciseModel.WorkoutId,
                WorkoutExerciseId = exerciseModel.WorkoutExerciseId,
                Name = exerciseModel.Name,
                Force = exerciseModel.Force,
                Level = exerciseModel.Level,
                Mechanic = exerciseModel.Mechanic,
                Equipment = exerciseModel.Equipment,
                PrimaryMuscles = exerciseModel.PrimaryMuscles,
                SecondaryMuscles = exerciseModel.SecondaryMuscles,
                Instructions = exerciseModel.Instructions,
                Category = exerciseModel.Category,
                Images = exerciseModel.Images,
                Sets = exerciseModel.Sets,
                Reps = exerciseModel.Reps,
            };
        }

        public WorkoutExercisesModel MapToWorkoutExerciseModel(ExerciseDto exerciseDto)
        {
            return new WorkoutExercisesModel()
            {
                Id = exerciseDto.WorkoutExerciseId,
                ExerciseId = exerciseDto.Id,
                Reps = exerciseDto.Reps,
                Sets = exerciseDto.Sets
            };
        }
    }
}
