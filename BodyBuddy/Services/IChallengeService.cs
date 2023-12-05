using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Services
{
    public interface IChallengeService
    {
        Task<List<ChallengeDto>> GetActiveChallenges();
    }
}
