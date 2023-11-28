using BodyBuddy.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services
{
    public interface IStepService
    {
        //Converts Dto --> Db object
        void SaveStepData(StepDto stepDto);

        //Converts Db --> Dto object
        Task<StepDto> GetStepData();
    }
}
