using BodyBuddy.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BodyBuddy.Services;
using BodyBuddy.Views.ExerciseViews;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.ViewModels.ExerciseViewModels
{
    public partial class CategoryViewModel : BaseViewModel
    {
        public ObservableCollection<ExerciseModel> Categories { get; set; } = new();

        [ObservableProperty]
        private List<string> _categoriesTest;

        private readonly IExerciseService _exerciseService;

        public CategoryViewModel(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
            Title = string.Empty;
        }

        [RelayCommand]
        async Task GoToPrimaryMusclesPage(string category)
        {
            if (category is null)
                return;

            await Task.Delay(100); // Add a short delay
            await Shell.Current.GoToAsync($"{nameof(MuscleGroupPage)}?Category={Uri.EscapeDataString(category)}");
        }

        public async Task Initialize()
        {
            CategoriesTest = await _exerciseService.GetUniqueCategoriesAsync();
        }
    }
}
