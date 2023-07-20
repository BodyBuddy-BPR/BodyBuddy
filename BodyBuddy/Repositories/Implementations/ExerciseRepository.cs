using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories.Implementations
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly Client _supabaseClient;

        public ExerciseRepository(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<Exercise>> GetExercisesAsync()
        {
            var response = await _supabaseClient.From<Exercise>().Get();
            var exercises = response.Models;
            return exercises;
        }

        public async Task SaveNewExerciseAsync(Exercise exercise)
        {
            await _supabaseClient.From<Exercise>().Insert(exercise);
        }
    }
}
