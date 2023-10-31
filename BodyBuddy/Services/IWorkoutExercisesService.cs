using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;

namespace BodyBuddy.Services
{
    public interface IWorkoutExercisesService
    {
        Task AddExerciseToWorkout(int workoutId, int exerciseId);

        Task<List<ExerciseDto>> GetExercisesInWorkout(int workoutId);

        Task EditExerciseInWorkout(ExerciseDto changedExercise);

        Task DeleteExerciseFromWorkout(int workoutId, int exerciseId);
    }
}
