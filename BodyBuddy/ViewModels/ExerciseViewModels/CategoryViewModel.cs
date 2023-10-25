using BodyBuddy.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BodyBuddy.Views.ExerciseViews;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    public partial class CategoryViewModel : BaseViewModel
    {
        public ObservableCollection<ExerciseModel> Categories { get; set; } = new();

        public CategoryViewModel()
        {
            Title = string.Empty;
            GenerateCategories();
        }

        [RelayCommand]
        async Task GoToPrimaryMusclesPage(ExerciseModel category)
        {
            if (category is null)
                return;

            await Task.Delay(100); // Add a short delay
            await Shell.Current.GoToAsync(nameof(MuscleGroupPage), true, new Dictionary<string, object>
            {
                { "Category", category }
            });
        }

        private void GenerateCategories()
        {
            Categories.Add(new ExerciseModel { Category = "Strength" });
            Categories.Add(new ExerciseModel { Category = "Cardio" });
            Categories.Add(new ExerciseModel { Category = "Stretching" });
            Categories.Add(new ExerciseModel { Category = "Plyometrics" });
            Categories.Add(new ExerciseModel { Category = "Strongman" });
            Categories.Add(new ExerciseModel { Category = "Powerlifting" });
            Categories.Add(new ExerciseModel { Category = "Olympic weightlifting" });
        }
    }
}
