using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models
{
    [Table("WorkoutPlanExercises")]
    public class WorkoutPlanExercises : BaseModel
    {
        [PrimaryKey("workout_plan_id", false)]
        // Composite key
        public int WorkoutPlanId { get; set; }

        [PrimaryKey("exercise_id", false)]
        public int ExerciseId { get; set; }

        // Navigation properties
        public WorkoutPlan WorkoutPlan { get; set; }
        public Exercise Exercise { get; set; }
    }
}
