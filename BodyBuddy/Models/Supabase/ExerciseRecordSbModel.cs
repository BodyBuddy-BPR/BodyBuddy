using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("WorkoutRecord")]
    public class ExerciseRecordSbModel : BaseModel
    {
        [Column("userId")]
        public int UserId { get; set; }

        [Column("exerciseId")]
        public int ExerciseId { get; set; }

        [Column("set")]
        public int Set { get; set; }

        [Column("weight")]
        public int Weight { get; set; }

        [Column("reps")]
        public int Reps { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }
}
