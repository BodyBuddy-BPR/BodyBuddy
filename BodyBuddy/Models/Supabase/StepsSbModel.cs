using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("Steps")]
    public class StepsSbModel : BaseModel
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("steps")]
        public int Steps { get; set; }

        [Reference(typeof(UserModel))]
        public UserModel User { get; set; }
    }
}
