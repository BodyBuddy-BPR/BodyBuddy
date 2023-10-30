using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services
{
    public interface IWorkoutExercisesService
    {
        Task AddExerciseToWorkout(int workoutId, int exerciseId);
    }
}
