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

		Task<List<Exercise>> GetExercisesInWorkout(int workoutId, bool isPreMade);

		Task<int> PostWorkoutPlanAsync(Workout workout);

		Task DeleteWorkout(Workout workout);
	}
}
