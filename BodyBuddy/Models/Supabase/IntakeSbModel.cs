using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models.Supabase
{
    [Table("Intake")]
    public class IntakeSbModel : BaseModel
    {
        [Column("userId")]
        public string UserId { get; set; }

        [Column("calorieGoal")]
        public int CalorieGoal { get; set; }

        [Column("waterGoal")]
        public int WaterGoal { get; set; }

        [Column("calorieCurrent")]
        public int CalorieCurrent { get; set; }

        [Column("waterCurrent")]
        public int WaterCurrent { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }
}
