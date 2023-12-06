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
        Task<List<WorkoutSbModel>> GetAllForProfile();
        Task AddWorkout(WorkoutSbModel model);
        Task AddWorkoutExercises(List<WorkoutExerciseSbModel> workoutExerciseSbModels);
    }
}
