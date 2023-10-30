using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class CategoryPage : ContentPage
{
	private CategoryViewModel _viewModel;

	public CategoryPage(CategoryViewModel categoryViewModel)
	{
		InitializeComponent();
        _viewModel = categoryViewModel;
		BindingContext = categoryViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(250); // Add a short delay

        await _viewModel.Initialize();
    }
}