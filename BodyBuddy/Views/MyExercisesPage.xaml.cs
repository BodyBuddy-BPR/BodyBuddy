
namespace BodyBuddy.Views;

public partial class MyExercisesPage : ContentPage
{
    private MyExercisesViewModel _viewModel;
    public MyExercisesPage(MyExercisesViewModel myExercisesViewModel)
    {
        InitializeComponent();

        _viewModel = myExercisesViewModel;
        BindingContext = myExercisesViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //await _viewModel.GetMyExercises();
    }
}