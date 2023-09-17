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

        public async Task LoadMuscleGroupsAsync()
        {
            // Load primary muscle groups from the database
            var primaryMuscleGroups = await _exerciseRepository.GetMuscleGroupsForCategory(ExerciseCategory.Category);

            //Create a mapping between muscle groups and target areas
            var muscleGroupToTargetAreaMapping = new Dictionary<string, string>
           {
                { "Shoulders", "Upper Body" },
                { "Chest", "Upper Body" },
                { "Traps", "Upper Body" },
                { "Neck", "Upper Body" },
                { "Biceps", "Arms" },
                { "Forearms", "Arms" },
                { "Triceps", "Arms" },
                { "Middle back", "Back" },
                { "Lower back", "Back" },
                { "Lats", "Back" },
                { "Abdominals", "Abs and Core" },
                { "Hamstrings", "Lower Body" },
                { "Adductors", "Lower Body" },
                { "Quadriceps", "Lower Body" },
                { "Glutes", "Lower Body" },
                { "Calves", "Lower Body" },
                { "Abductors", "Lower Body" }
           };

            // Group primary muscle groups by their target areas
            var groupedMuscleGroups = from exercise in primaryMuscleGroups
                                      group exercise by muscleGroupToTargetAreaMapping.ContainsKey(exercise.PrimaryMuscles) ?
                                      muscleGroupToTargetAreaMapping[exercise.PrimaryMuscles] : "Unknown" into muscleGroup
                                      select new Grouping<string, Exercise>(muscleGroup.Key, muscleGroup);

            MuscleGroups = new ObservableCollection<Grouping<string, string>>(groupedMuscleGroups);
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
