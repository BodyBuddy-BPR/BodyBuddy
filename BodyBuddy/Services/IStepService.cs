using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IStepService
    {
        //Converts Dto --> Db object
        Task SaveStepData(StepDto stepDto);

        //Converts Db --> Dto object
        Task<StepDto> GetStepDataTodayAsync();


        // Total sum of steps for a period
        Task<List<UserTotalSteps>> GetStepsForPeriodFriends();
        Task<UserTotalSteps> GetStepDataForPeriodAsync(long startDate, long endDate);

        /// <summary>
        /// Delegate and Event to notify challenge about addition of steps
        /// </summary>
        delegate void StepsChanged(int steps);

        /// <summary>
        /// Delegate and Event to notify challenge about addition of steps
        /// </summary>
        event StepsChanged IsStepsChanged;


        Task ReplaceSQLiteDataWithRemoteData();
    }
}
