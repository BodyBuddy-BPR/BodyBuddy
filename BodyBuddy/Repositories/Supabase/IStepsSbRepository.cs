using BodyBuddy.Dtos;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IStepsSbRepository
    {
        Task<List<StepsSbModel>> GetStepsForPeriodFriends();
        Task AddOrUpdateSteps(StepDto stepDto);
    }
}
