using SQLite;

namespace BodyBuddy.Models
{
	[Table("WorkoutExercises")]
	public class WorkoutExercises
	{
		[PrimaryKey] 
		public int Id { get; set; }

		[Column("exercise_id")]
		public int ExerciseId { get; set; }

		[Column("workout_id")]
		public int WorkoutId { get; set; }

		//public Workout workout { get; set; }

		//public Exercise exercise { get; set; }
	}
}
