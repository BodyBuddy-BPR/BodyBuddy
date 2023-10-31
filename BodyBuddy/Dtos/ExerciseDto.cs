using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Dtos
{
    public class ExerciseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Force { get; set; }

        public string Level { get; set; }

        public string Mechanic { get; set; }

        public string Equipment { get; set; }

        public string PrimaryMuscles { get; set; }

        public string SecondaryMuscles { get; set; }

        public string Instructions { get; set; }

        public string Category { get; set; }

        public string Images { get; set; }

        //Based on PrimaryMuscles
        public string TargetArea { get; set; }

        public int WorkoutExerciseId { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public List<ExerciseRecordsDto> Records { get; set; } = new();
    }

}
