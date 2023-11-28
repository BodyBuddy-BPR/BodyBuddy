
using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IStepRepository
    {
        Task<StepModel> GetStepsAsync();
        Task SaveChangesAsync(StepModel stepDetails);
    }
}
