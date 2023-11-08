using BodyBuddy.Views.Authentication;
using BodyBuddy.Views.ExerciseViews;
using BodyBuddy.Views.Profile;
using BodyBuddy.Views.StartupTest;
using BodyBuddy.Views.User;
using BodyBuddy.Views.WorkoutViews;

namespace BodyBuddy;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		//Profile
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		Routing.RegisterRoute(nameof(FriendsPage), typeof(FriendsPage));

		//Exercise Pages
		Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
		Routing.RegisterRoute(nameof(MuscleGroupPage), typeof(MuscleGroupPage));
		Routing.RegisterRoute(nameof(ExercisesPage), typeof(ExercisesPage));
		Routing.RegisterRoute(nameof(ExerciseDetailsPage), typeof(ExerciseDetailsPage));

		//Workout Pages
		Routing.RegisterRoute(nameof(WorkoutDetailsPage), typeof(WorkoutDetailsPage));
		Routing.RegisterRoute(nameof(StartedWorkoutPage), typeof(StartedWorkoutPage));

        //Startup Test
        Routing.RegisterRoute(nameof(StartupTestPage), typeof(StartupTestPage));

        // Authentication
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
    }
}
