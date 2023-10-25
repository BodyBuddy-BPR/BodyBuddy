using SQLite;

namespace BodyBuddy.Models
{
	[Table("Workout")]
	public class WorkoutModel
	{
		[PrimaryKey]
		public int Id { get; set; }

		[Column("name")]
		public string Name { get; set; }
		
		[Column("description")]
		public string Description { get; set; }

        // Needs to be int since SQLite does not support bools.
		// Instead, Boolean values are stored as integers 0 (false) and 1 (true).
        [Column("preMade")]
		public int PreMade { get; set; }
    }
}
