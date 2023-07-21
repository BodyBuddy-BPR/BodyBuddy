using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using BodyBuddy.Database;
using Syncfusion.Maui.Core.Hosting;
using Supabase;


namespace BodyBuddy;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Supabase
        var url = SupabaseConfig.SUPABASE_URL;
        var key = SupabaseConfig.SUPABASE_KEY;
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false,
            // SessionHandler = new SupabaseSessionHandler() <-- This must be implemented by the developer
        };

        // Views
        builder.Services.AddSingleton<WorkoutPlansPage>();
        builder.Services.AddSingleton<ExercisesPage>();
		builder.Services.AddSingleton<CustomExercisesPage>();
		builder.Services.AddTransient<NewExercisePage>();
		builder.Services.AddTransient<WorkoutPlanDetailsPage>();

		// ViewModels
		builder.Services.AddSingleton<WorkoutPlansViewModel>();
		builder.Services.AddSingleton<ExercisesViewModel>();
        builder.Services.AddSingleton<CustomExercisesViewModel>();
		builder.Services.AddTransient<NewExerciseViewModel>();
		builder.Services.AddTransient<WorkoutPlanDetailsViewModel>();

        // Repositories
        builder.Services.AddSingleton<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddSingleton<IWorkoutPlanRepository, WorkoutPlanRepository>();

        // Database
        builder.Services.AddSingleton<LocalDatabase>();
        builder.Services.AddSingleton(provider => new Supabase.Client(url, key, options));

        return builder.Build();
	}
}
