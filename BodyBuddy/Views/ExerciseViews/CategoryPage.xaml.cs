using BodyBuddy.ViewModels.ExerciseViewModels;

namespace BodyBuddy.Views.ExerciseViews;

public partial class CategoryPage : ContentPage
{
	private CategoryViewModel _viewmodel;

	public CategoryPage(CategoryViewModel categoryViewModel)
	{
		InitializeComponent();
		_viewmodel = categoryViewModel;
		BindingContext = categoryViewModel;
	}
}