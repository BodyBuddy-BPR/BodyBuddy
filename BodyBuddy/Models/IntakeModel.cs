using SQLite;

namespace BodyBuddy.Models
{
    [Table("Intake")]
	public class IntakeModel
	{
		[PrimaryKey]
		[Column("id")]
		public int Id { get; set; }

		[Column("calorieGoal")]
		public int CalorieGoal { get; set; }

		[Column("waterGoal")]
		public int WaterGoal { get; set; }

		[Column("calorieCurrent")]
		public int CalorieCurrent { get; set; }

		[Column("waterCurrent")]
		public int WaterCurrent { get; set; }

		[Column("date")]
		public int Date { get; set; }

		public IntakeModel()
		{
			Date = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}
	}
}
