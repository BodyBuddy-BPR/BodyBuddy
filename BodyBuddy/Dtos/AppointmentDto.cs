namespace BodyBuddy.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string EventName { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Brush Background { get; set; }

        public WorkoutDto? Workout { get; set; }

        public bool IsWorkoutVisible => Workout != null && Workout.Id != 0;
    }
}
