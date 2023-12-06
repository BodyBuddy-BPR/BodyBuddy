using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IChallengeService
    {
        Task<List<ChallengeDto>> GetActiveChallenges();
    }
}
