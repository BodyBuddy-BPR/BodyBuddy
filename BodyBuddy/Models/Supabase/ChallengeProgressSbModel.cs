using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("ChallengeUserProgress")]
    public class ChallengeProgressSbModel : BaseModel
    {
        [Column("userId")]
        public string UserId { get; set; }
        [Column("activeChallengeId")]
        public int ActiveChallengeId { get; set; }
        [Column("progress")]
        public int Progress { get; set; }

        [Reference(typeof(ActiveChallengeSbModel))]
        public ActiveChallengeSbModel ActiveChallenge { get; set; }
    }
}
