using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Repositories;

namespace BodyBuddy.Services.Implementations
{
    public class WorkoutExercisesService : IWorkoutExercisesService
    {
        private IWorkoutExercisesRepository _repo;

        public WorkoutExercisesService(IWorkoutExercisesRepository repo)
        {
            _repo = repo;
        }
        public async Task AddExerciseToWorkout(int workoutId, int exerciseId)
        {
            await _repo.AddExerciseToWorkout(workoutId, exerciseId);
        }
    }
}
