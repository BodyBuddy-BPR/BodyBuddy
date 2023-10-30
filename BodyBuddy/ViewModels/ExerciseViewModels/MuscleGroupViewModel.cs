using BodyBuddy.Models;
using BodyBuddy.Repositories;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BodyBuddy.Services;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    [QueryProperty(nameof(Category), "Category")]
    public partial class MuscleGroupViewModel : BaseViewModel
    {
        [ObservableProperty] private string _category;
        public ObservableCollection<Grouping<string, ExerciseModel>> MuscleGroups { get; set; } = new();

        private readonly IExerciseService _exerciseService;

        public MuscleGroupViewModel(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task GetMuscleGroups()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (MuscleGroups.Count != 0)
                {
                    MuscleGroups.Clear();
                }

                await GenerateMuscleGroupsForCategory(Category);

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
        public async Task GoToExercisesPage(ExerciseModel exercise)
        {
            if (exercise is null)
                return;

            await Task.Delay(150); // Add a short delay
            await Shell.Current.GoToAsync(nameof(ExercisesPage), true, new Dictionary<string, object>
            {
                { "Exercise", exercise }
            });
        }


        private async Task GenerateMuscleGroupsForCategory(string category)
        {
            var muscleGroups = await _exerciseService.GetMuscleGroupsForCategory(category);
            
            var groupedMuscleGroups = from exercise in muscleGroups
                                      orderby exercise.TargetArea
                                      group exercise by exercise.TargetArea into targetGroup
                                      select new Grouping<string, ExerciseModel>(targetGroup.Key, targetGroup);

            foreach(var group in groupedMuscleGroups)
            {
                MuscleGroups.Add(group);
            }
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
