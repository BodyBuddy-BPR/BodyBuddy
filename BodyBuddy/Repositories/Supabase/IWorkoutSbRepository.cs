using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IWorkoutSbRepository
    {
        Task<List<WorkoutSbModel>> GetAllWorkoutsForProfile();
        Task<List<WorkoutExerciseSbModel>> GetAllWorkoutExercisesForProfile();
        Task AddOrUpdateWorkout(WorkoutSbModel model);
        Task AddOrUpdateWorkoutExercise(WorkoutExerciseSbModel workoutExerciseSbModel);
        Task RemoveWorkoutExercise(int workoutId, int exerciseId);
    }
}
