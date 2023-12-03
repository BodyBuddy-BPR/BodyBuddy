using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.SupaBaseModels;
using Supabase;

namespace BodyBuddy.SupaBase
{
    public class StepsSupaBase : IStepsSupaBase
    {
        private readonly Client _supabase;

        public StepsSupaBase(Client client)
        {
            _supabase = client; 
        }
        public StepModel GetStepsForPeriodFriends()
        {
            throw new NotImplementedException();
        }

        public async void AddOrUpdateSteps(StepDto stepDto)
        {
            //var testId = SecureStorage.GetAsync("UserUIDKey").Result;
            var testId = "ca1f7124-9763-4f1d-848c-6661adf4689a";
            var record = new StepsSupaBaseModel
            {
                Id = testId,
                Date = stepDto.Date,
                Steps = stepDto.Steps
            }; 
            await _supabase.From<StepsSupaBaseModel>().Upsert(record);
        }
    }
}
