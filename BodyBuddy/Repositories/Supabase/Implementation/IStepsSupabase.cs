using BodyBuddy.Dtos;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public interface IStepsSupabase
    {
        Task<List<StepsSbModel>> GetStepsForPeriodFriends();
        void AddOrUpdateSteps(StepDto stepDto);
    }
}
