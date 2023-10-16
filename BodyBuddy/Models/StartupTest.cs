using SQLite;

namespace BodyBuddy.Models
{
    [Table("StartupTest")]
    public class StartupTest
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("gender")]
        public string Gender { get; set; }

        [Column("weight")]
        public int Weight { get; set; }

        [Column("height")]
        public string Height { get; set; }

        [Column("birthday")]
        public string Birthday { get; set; }

        [Column("activeAmount")]
        public string ActiveAmount { get; set; }

        [Column("passiveCalorieBurn")]
        public int PassiveCalorieBurn { get; set; }

        [Column("goal")]
        public string Goal { get; set; }
    }
}