using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models.Supabase;
using Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class ExerciseRecordSbRepository : IExerciseRecordSbRepository
    {
        private readonly Client _supabase;

        public ExerciseRecordSbRepository(Client client)
        {
            _supabase = client;
        }

        public async Task AddExerciseRecord(ExerciseRecordSbModel exerciseRecordSbModel)
        {
            exerciseRecordSbModel.UserId = SecureStorage.GetAsync("UserUID").Result;

            await _supabase.From<ExerciseRecordSbModel>().Insert(exerciseRecordSbModel);
        }
    }
}
