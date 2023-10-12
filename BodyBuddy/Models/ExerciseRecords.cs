using SQLite;

namespace BodyBuddy.Models
{
    [Table("ExerciseRecords")]
    public class ExerciseRecords
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; }

        [Column("exercise_id")]
        public int ExerciseId { get; set; }

        [Column("set")]
        public int Set { get; set; }

        [Column("weight")]
        public int Weight { get; set; }

        [Column("reps")]
        public int Reps { get; set; }
    }
}
