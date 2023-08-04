namespace BodyBuddy;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(NewExercisePage), typeof(NewExercisePage));
		Routing.RegisterRoute(nameof(ExerciseDetailsPage), typeof(ExerciseDetailsPage));
		Routing.RegisterRoute(nameof(WorkoutPlanDetailsPage), typeof(WorkoutPlanDetailsPage));
	}
}
