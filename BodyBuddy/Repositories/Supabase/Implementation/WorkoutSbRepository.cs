using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models.Supabase;
using Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class WorkoutSbRepository : IWorkoutSbRepository
    {
        private readonly Client _supabase;

        public WorkoutSbRepository(Client client)
        {
            _supabase = client;
        }

        public async Task<List<WorkoutSbModel>> GetAllWorkoutsForProfile()
        {
            try
            {
                var userId = SecureStorage.GetAsync("UserUID").Result;

                var stepModel = await _supabase.From<WorkoutSbModel>().Where(x => x.UserId == userId).Get();

                return stepModel.Models;
            }
            catch (Exception ex)
            {
                return new List<WorkoutSbModel>();
            }
        }

        public async Task<List<WorkoutExerciseSbModel>> GetAllWorkoutExercisesForProfile()
        {
            throw new NotImplementedException();
        }

        public async Task AddOrUpdateWorkout(WorkoutSbModel model)
        {
            model.UserId = SecureStorage.GetAsync("UserUID").Result;
            try
            {
                await _supabase.From<WorkoutSbModel>().Upsert(model);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task AddOrUpdateWorkoutExercise(WorkoutExerciseSbModel workoutExerciseSbModel)
        {
            try
            {
                await _supabase.From<WorkoutExerciseSbModel>().Upsert(workoutExerciseSbModel);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task RemoveWorkoutExercise(int workoutId, int exerciseId)
        {
            try
            {
                await _supabase.From<WorkoutExerciseSbModel>()
                    .Where(x => x.WorkoutId == workoutId && x.ExerciseId == exerciseId).Delete();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
