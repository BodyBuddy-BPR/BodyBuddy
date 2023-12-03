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
        public async Task<List<StepsSupaBaseModel>> GetStepsForPeriodFriends()
        {
            var stepsSupaBaseModels = new List<StepsSupaBaseModel>();

            var userId = SecureStorage.GetAsync("UserUID").Result;

            var friendListModels = await _supabase.From<UserRelationsModel>()
                .Where(x => x.UserId == userId && x.Type == "friend").Get();
            var friends = friendListModels.Models;


            foreach (var friend in friends)
            {
                var stepModel = await _supabase.From<StepsSupaBaseModel>().Where(x => x.Id == friend.FriendId).Get();
                stepsSupaBaseModels.AddRange(stepModel.Models);
            }

            return stepsSupaBaseModels;
        }

        public async void AddOrUpdateSteps(StepDto stepDto)
        {
            var record = new StepsSupaBaseModel
            {
                Id = SecureStorage.GetAsync("UserUID").Result,
                Date = stepDto.Date,
                Steps = stepDto.Steps
            }; 
            await _supabase.From<StepsSupaBaseModel>().Upsert(record);
        }
    }
}
