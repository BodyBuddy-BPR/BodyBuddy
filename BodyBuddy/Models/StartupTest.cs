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
        public int Gender { get; set; }

        [Column("weight")]
        public double Weight { get; set; }

        [Column("height")]
        public int Height { get; set; }

        [Column("birthday")]
        public long Birthday { get; set; }

        [Column("activeAmount")]
        public int ActiveAmount { get; set; }

        [Column("passiveCalorieBurn")]
        public int PassiveCalorieBurn { get; set; }

        [Column("goal")]
        public int Goal { get; set; }
    }
}