﻿using BodyBuddy.Models;
using SQLite;

namespace BodyBuddy.Repositories.Implementations
{
    public class WorkoutExercisesRepository : IWorkoutExercisesRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public WorkoutExercisesRepository(SQLiteAsyncConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ExerciseModel>> GetExercisesInWorkout(int workoutId)
        {

            List<ExerciseModel> exercises = new();
            try
            {
                var workoutExercises = await _context.Table<WorkoutExercisesModel>().Where(x => x.WorkoutId == workoutId).ToListAsync();
                foreach (var workoutExerciseModel in workoutExercises)
                {
                    var exercise = await _context.Table<ExerciseModel>().FirstOrDefaultAsync(x => x.Id == workoutExerciseModel.ExerciseId);

                    exercise.WorkoutExerciseId = workoutExerciseModel.Id;

                    // Check if the exercise is not null
                    if (workoutExerciseModel.Sets != 0)
                    {
                        // Update the Sets and Reps properties
                        exercise.Sets = workoutExerciseModel.Sets;
                        exercise.Reps = workoutExerciseModel.Reps;
                        exercises.Add(exercise);
                    }
                    else
                    {
                        exercise.Sets = 3;
                        exercise.Reps = 12;
                        exercises.Add(exercise);
                    }
                }
                return exercises;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error in GetExercisesAsync: {ex}");
                return new List<ExerciseModel>(); // Return an empty list or handle the error gracefully.
            }
        }

        public async Task AddExerciseToWorkout(int workoutId, int exerciseId)
        {
            WorkoutExercisesModel workoutExercise = new() { Id = await GetNextId(), WorkoutId = workoutId, ExerciseId = exerciseId };
            await _context.InsertAsync(workoutExercise);
        }

        private async Task<int> GetNextId()
        {
            var lastItem = await _context.Table<WorkoutExercisesModel>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return lastItem?.Id + 1 ?? 1;
        }

        public async Task EditExerciseInWorkout(WorkoutExercisesModel changedWorkoutExercisesModel)
        {
            try
            {
                WorkoutExercisesModel workoutExerciseToChange = await _context.Table<WorkoutExercisesModel>()
                    .FirstOrDefaultAsync(x => x.Id == changedWorkoutExercisesModel.Id);

                if (workoutExerciseToChange != null)
                {
                    // Only updating the Sets and Reps values
                    workoutExerciseToChange.Sets = changedWorkoutExercisesModel.Sets;
                    workoutExerciseToChange.Reps = changedWorkoutExercisesModel.Reps;

                    await _context.UpdateAsync(workoutExerciseToChange);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EditExerciseInWorkout: {ex}");
            }

        }

        public async Task DeleteExerciseFromWorkout(int workoutId, int exerciseId)
        {
            try
            {
                await _context.Table<WorkoutExercisesModel>().DeleteAsync(x => x.WorkoutId == workoutId && x.ExerciseId == exerciseId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EditExerciseInWorkout: {ex}");
            }
        }

        public async Task AddExercisesToWorkouts(List<WorkoutExercisesModel> workoutExerciseModels)
        {
            foreach (var model in workoutExerciseModels)
            {
                model.Id = await GetNextId();
                await _context.InsertAsync(model);
            }
        }
    }
}
