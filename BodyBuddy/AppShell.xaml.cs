using BodyBuddy.Views.Authentication;
using BodyBuddy.Views.ExerciseViews;
using BodyBuddy.Views.WorkoutViews;

namespace BodyBuddy;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		//Exercise Pages
		Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
		Routing.RegisterRoute(nameof(MuscleGroupPage), typeof(MuscleGroupPage));
		Routing.RegisterRoute(nameof(ExercisesPage), typeof(ExercisesPage));
		Routing.RegisterRoute(nameof(ExerciseDetailsPage), typeof(ExerciseDetailsPage));

		//Workout Pages
		Routing.RegisterRoute(nameof(WorkoutDetailsPage), typeof(WorkoutDetailsPage));
		Routing.RegisterRoute(nameof(StartedWorkoutPage), typeof(StartedWorkoutPage));

		// Authentication
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
	}
}
