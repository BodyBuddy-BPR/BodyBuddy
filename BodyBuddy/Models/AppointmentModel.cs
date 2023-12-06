using SQLite;

namespace BodyBuddy.Models
{
    [Table("Appointment")]
    public class AppointmentModel
    {
        [PrimaryKey]
        public int Id { get; set; }

        [Column("date")]
        public string Date { get; set; }

        [Column("eventName")]
        public string EventName { get; set; }

        [Column("from")]
        public string From { get; set; }

        [Column("to")]
        public string To { get; set; }

        [Column("background")]
        public string Background { get; set; }

        [Column("workoutId")]
        public int WorkoutId { get; set; }
    }
}
