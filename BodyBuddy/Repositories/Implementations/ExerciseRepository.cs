using BodyBuddy.Models;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Postgrest.Constants;

namespace BodyBuddy.Repositories.Implementations
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly Client _supabaseClient;

        public ExerciseRepository(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }


        public async Task<List<Exercise>> GetExercisesAsync(string category, string musclegroup)
        {
            //var response = await _supabaseClient.From<Exercise>().Select(x => new object[] { x.Id, x.Name, x.Images, x.Level }).Where(x => x.Category == category && x.PrimaryMuscles == musclegroup).Get();
            //var response = await _supabaseClient.From<Exercise>().Select(x => new object[] { x.Id, x.Name, x.Images, x.Level })
            //    .Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase)
            //        && x.PrimaryMuscles.Equals(musclegroup, StringComparison.OrdinalIgnoreCase)).Get();

            var response = await _supabaseClient.From<Exercise>().Select(x => new object[] { x.Id, x.Name, x.Images, x.Level })
                .Filter(x => x.Category, Operator.ILike, category).Filter(x => x.PrimaryMuscles, Operator.ILike, musclegroup).Get();

            var exercises = response.Models;
            return exercises;
        }

        public async Task<Exercise> GetExerciseDetailsAsync(int id)
        {
            var response = await _supabaseClient.From<Exercise>().Where(x => x.Id == id).Get();
            var exerciseDetails = response.Model;
            return exerciseDetails;
        }

        // Not in use anymore
        public async Task<List<string>> GetPrimaryMusclesAsync()
        {
            var response = await _supabaseClient.From<Exercise>().Select("primaryMuscles").Get();
            var exercises = response.Models;

            // Use LINQ to extract unique primary muscles
            var primaryMusclesList = exercises
                .Select(exercise => exercise.PrimaryMuscles)
                .Where(primaryMuscle => !string.IsNullOrEmpty(primaryMuscle))
                .Distinct()
                .ToList();

            return primaryMusclesList;
        }

    }
}
