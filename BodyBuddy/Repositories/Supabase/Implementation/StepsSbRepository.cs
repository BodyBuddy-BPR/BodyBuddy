using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Models.Supabase;
using Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class StepsSbRepository : IStepsSbRepository
    {
        private readonly Client _supabase;

        public StepsSbRepository(Client client)
        {
            _supabase = client;
        }
        public async Task<List<StepsSbModel>> GetStepsForPeriodFriends()
        {
            var stepsSupaBaseModels = new List<StepsSbModel>();

            var userId = SecureStorage.GetAsync("UserUID").Result;

            var friendListModels = await _supabase.From<UserRelationsModel>()
                .Where(x => x.UserId == userId && x.Type == "friend").Get();
            var friends = friendListModels.Models;


            foreach (var friend in friends)
            {
                var stepModel = await _supabase.From<StepsSbModel>().Where(x => x.Id == friend.FriendId).Get();
                stepsSupaBaseModels.AddRange(stepModel.Models);
            }

            return stepsSupaBaseModels;
        }

        public async Task AddOrUpdateSteps(StepDto stepDto)
        {
            var record = new StepsSbModel
            {
                Id = SecureStorage.GetAsync("UserUID").Result,
                Date = stepDto.Date,
                Steps = stepDto.Steps
            };
            await _supabase.From<StepsSbModel>().Upsert(record);
        }
    }
}
