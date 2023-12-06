using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IChallengeSbRepository
    {
        Task<List<ActiveChallengeSbModel>> GetActiveChallenges();
        Task UpdateChallengeData(int activeChallengeId, int progress);

    }
}
