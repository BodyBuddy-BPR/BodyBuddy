using SQLite;

namespace BodyBuddy.Models
{
	[Table("Workout")]
	public class Workout
	{
		[PrimaryKey]
		public int Id { get; set; }

		[Column("name")]
		public string Name { get; set; }
	}
}
