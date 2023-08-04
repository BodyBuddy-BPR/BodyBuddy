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

        
        #region Exercises

        public async Task<List<Exercise>> GetExercisesAsync(string musclegroup)
        {
            var response = await _supabaseClient.From<Exercise>().Select(x => new object[] {x.Id, x.Name, x.Images, x.Level}).Where(x => x.PrimaryMuscles == musclegroup).Get();
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

        #endregion

        #region Custom Exercises

        public async Task<List<CustomExercise>> GetCustomExercisesAsync()
        {
            var response = await _supabaseClient.From<CustomExercise>().Get();
            var exercises = response.Models;
            return exercises;
        }

        public async Task SaveCustomExerciseAsync(CustomExercise exercise)
        {
            await _supabaseClient.From<CustomExercise>().Insert(exercise);
        }

       

        #endregion
    }
}
