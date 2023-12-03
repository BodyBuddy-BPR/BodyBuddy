using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;

namespace BodyBuddy.SupaBase
{
    public interface IStepsSupaBase
    {
        StepModel GetStepsForPeriodFriends();
        void AddOrUpdateSteps(StepDto stepDto);
    }
}
