﻿
using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IStepRepository
    {
        Task<StepModel> GetStepsForDayAsTimestampAsync(long dayAsTimestamp);
        Task SaveChangesAsync(StepModel stepDetails);
        Task ClearSQLiteData();
        Task AddListOfStepData(List<StepModel> stepModels);
    }
}
