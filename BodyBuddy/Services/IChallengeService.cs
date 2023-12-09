using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IChallengeService
    {
        /// <summary>
        /// Used to get all challenges from remote db
        /// </summary>
        /// <returns></returns>
        Task<List<ChallengeDto>> GetActiveChallenges();
    }
}
