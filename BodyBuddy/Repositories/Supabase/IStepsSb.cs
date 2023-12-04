using BodyBuddy.Dtos;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IStepsSb
    {
        Task<List<StepsSbModel>> GetStepsForPeriodFriends();
        Task AddOrUpdateSteps(StepDto stepDto);
    }
}
