using BodyBuddy.Models;
using SQLite;
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

        //private readonly Client _supabaseClient;
		private readonly SQLiteAsyncConnection _context;

		public WorkoutPlanRepository(SQLiteAsyncConnection context, Client supabaseClient)
        {
			_context = context ?? throw new ArgumentNullException(nameof(context));
			//_supabaseClient = supabaseClient;
        }

        public async Task<List<Workout>> GetWorkoutPlansAsync()
        {
			try
			{
				var workouts = await _context.Table<Workout>().ToListAsync();

				return workouts;
			}
			catch (Exception ex)
			{
				// Handle or log the exception
				Console.WriteLine($"Error in GetExercisesAsync: {ex}");
				return new List<Workout>(); // Return an empty list or handle the error gracefully.
			}
		}

		//Need to remake this method to use SQLite instead of supabase
        //public async Task SaveWorkoutPlanAsync(Workout workoutPlan)
        //{
        //    await _context.Table<Workout>().Insert(workoutPlan);

        //}
    }
}
