using BodyBuddy.Views;
using BodyBuddy.Views.ExerciseViews;

namespace BodyBuddy;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		//Exercise Pages
		Routing.RegisterRoute(nameof(MuscleGroupPage), typeof(MuscleGroupPage));
		Routing.RegisterRoute(nameof(ExercisesPage), typeof(ExercisesPage));
		Routing.RegisterRoute(nameof(ExerciseDetailsPage), typeof(ExerciseDetailsPage));

		//Workout Pages
		Routing.RegisterRoute(nameof(WorkoutPlanDetailsPage), typeof(WorkoutPlanDetailsPage));
	}
}
