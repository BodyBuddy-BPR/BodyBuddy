using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using BodyBuddy.Database;
using Syncfusion.Maui.Core.Hosting;
using Supabase;
using BodyBuddy.Repositories;
using BodyBuddy.Repositories.Implementations;
using BodyBuddy.ViewModels;
using BodyBuddy.Views;
using BodyBuddy.Views.ExerciseViews;
using BodyBuddy.ViewModels.ExerciseViewModels;

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
        builder.Services.AddTransient<WorkoutPlanDetailsPage>();

        builder.Services.AddSingleton<CategoryPage>();
        builder.Services.AddSingleton<MuscleGroupPage>();
        builder.Services.AddTransient<ExercisesPage>();
		builder.Services.AddTransient<ExerciseDetailsPage>();
		builder.Services.AddTransient<WorkoutPlanDetailsPage>();

		// ViewModels
		builder.Services.AddSingleton<WorkoutPlansViewModel>();
        builder.Services.AddTransient<WorkoutPlanDetailsViewModel>();

        builder.Services.AddSingleton<CategoryViewModel>();
        builder.Services.AddSingleton<MuscleGroupViewModel>();
        builder.Services.AddSingleton<ExercisesViewModel>();
		builder.Services.AddTransient<ExerciseDetailsViewModel>();
		builder.Services.AddTransient<WorkoutPlanDetailsViewModel>();

        // Repositories
        builder.Services.AddSingleton<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddSingleton<IWorkoutPlanRepository, WorkoutPlanRepository>();

        // Database
        builder.Services.AddSingleton<LocalDatabase>();
        builder.Services.AddSingleton(provider => new Supabase.Client(url, key, options));

        // Services
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

        return builder.Build();
	}
}
