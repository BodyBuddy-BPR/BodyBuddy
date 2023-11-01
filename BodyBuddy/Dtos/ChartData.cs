using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Dtos
{
    public class ChartData
    {
        public DateTime Date { get; set; }
        public double MaxWeight { get; set; }
        public double MinWeight { get; set; }
        public double AverageWeight { get; set; }
    }
}
