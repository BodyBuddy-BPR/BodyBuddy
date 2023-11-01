//using Postgrest.Attributes;
//using Postgrest.Models;
using SQLite;

namespace BodyBuddy.Models
{
    [Table("Exercise")]
    public class ExerciseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("force")]
        public string Force { get; set; }

        [Column("level")]
        public string Level { get; set; }

        [Column("mechanic")]
        public string Mechanic { get; set; }

        [Column("equipment")]
        public string Equipment { get; set; }

        [Column("primaryMuscles")]
        public string PrimaryMuscles { get; set; }

        [Column("secondaryMuscles")]
        public string SecondaryMuscles { get; set; }

        [Column("instructions")]
        public string Instructions { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("images")]
        public string Images { get; set; }

        [Column("name_id")]
        public string NameId { get; set; }


        // Used in WorkoutDetailsViewModel
        [Ignore]
        public int WorkoutExerciseId { get; set; }

        [Ignore]
        public int Sets { get; set; }
        [Ignore]
        public int Reps { get; set; }

        public List<ExerciseRecordsModel> Records { get; set; } = new();
    }
}
