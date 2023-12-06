using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("Workout")]

    public class WorkoutSbModel : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("userId")]
        public string UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("preMade")]
        public bool PreMade { get; set; }
    }
}
