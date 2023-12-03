using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.SupaBaseModels
{
    [Table("Steps")]
    public class StepsSupaBaseModel : BaseModel
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("steps")]
        public int Steps { get; set; }
    }
}
