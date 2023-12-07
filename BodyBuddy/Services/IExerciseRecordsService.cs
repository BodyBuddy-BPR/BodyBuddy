using BodyBuddy.Dtos;

namespace BodyBuddy.Services
{
    public interface IExerciseRecordsService
    {
        /// <summary>
        /// Saves the ExerciseRecord into local and remote Db
        /// </summary>
        /// <param name="exerciseRecordsDto"></param>
        /// <returns></returns>
        Task SaveExerciseRecords(ExerciseRecordsDto exerciseRecordsDto);

        /// <summary>
        /// Getting local ExerciseRecords
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <returns>All ExerciseRecords for given Exercise</returns>
        Task<List<ExerciseRecordsDto>> GetAllExerciseRecordsForExercise(int exerciseId);

        /// <summary>
        /// Remove local data and replacing with remote db data if internet and logged in
        /// </summary>
        /// <returns></returns>
        Task ReplaceSQLiteDataWithRemoteData();

    }
}
