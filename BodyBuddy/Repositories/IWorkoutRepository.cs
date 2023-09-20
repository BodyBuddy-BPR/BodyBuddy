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
        Task<List<Workout>> GetWorkoutPlansAsync();

		Task<List<Exercise>> GetExercisesFromWorkoutId(int workoutId);
		Task SaveWorkoutPlanAsync(Workout workoutPlan);
    }
}
