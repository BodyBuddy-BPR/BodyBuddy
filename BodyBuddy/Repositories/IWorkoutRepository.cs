using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutRepository
    {
        Task<List<Workout>> GetWorkoutPlansAsync(int isPreMade);

		Task<int> PostWorkoutPlanAsync(Workout workout);

		Task DeleteWorkout(Workout workout);

		Task<bool> DoesWorkoutAlreadyExist(string name);

        // WorkoutExercises ---- These should maybe be moved to its own repo
        Task<List<Exercise>> GetExercisesInWorkout(int workoutId, bool isPreMade);

        Task AddExerciseToWorkout(int workoutId, int exerciseId);

        Task EditExerciseInWorkout(int workoutId, Exercise changedExercise);
    }


}
