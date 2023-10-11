using SQLite;
using System;

namespace BodyBuddy.Models
{
	[Table("Intake")]
	public class Intake
	{
		[PrimaryKey]
		[Column("id")]
		public int Id { get; set; }

		[Column("caloriegoal")]
		public int CalorieGoal { get; set; }

		[Column("watergoal")]
		public int WaterGoal { get; set; }

		[Column("caloriecurrent")]
		public int CalorieCurrent { get; set; }

		[Column("watercurrent")]
		public int WaterCurrent { get; set; }

		[Column("date")]
		public int Date { get; set; }

		public Intake()
		{
			Date = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}
	}
}
