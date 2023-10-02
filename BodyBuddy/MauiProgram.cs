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
using BodyBuddy.Views.WorkoutViews;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;

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
            .ConfigureMopups()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Montserrat-Regular-400.ttf", "Montserrat");
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

        #region Dependency Registration

        #region Views

        // Workout
        builder.Services.AddSingleton<WorkoutsPage>();
        builder.Services.AddTransient<WorkoutDetailsPage>();

        builder.Services.AddSingleton<PreMadeWorkoutsPage>();
        builder.Services.AddTransient<PreMadeWorkoutDetailsPage>();

        // Exercise
        builder.Services.AddTransient<CategoryPage>();
        builder.Services.AddTransient<MuscleGroupPage>();
        builder.Services.AddTransient<ExercisesPage>();
        builder.Services.AddTransient<ExerciseDetailsPage>();
        #endregion

        #region ViewModels

        // Workout
        builder.Services.AddSingleton<WorkoutViewModel>();
        builder.Services.AddTransient<WorkoutDetailsViewModel>();

        builder.Services.AddSingleton<PreMadeWorkoutsViewModel>();
        builder.Services.AddTransient<PreMadeWorkoutDetailsViewModel>();

        // Exercise
        builder.Services.AddTransient<CategoryViewModel>();
        builder.Services.AddTransient<MuscleGroupViewModel>();
        builder.Services.AddTransient<ExercisesViewModel>();
        builder.Services.AddTransient<ExerciseDetailsViewModel>();
        #endregion

        #region Repositories
        builder.Services.AddSingleton<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddSingleton<IWorkoutRepository, WorkoutRepository>();
        #endregion

        #region Database
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
        #endregion

        #region Services

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);

        #endregion

        #endregion

        return builder.Build();
    }
}
