using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Dtos
{
    public class IntakeDto
    {
        public int Id { get; set; }

        public int CalorieGoal { get; set; }

        public int WaterGoal { get; set; }

        public int CalorieCurrent { get; set; }

        public int WaterCurrent { get; set; }

        public double CalorieProgress { get; set; }

        public double WaterProgress { get; set; }

        public DateTime Date { get; set; }
    }
}
