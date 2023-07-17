using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQLiteNetExtensions.Attributes;

namespace BodyBuddy.Models
{
    public class WorkoutPlan
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }


        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Exercise> Exercises { get; set; }
    }
}
