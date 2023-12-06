using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("ActiveChallenges")]
    public class ActiveChallengeSbModel : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("challengeId")]
        public int ChallengeId { get; set; }
        [Column("from")]
        public DateTime From { get; set; }
        [Column("to")]
        public DateTime To { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [Reference(typeof(ChallengeSbModel))]
        public ChallengeSbModel Challenge { get; set; }
    }
}
