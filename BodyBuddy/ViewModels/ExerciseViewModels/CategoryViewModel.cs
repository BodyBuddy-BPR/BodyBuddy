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
        [ObservableProperty]
        private List<string> _categories;

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

            await Shell.Current.GoToAsync($"{nameof(MuscleGroupPage)}?Category={Uri.EscapeDataString(category)}",true);
        }

        public async Task Initialize()
        {
            Categories = await _exerciseService.GetUniqueCategoriesAsync();
        }
    }
}
