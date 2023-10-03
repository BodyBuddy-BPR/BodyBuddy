using SQLite;

namespace BodyBuddy.Models
{
	[Table("PreMadeWorkoutExercises")]
	public class PreMadeWorkoutExercises
	{
		[PrimaryKey] 
		public int Id { get; set; }

		[Column("exercise_id")]
		public int ExerciseId { get; set; }

		[Column("workout_id")]
		public int WorkoutId { get; set; }

        [Column("sets")]
        public int Sets { get; set; }

        [Column("reps")]
        public int Reps { get; set; }
    }
}
