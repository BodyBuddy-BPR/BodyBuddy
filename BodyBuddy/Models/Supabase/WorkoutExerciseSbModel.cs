using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;

namespace BodyBuddy.Models.Supabase
{
    [Table("WorkoutExercise")]

    public class WorkoutExerciseSbModel : BaseModel
    {
        [Column("exerciseId")]
        public int ExerciseId { get; set; }

        [Column("workoutId")]
        public int WorkoutId { get; set; }

        [Column("sets")]
        public int Sets { get; set; }

        [Column("reps")]
        public int Reps { get; set; }
    }
}
