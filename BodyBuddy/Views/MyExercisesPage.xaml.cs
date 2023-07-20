
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

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
        await _viewModel.GetMyExercises();
        await _viewModel.GetWorkoutplans();
    }

    private void AddToWorkoutPopup_Clicked(object sender, EventArgs e)
    {
        var exercise = (sender as ImageButton)?.CommandParameter as Exercise;
        if (exercise != null)
        {
            addToWorkoutPopup.Show();
            _viewModel.SelectedExercise = exercise;
        }
    }

    private  void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var current = (e.CurrentSelection)[0];
        _viewModel.SelectedItemsChanged(current);
    }
}