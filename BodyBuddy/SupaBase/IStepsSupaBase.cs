using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.SupaBaseModels;

namespace BodyBuddy.SupaBase
{
    public interface IStepsSupaBase
    {
        Task<List<StepsSupaBaseModel>> GetStepsForPeriodFriends();
        void AddOrUpdateSteps(StepDto stepDto);
    }
}
