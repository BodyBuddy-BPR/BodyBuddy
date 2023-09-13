using BodyBuddy.ViewModels;

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
        await _viewModel.GetExercises();
    }

    //private async void FilterChips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    //{
    //    //e.AddedItem is the musclegroup chip selected
    //    await _viewModel.GetExercises(e.AddedItem.ToString());
    //}
}