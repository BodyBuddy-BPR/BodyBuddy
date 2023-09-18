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
        builder.Services.AddTransient<MuscleGroupViewModel>();
        builder.Services.AddSingleton<ExercisesViewModel>();
		builder.Services.AddTransient<ExerciseDetailsViewModel>();
		builder.Services.AddTransient<WorkoutPlanDetailsViewModel>();

        // Repositories
        builder.Services.AddSingleton<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddSingleton<IWorkoutPlanRepository, WorkoutPlanRepository>();

        // Local Database
        builder.Services.AddSingleton<LocalDatabase>();
        builder.Services.AddTransient(async provider =>
        {
            var localDatabase = provider.GetRequiredService<LocalDatabase>();
            return await localDatabase.GetAsyncConnection();
        });
        builder.Services.AddSingleton(provider =>
        {
            var localDatabase = provider.GetRequiredService<LocalDatabase>();
            return localDatabase.GetAsyncConnection().Result; // Use .Result to block and get the connection synchronously.
        });

        // Supabase Database
        builder.Services.AddSingleton(provider => new Supabase.Client(url, key, options));

        // Services
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

        return builder.Build();
	}
}
