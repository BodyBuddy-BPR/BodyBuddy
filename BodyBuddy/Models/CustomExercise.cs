using Postgrest.Attributes;
using Postgrest.Models;


namespace BodyBuddy.Models
{
    [Table("CustomExercise")]
    public class CustomExercise : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("instructions")]
        public string Instructions { get; set; }

        [Column("muscle_group")]
        public string MuscleGroup { get; set; }

    }
}
