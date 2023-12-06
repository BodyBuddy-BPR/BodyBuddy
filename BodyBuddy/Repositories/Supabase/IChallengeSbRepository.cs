﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IChallengeSbRepository
    {
        Task<List<ActiveChallengeSbModel>> GetActiveChallenges();
        Task UpdateChallengeData(int activeChallengeId, int progress);

    }
}