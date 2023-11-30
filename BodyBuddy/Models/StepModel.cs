using SQLite;

namespace BodyBuddy.Models
{
    [Table("Steps")]
    public class StepModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("steps")]
        public int Steps { get; set; }
        [Column("stepGoal")]
        public int StepGoal { get; set; }

        [Column("date")]
        public long Date { get; set; }
    }
}
