using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutExercisesRepository
    {
        Task<List<Exercise>> GetExercisesInWorkout(int workoutId, bool isPreMade);

        Task AddExerciseToWorkout(int workoutId, int exerciseId);

        Task EditExerciseInWorkout(int workoutId, Exercise changedExercise);

        Task DeleteExerciseFromWorkout(int workoutId, int exerciseId);
    }
}
