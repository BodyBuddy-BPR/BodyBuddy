﻿namespace BodyBuddy.Dtos
{
    public class ExerciseRecordsDto
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int Set { get; set; }
        public int Weight { get; set; }
        public int Reps { get; set; }
        public DateTime Date {get; set; }
    }
}
