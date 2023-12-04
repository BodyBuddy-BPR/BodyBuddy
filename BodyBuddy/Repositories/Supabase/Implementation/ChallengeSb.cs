using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;
using BodyBuddy.Models.Supabase;
using Supabase;

namespace BodyBuddy.Repositories.Supabase.Implementation
{
    public class ChallengeSb : IChallengeSb
    {
        private readonly Client _supabase;

        public ChallengeSb(Client client)
        {
            _supabase = client;
        }

        public async Task<List<ActiveChallengeSbModel>> GetActiveChallenges()
        {
            var activeChallengesModel = await _supabase.From<ActiveChallengeSbModel>()
                .Where(x => x.From <= DateTime.Now && x.To >= DateTime.Now).Get();
            
            return activeChallengesModel.Models;
        }

        public async Task UpdateChallengeData(ChallengeDto challengeDto)
        {
            var record = new ChallengeProgressSbModel() { 
                UserId = SecureStorage.GetAsync("UserUID").Result,
                ActiveChallengeId = challengeDto.ActiveChallengeId, 
                Progress = challengeDto.Progress };

            await _supabase.From<ChallengeProgressSbModel>().Upsert(record);
        }
    }
}
