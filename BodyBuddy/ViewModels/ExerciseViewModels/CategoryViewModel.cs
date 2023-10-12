using BodyBuddy.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BodyBuddy.Views.ExerciseViews;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    public partial class CategoryViewModel : BaseViewModel
    {
        public ObservableCollection<Exercise> Categories { get; set; } = new ObservableCollection<Exercise>();

        public CategoryViewModel()
        {
            Title = string.Empty;
            GenerateCategories();
        }

        [RelayCommand]
        async Task GoToPrimaryMusclesPage(Exercise category)
        {
            if (category is null)
                return;

            await Shell.Current.GoToAsync(nameof(MuscleGroupPage), true, new Dictionary<string, object>
            {
                { "Category", category }
            });
        }

        private void GenerateCategories()
        {
            Categories.Add(new Exercise { Category = "Strength" });
            Categories.Add(new Exercise { Category = "Cardio" });
            Categories.Add(new Exercise { Category = "Stretching" });
            Categories.Add(new Exercise { Category = "Plyometrics" });
            Categories.Add(new Exercise { Category = "Strongman" });
            Categories.Add(new Exercise { Category = "Powerlifting" });
            Categories.Add(new Exercise { Category = "Olympic weightlifting" });
        }
    }
}
