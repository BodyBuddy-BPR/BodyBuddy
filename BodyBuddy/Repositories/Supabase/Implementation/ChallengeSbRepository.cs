using BodyBuddy.Models.Supabase;
using Client = Supabase.Client;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class ChallengeSbRepository : IChallengeSbRepository
    {
        private readonly Client _supabase;

        public ChallengeSbRepository(Client client)
        {
            _supabase = client;
        }

        public async Task<List<ActiveChallengeSbModel>> GetActiveChallenges()
        {
            var activeChallengesModel = await _supabase.From<ActiveChallengeSbModel>()
                .Where(x => x.Active == true).Get();

            return activeChallengesModel.Models;
        }

        public async Task UpdateChallengeData(int activeChallengeId, int progress)
        {
            ChallengeProgressSbModel activeChallengeSbModel = new()
            {
                ActiveChallengeId = activeChallengeId,
                Progress = progress,
                UserId = SecureStorage.GetAsync("UserUID").Result
            };

            await _supabase.From<ChallengeProgressSbModel>()
               .Upsert(activeChallengeSbModel);
        }
    }
}
