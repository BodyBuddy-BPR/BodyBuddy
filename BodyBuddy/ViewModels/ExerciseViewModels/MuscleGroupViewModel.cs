using BodyBuddy.Models;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(ExerciseCategory), "Category")]
    public partial class MuscleGroupViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Exercise exerciseCategory;

        public ObservableCollection<Grouping<string, Exercise>> MuscleGroups { get; set; } /*= new ObservableCollection<Grouping<string, Exercise>>();*/

        public MuscleGroupViewModel()
        {
            GenerateMuscleGroups();
        }

        [RelayCommand]
        async Task GoToExercisesPage(Exercise exercise)
        {
            if (exercise is null)
                return;

            exercise.Category = ExerciseCategory.Category;

            await Shell.Current.GoToAsync(nameof(ExercisesPage), true, new Dictionary<string, object>
            {
                { "Exercise", exercise }
            });
        }

        // Used to generate and order the exercises in their target area
        private void GenerateMuscleGroups()
        {
            var muscleGroups = new List<Exercise>
            {
                new Exercise { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Traps", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Neck", TargetArea = "Upper Body" },

                new Exercise { PrimaryMuscles = "Biceps", TargetArea = "Arms" },
                new Exercise { PrimaryMuscles = "Forearms", TargetArea = "Arms" },
                new Exercise { PrimaryMuscles = "Triceps", TargetArea = "Arms" },

                new Exercise { PrimaryMuscles = "Middle back", TargetArea = "Back" },
                new Exercise { PrimaryMuscles = "Lower back", TargetArea = "Back" },
                new Exercise { PrimaryMuscles = "Lats", TargetArea = "Back" },

                new Exercise { PrimaryMuscles = "Abdominals", TargetArea = "Abs and Core" },

                new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Adductors", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Calves", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Abductors", TargetArea = "Lower Body" }
            };

            var groupedMusclegroups = from exercise in muscleGroups
                                      orderby exercise.TargetArea
                                      group exercise by exercise.TargetArea.ToString() into targetGroup
                                      select new Grouping<string, Exercise>(targetGroup.Key, targetGroup);

            MuscleGroups = new ObservableCollection<Grouping<string, Exercise>>(groupedMusclegroups);

            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Chest", TargetArea = "Upper Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Traps", TargetArea = "Upper Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Neck", TargetArea = "Upper Body" });

            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Biceps", TargetArea = "Arms" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Forearms", TargetArea = "Arms" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Triceps", TargetArea = "Arms" });

            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Middle back", TargetArea = "Back" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Lower back", TargetArea = "Back" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Lats", TargetArea = "Back" });

            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Abdominals", TargetArea = "Abs and Core" });

            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Adductors", TargetArea = "Lower Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Calves", TargetArea = "Lower Body" });
            //MuscleGroups.Add(new Exercise { PrimaryMuscles = "Abductors", TargetArea = "Lower Body" });
        }
    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
