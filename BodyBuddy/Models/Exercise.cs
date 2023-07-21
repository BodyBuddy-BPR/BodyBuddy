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

        [Column("force")]
        public string Force { get; set; }

        [Column("level")]
        public string Level { get; set; }

        [Column("mechanic")]
        public string Mechanic { get; set; }

        [Column("equipment")]
        public string Equipment { get; set; }

        [Column("primaryMuscles")]
        public string PrimaryMuscles { get; set; }

        [Column("secondaryMuscles")]
        public string SecondaryMuscles { get; set; }

        [Column("instructions")]
        public string Instructions { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("images")]
        public string Images { get; set; }

        [Column("name_id")]
        public string NameId { get; set; }
    }
}
