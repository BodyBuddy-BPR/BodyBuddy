﻿using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class ExerciseRepository : IExerciseRepository
    {
        //private readonly Client _supabaseClient;
        private readonly SQLiteAsyncConnection _context;

        public ExerciseRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_supabaseClient = supabaseClient;
        }

        public async Task<List<ExerciseModel>> GetExercisesAsync(string category, string musclegroup)
        {
            try
            {
                var exercises = await _context.Table<ExerciseModel>()
                    .Where(x => x.Category == category && x.PrimaryMuscles == musclegroup).ToListAsync();

                return exercises;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error in GetExercisesAsync: {ex}");
                return new List<ExerciseModel>(); // Return an empty list or handle the error gracefully.
            }
        }

        public async Task<List<string>> GetMuscleGroupsForCategory(string category)
        {
            try
            {
                var sql = $"SELECT DISTINCT primaryMuscles FROM Exercise WHERE category = '{category}'";

                var distinctMuscleGroupsObjects = await _context.QueryAsync<ExerciseModel>(sql);

                // Convert the objects to strings
                var distinctMuscleGroups = distinctMuscleGroupsObjects.Select(x => x.PrimaryMuscles).ToList();

                return distinctMuscleGroups;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error in GetMuscleGroupsForCategory: {ex}");
                return new List<string>(); // Return an empty list or handle the error gracefully.
            }
        }


        public async Task<List<string>> GetUniqueCategoriesAsync()
        {
            var sql = "SELECT DISTINCT category FROM Exercise";

            var distinctCategories = await _context.QueryAsync<ExerciseModel>(sql);

            var categories = distinctCategories.Select(x => x.Category).ToList();

            return categories;
        }

        public async Task<ExerciseModel> GetExerciseDetails(int id)
        {
            var exercise = await _context.Table<ExerciseModel>().FirstOrDefaultAsync(x => x.Id == id);
            return exercise;
        }

        #region Supabase methods

        //public async Task<List<Exercise>> GetExercisesAsync(string category, string musclegroup)
        //{


        //    var response = await _supabaseClient.From<Exercise>().Select(x => new object[] { x.Id, x.Name, x.Images, x.Level })
        //        .Filter(x => x.Category, Operator.ILike, category).Filter(x => x.PrimaryMuscles, Operator.ILike, musclegroup).Get();

        //    var exercises = response.Models;
        //    return exercises;
        //}

        //public async Task<Exercise> GetExerciseDetailsAsync(int id)
        //{
        //    var response = await _supabaseClient.From<Exercise>().Where(x => x.Id == id).Get();
        //    var exerciseDetails = response.Model;
        //    return exerciseDetails;
        //}

        // Not in use anymore
        //public async Task<List<string>> GetPrimaryMusclesAsync()
        //{
        //    var response = await _supabaseClient.From<Exercise>().Select("primaryMuscles").Get();
        //    var exercises = response.Models;

        //    // Use LINQ to extract unique primary muscles
        //    var primaryMusclesList = exercises
        //        .Select(exercise => exercise.PrimaryMuscles)
        //        .Where(primaryMuscle => !string.IsNullOrEmpty(primaryMuscle))
        //        .Distinct()
        //        .ToList();

        //    return primaryMusclesList;
        //}
        #endregion
    }
}
