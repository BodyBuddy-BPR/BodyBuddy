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
            try
            {
                await _supabase.From<ExerciseRecordSbModel>().Insert(exerciseRecordSbModel);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<ExerciseRecordSbModel>> GetAllExerciseRecordsForUser()
        {
            try
            {
                var userId = SecureStorage.GetAsync("UserUID").Result;

                var exerciseRecordsModel = await _supabase.From<ExerciseRecordSbModel>().Where(x => x.UserId == userId).Get();

                return exerciseRecordsModel.Models;
            }
            catch (Exception ex)
            {
                return new List<ExerciseRecordSbModel>();
            }
        }
    }
}
