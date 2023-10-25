using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Dtos
{
    public class StartupTestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public double Weight { get; set; }

        public int Height { get; set; }

        public DateTime Birthday { get; set; }

        public string ActiveAmount { get; set; }

        public int PassiveCalorieBurn { get; set; }

        public string Goal { get; set; }

        public List<string> FocusAreas { get; set; } = new();
    }
}
