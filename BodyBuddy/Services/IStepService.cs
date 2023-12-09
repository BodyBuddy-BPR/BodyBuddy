using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IStepService
    {
        //Converts Dto --> Db object
        Task SaveStepData(StepDto stepDto);

        //Converts Db --> Dto object
        Task<StepDto> GetStepDataTodayAsync();

        /// <summary>
        /// Getting users own steps within a time period, excluding current day
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<UserTotalSteps> GetStepDataForPeriodAsync(long startDate, long endDate);


        /// <summary>
        /// Using remote db
        /// Gets all friends Steps in a time period and sums the steps together
        /// </summary>
        /// <returns>List of UserTotalSteps</returns>
        Task<List<UserTotalSteps>> GetStepsForPeriodFriends();

        /// <summary>
        /// Delegate and Event to notify challenge about addition of steps and update active challenges
        /// </summary>
        delegate void StepsChanged(int steps);

        /// <summary>
        /// Delegate and Event to notify challenge about addition of steps and update active challenges
        /// </summary>
        event StepsChanged IsStepsChanged;


        /// <summary>
        /// Remove local data and replacing with remote db data if internet and logged in
        /// </summary>
        /// <returns></returns>
        Task ReplaceSQLiteDataWithRemoteData();
    }
}
