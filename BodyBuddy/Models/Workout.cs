using Postgrest.Models;
using SQLite;

namespace BodyBuddy.Models
{
	[Table("Workout")]
	public class Workout : BaseModel
	{
		[PrimaryKey]
		public int Id { get; set; }

		[Column("name")]
		public string Name { get; set; }
	}
}
