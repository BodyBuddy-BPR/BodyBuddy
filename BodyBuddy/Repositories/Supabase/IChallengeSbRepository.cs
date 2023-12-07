using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IChallengeSbRepository
    {
        /// <summary>
        /// Getting all active challenges from supabase
        /// </summary>
        /// <returns></returns>
        Task<List<ActiveChallengeSbModel>> GetActiveChallenges();

        /// <summary>
        /// Used to update challenge progress
        /// </summary>
        /// <param name="activeChallengeId"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        Task UpdateChallengeData(int activeChallengeId, int progress);
    }
}
