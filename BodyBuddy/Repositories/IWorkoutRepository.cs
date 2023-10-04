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
    }


}
