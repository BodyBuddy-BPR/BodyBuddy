using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("ActiveChallenge")]
    public class ChallengeSbModel : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("goal")]
        public int Goal { get; set; }

        [Column("difficulty")]
        public string Difficulty { get; set; }
    }
}
