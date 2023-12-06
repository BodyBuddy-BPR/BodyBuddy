using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Mappers
{
    public class ChallengeMapper
    {
        public ChallengeMapper() { }

        public ChallengeDto MapFromSbToDto(ActiveChallengeSbModel activeChallengeSb, List<UserTotalSteps> steps)
        {
            var challengeDto = new ChallengeDto()
            {
                ActiveChallengeId = activeChallengeSb.Id,
                Difficulty = activeChallengeSb.Challenge.Difficulty,
                From = activeChallengeSb.From,
                To = activeChallengeSb.To,
                Goal = activeChallengeSb.Challenge.Goal,
                Type = activeChallengeSb.Challenge.Type
            };

            challengeDto.UserTotalSteps = steps;
            challengeDto.Progress = steps.Sum(s => s.TotalSteps);

            return challengeDto;
        }
    }
}
