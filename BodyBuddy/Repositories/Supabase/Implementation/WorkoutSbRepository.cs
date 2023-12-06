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

        public async Task<List<WorkoutSbModel>> GetAllForProfile()
        {
            throw new NotImplementedException();
        }

        public async Task AddWorkout(WorkoutSbModel model)
        {
            throw new NotImplementedException();
        }

        public async Task AddWorkoutExercises(List<WorkoutExerciseSbModel> workoutExerciseSbModels)
        {
            throw new NotImplementedException();
        }
    }
}
