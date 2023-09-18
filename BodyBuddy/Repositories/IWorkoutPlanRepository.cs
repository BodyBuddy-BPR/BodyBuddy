using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutPlanRepository
    {
        Task<List<Workout>> GetWorkoutPlansAsync();

        //Need to change implementation of this method to use SQLite instead of Supabase
        //Task SaveWorkoutPlanAsync(WorkoutPlan workoutPlan);


    }
}
