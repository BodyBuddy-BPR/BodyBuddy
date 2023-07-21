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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //await _viewModel.GetExercises();
    }

    private void SfChipGroup_ChipClicked(object sender, EventArgs e)
    {
        //_viewModel.SelectedChip = FilterChips;
    }

    private async void FilterChips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        //_viewModel.SelectedChip = e.AddedItem.ToString();
        await _viewModel.GetExercises(e.AddedItem.ToString());
    }
}