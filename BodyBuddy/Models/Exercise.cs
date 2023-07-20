using Postgrest.Attributes;
using Postgrest.Models;


namespace BodyBuddy.Models
{
    [Table("Exercise")]
    public class Exercise : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("muscle_group")]
        public string MuscleGroup { get; set; }
    }
}
