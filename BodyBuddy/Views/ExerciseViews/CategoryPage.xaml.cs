using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class CategoryPage : ContentPage
{
	private readonly CategoryViewModel _viewModel;

	public CategoryPage(CategoryViewModel categoryViewModel)
	{
		InitializeComponent();
        _viewModel = categoryViewModel;
		BindingContext = categoryViewModel;

        _viewModel.Initialize();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(50); // Add a short delay
    }
}