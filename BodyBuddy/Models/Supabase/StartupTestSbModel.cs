using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("StartupTest")]
    public class StartupTestSbModel : BaseModel
    {
        [Column("userId")]
        public string UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("gender")]
        public int Gender { get; set; }

        [Column("weight")]
        public double Weight { get; set; }

        [Column("height")]
        public int Height { get; set; }

        [Column("birthday")]
        public DateTime Birthday { get; set; }

        [Column("activeAmount")]
        public int ActiveAmount { get; set; }

        [Column("passiveCalorieBurn")]
        public int PassiveCalorieBurn { get; set; }

        [Column("targetAreas")]
        public string TargetAreas { get; set; }

        [Column("goal")]
        public int Goal { get; set; }
    }
}
