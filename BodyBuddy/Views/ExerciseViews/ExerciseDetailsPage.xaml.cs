using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class ExerciseDetailsPage : ContentPage
{
	private ExerciseDetailsViewModel _viewModel;
	public ExerciseDetailsPage(ExerciseDetailsViewModel exerciseDetailsViewModel)
	{
		InitializeComponent();
		_viewModel = exerciseDetailsViewModel;
		BindingContext = exerciseDetailsViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(200); // Add a short delay

        await _viewModel.GetExerciseDetails();
        ToolBarItems();
    }

    private void ToolBarItems()
    {
        var toolbarItems = new List<ToolbarItem>();

        toolbarItems.Add(new ToolbarItem
        {
            Command = _viewModel.GoToExerciseGraphsCommand,
            Text = "Graph",
            CommandParameter = _viewModel.ExerciseDetails,
        });

        ToolbarItems.Clear();
        foreach (ToolbarItem toolbarItem in toolbarItems)
        {
            ToolbarItems.Add(toolbarItem);
        }
    }

}