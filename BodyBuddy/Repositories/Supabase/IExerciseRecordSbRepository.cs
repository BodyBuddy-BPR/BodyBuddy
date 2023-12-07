using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Models.Supabase;

namespace BodyBuddy.Repositories.Supabase
{
    public interface IExerciseRecordSbRepository
    {
        Task AddExerciseRecord(ExerciseRecordSbModel exerciseRecordSbModel);

        Task<List<ExerciseRecordSbModel>> GetAllExerciseRecordsForUser();
    }
}
