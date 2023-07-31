using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories
{
    public interface IExerciseRepository
    {
        #region Exercises

        Task<List<Exercise>> GetExercisesAsync(string musclegroup);

        Task<List<string>> GetPrimaryMusclesAsync();

        #endregion

        #region Custom Exercises

        Task<List<CustomExercise>> GetCustomExercisesAsync();
        Task SaveCustomExerciseAsync(CustomExercise exercise);

        #endregion
    }
}
