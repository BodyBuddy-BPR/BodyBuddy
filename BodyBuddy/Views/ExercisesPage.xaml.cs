namespace BodyBuddy.Views;

public partial class ExercisesPage : ContentPage
{
    private ExercisesViewModel _viewModel;
    public ExercisesPage(ExercisesViewModel exercisesViewModel)
    {
        InitializeComponent();
        _viewModel = exercisesViewModel;
        BindingContext = exercisesViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void FilterChips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        //e.AddedItem is the musclegroup chip selected
        await _viewModel.GetExercises(e.AddedItem.ToString());
    }
}