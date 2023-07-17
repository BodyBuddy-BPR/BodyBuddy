using SQLite;
using SQLiteNetExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BodyBuddy.Models
{
    public class Exercise
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        //[DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string MuscleGroup { get; set; }


        [ForeignKey(typeof(WorkoutPlan))]
        public int WorkoutPlanId { get; set; }
    }
}
