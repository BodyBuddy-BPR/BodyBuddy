using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class ExercisesPage : ContentPage
{
    private ExercisesViewModel _viewModel;
    public ExercisesPage(ExercisesViewModel exercisesViewModel)
    {
        InitializeComponent();
        _viewModel = exercisesViewModel;
        BindingContext = exercisesViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialize();
    }
}