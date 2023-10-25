using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(ExerciseCategory), "Category")]
    public partial class MuscleGroupViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ExerciseModel exerciseCategory;

        private readonly IExerciseRepository _exerciseRepository;

        public ObservableCollection<Grouping<string, ExerciseModel>> MuscleGroups { get; set; } = new ObservableCollection<Grouping<string, ExerciseModel>>();

        public MuscleGroupViewModel(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task GetMusclegroups()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (MuscleGroups.Count != 0)
                {
                    MuscleGroups.Clear();
                }

                GenerateMuscleGroupsForCategory(ExerciseCategory.Category);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get musclegroups {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToExercisesPage(ExerciseModel exercise)
        {
            if (exercise is null)
                return;

            exercise.Category = ExerciseCategory.Category;

            await Task.Delay(100); // Add a short delay
            await Shell.Current.GoToAsync(nameof(ExercisesPage), true, new Dictionary<string, object>
            {
                { "Exercise", exercise }
            });
        }


        private void GenerateMuscleGroupsForCategory(string category)
        {
            var muscleGroups = new List<ExerciseModel>();

            if (category == "Strength")
            {
                var strength = new List<ExerciseModel>
                {
                    new ExerciseModel { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Traps", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Neck", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Biceps", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Forearms", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Triceps", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Middle back", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Lower back", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Lats", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Abdominals", TargetArea = "Abs and Core" },
                    new ExerciseModel { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Adductors", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Calves", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Abductors", TargetArea = "Lower Body" }
                };
                muscleGroups.AddRange(strength);
            }
            else if (category == "Stretching")
            {
                var stretching = new List<ExerciseModel>
                {
                    new ExerciseModel { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Neck", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Biceps", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Forearms", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Triceps", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Middle back", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Lower back", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Lats", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Abdominals", TargetArea = "Abs and Core" },
                    new ExerciseModel { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Adductors", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Calves", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Abductors", TargetArea = "Lower Body" }
                };
                muscleGroups.AddRange(stretching);
            }
            else if (category == "Plyometrics")
            {
                var plyometrics = new List<ExerciseModel>
                {
                    new ExerciseModel { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Triceps", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Lats", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Abdominals", TargetArea = "Abs and Core" },
                    new ExerciseModel { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Adductors", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                };
                muscleGroups.AddRange(plyometrics);
            }
            else if (category == "Strongman")
            {
                var strongman = new List<ExerciseModel>
                {
                    new ExerciseModel { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Forearms", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Lower back", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                };
                muscleGroups.AddRange(strongman);
            }
            else if (category == "Powerlifting")
            {
                var powerlifting = new List<ExerciseModel>
                {
                    new ExerciseModel { PrimaryMuscles = "Chest", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Triceps", TargetArea = "Arms" },
                    new ExerciseModel { PrimaryMuscles = "Lower back", TargetArea = "Back" },
                    new ExerciseModel { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
                };
                muscleGroups.AddRange(powerlifting);
            }
            else if (category == "Cardio")
            {
                var cardio = new List<ExerciseModel>
                {
                    new ExerciseModel { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                };
                muscleGroups.AddRange(cardio);
            }
            else if (category == "Olympic weightlifting")
            {
                var olympicWeightlifting = new List<ExerciseModel>
                {
                    new ExerciseModel { PrimaryMuscles = "Shoulders", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Traps", TargetArea = "Upper Body" },
                    new ExerciseModel { PrimaryMuscles = "Hamstrings", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Quadriceps", TargetArea = "Lower Body" },
                    new ExerciseModel { PrimaryMuscles = "Glutes", TargetArea = "Lower Body" },
                };
                muscleGroups.AddRange(olympicWeightlifting);
            }

            var groupedMusclegroups = from exercise in muscleGroups
                                      orderby exercise.TargetArea
                                      group exercise by exercise.TargetArea.ToString() into targetGroup
                                      select new Grouping<string, ExerciseModel>(targetGroup.Key, targetGroup);

            foreach(var group in groupedMusclegroups)
            {
                MuscleGroups.Add(group);
            }

            //MuscleGroups = new ObservableCollection<Grouping<string, Exercise>>(groupedMusclegroups);
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
