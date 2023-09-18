using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private readonly IExerciseRepository _exerciseRepository;

        public ObservableCollection<Grouping<string, string>> MuscleGroups { get; set; } /*= new ObservableCollection<Grouping<string, Exercise>>();*/
        public ObservableCollection<string> MuscleGroupsTesting { get; set; } = new();

        public MuscleGroupViewModel(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
            //GenerateMuscleGroups();

            //LoadMuscleGroupsAsync();
        }


        //public async Task GetMuscleGroups()
        //{
        //    if (IsBusy) return;

        //    try
        //    {
        //        IsBusy = true;

        //        //var exercises = await _exerciseRepository.GetExercisesAsync(Exercise.Category, Exercise.PrimaryMuscles);
        //        var muscleGroups = await _exerciseRepository.GetMuscleGroupsForCategory(ExerciseCategory.Category);

        //        if (MuscleGroups.Count != 0)
        //        {
        //            MuscleGroups.Clear();
        //        }

        //        var groupedMusclegroups = from exercise in muscleGroups
        //                                  orderby exercise.TargetArea
        //                                  group exercise by exercise.TargetArea.ToString() into targetGroup
        //                                  select new Grouping<string, Exercise>(targetGroup.Key, targetGroup);

        //        MuscleGroups = new ObservableCollection<Grouping<string, Exercise>>(groupedMusclegroups);


        //        //foreach (var exercise in exercises)
        //        //{
        //        //    ExercisesList.Add(exercise);
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //        await Shell.Current.DisplayAlert("Error!", $"Unable to get musclegroups for the category {ex.Message}", "OK");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}



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
        //private void GenerateMuscleGroups()
        //{
        //    var muscleGroups = new List<Exercise>
        //    {
        //        new Exercise { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
        //        new Exercise { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
        //        new Exercise { PrimaryMuscles = "Traps", TargetArea = "Upper Body" },
        //        new Exercise { PrimaryMuscles = "Neck", TargetArea = "Upper Body" },

        //        new Exercise { PrimaryMuscles = "Biceps", TargetArea = "Arms" },
        //        new Exercise { PrimaryMuscles = "Forearms", TargetArea = "Arms" },
        //        new Exercise { PrimaryMuscles = "Triceps", TargetArea = "Arms" },

        //        new Exercise { PrimaryMuscles = "Middle back", TargetArea = "Back" },
        //        new Exercise { PrimaryMuscles = "Lower back", TargetArea = "Back" },
        //        new Exercise { PrimaryMuscles = "Lats", TargetArea = "Back" },

        //        new Exercise { PrimaryMuscles = "Abdominals", TargetArea = "Abs and Core" },

        //        new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
        //        new Exercise { PrimaryMuscles = "Adductors", TargetArea = "Lower Body" },
        //        new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
        //        new Exercise { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
        //        new Exercise { PrimaryMuscles = "Calves", TargetArea = "Lower Body" },
        //        new Exercise { PrimaryMuscles = "Abductors", TargetArea = "Lower Body" }
        //    };

        //    var groupedMusclegroups = from exercise in muscleGroups
        //                              orderby exercise.TargetArea
        //                              group exercise by exercise.TargetArea.ToString() into targetGroup
        //                              select new Grouping<string, Exercise>(targetGroup.Key, targetGroup);

        //    MuscleGroups = new ObservableCollection<Grouping<string, Exercise>>(groupedMusclegroups);
        //}

        private void GenerateMuscleGroupsForCategory(string category)
        {
            var strength = new List<Exercise>
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

            var stretching = new List<Exercise>
            {
                new Exercise { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
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
            
            var plyometrics = new List<Exercise>
            {
                new Exercise { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Triceps", TargetArea = "Arms" },
                new Exercise { PrimaryMuscles = "Lats", TargetArea = "Back" },
                new Exercise { PrimaryMuscles = "Abdominals", TargetArea = "Abs and Core" },
                new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Adductors", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
            };
            
            var strongman = new List<Exercise>
            {
                new Exercise { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Forearms", TargetArea = "Arms" },
                new Exercise { PrimaryMuscles = "Lower back", TargetArea = "Back" },
                new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
            };
            
            var powerlifting = new List<Exercise>
            {
                new Exercise { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Triceps", TargetArea = "Arms" },
                new Exercise { PrimaryMuscles = "Lower back", TargetArea = "Back" },
                new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
            };

            var cardio = new List<Exercise>
            {
                new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
            };
            
            var olympicWeightlifting = new List<Exercise>
            {
                new Exercise { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Traps", TargetArea = "Upper Body" },
                new Exercise { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                new Exercise { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
            };
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
