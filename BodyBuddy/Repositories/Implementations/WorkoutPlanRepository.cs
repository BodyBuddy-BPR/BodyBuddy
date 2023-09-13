using BodyBuddy.Models;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories.Implementations
{
    public class WorkoutPlanRepository : IWorkoutPlanRepository
    {
        private readonly Client _supabaseClient;

        public WorkoutPlanRepository(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<WorkoutPlan>> GetWorkoutPlansAsync()
        {
            var response = await _supabaseClient.From<WorkoutPlan>().Get();
            var workoutPlans = response.Models;
            return workoutPlans;
        }

        public async Task SaveWorkoutPlanAsync(WorkoutPlan workoutPlan)
        {
            await _supabaseClient.From<WorkoutPlan>().Insert(workoutPlan);

        }
    }
}
