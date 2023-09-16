using BodyBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Repositories
{
    public interface IExerciseRepository
    {
        //Supabase methods
        //Task<List<Exercise>> GetExercisesAsync(string category, string musclegroup);

        //Task<Exercise> GetExerciseDetailsAsync(int id);

        //// Not in use anymore
        //Task<List<string>> GetPrimaryMusclesAsync();


        Task<List<Exercise>> GetExercisesAsync();
    }
}
