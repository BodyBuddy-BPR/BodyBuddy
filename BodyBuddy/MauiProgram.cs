using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using BodyBuddy.Database;
using Syncfusion.Maui.Core.Hosting;
using Supabase;
using BodyBuddy.Repositories;
using BodyBuddy.Repositories.Implementations;
using BodyBuddy.Views.ExerciseViews;
using BodyBuddy.ViewModels.ExerciseViewModels;
using BodyBuddy.Views.WorkoutViews;
using BodyBuddy.Views.StatisticsViews;
using BodyBuddy.ViewModels.WorkoutViewModels;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using Camera.MAUI;
using Maui.FixesAndWorkarounds;
using BodyBuddy.ViewModels;
using BodyBuddy.Mappers;
using BodyBuddy.Services;
using BodyBuddy.Services.Implementations;
using BodyBuddy.ViewModels.Authentication;
using BodyBuddy.ViewModels.IntakeViewModels;
using BodyBuddy.Views.StartupTest;
using BodyBuddy.Views.Profile;
using BodyBuddy.ViewModels.StartupTest;
using BodyBuddy.ViewModels.Profile;
using BodyBuddy.Views.Authentication;

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
            .ConfigureMauiWorkarounds()
            .ConfigureMopups()
            .UseMauiCameraView()
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
#if GITHUB_BUILD
    string url = Environment.GetEnvironmentVariable("SUPABASE_URL");
    string key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
#else
        string url = AppSettingKeys.SUPABASE_URL;
        string key = AppSettingKeys.SUPABASE_KEY;
#endif
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false,
            // SessionHandler = new SupabaseSessionHandler() <-- This must be implemented by the developer
        };

        #region Dependency Registration

        #region Database

        // Local Database
        builder.Services.AddSingleton<LocalDatabase>();
        builder.Services.AddTransient(async provider =>
        {
            var localDatabase = provider.GetRequiredService<LocalDatabase>();
            await localDatabase.Initialization; // Wait for initialization
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


        #region Views

        // MainPage
        builder.Services.AddSingleton<MainPage>();

        // Startup Test
        builder.Services.AddSingleton<StartupTestPage>();
        builder.Services.AddSingleton<ProfilePage>();

        // Workout
        builder.Services.AddTransient<WorkoutsPage>();
        builder.Services.AddTransient<WorkoutDetailsPage>();
        builder.Services.AddSingleton<StartedWorkoutPage>();

        // Exercise
        builder.Services.AddSingleton<CategoryPage>();
        builder.Services.AddTransient<MuscleGroupPage>();
        builder.Services.AddTransient<ExercisesPage>();
        builder.Services.AddTransient<ExerciseDetailsPage>();

        // Statistics
        builder.Services.AddSingleton<IntakePage>();

        // Authentication
        builder.Services.AddSingleton<LoginPage>();

        #endregion


        #region ViewModels

        // MainPage
        builder.Services.AddSingleton<MainPageViewModel>();

        // Startup Test
        builder.Services.AddSingleton<StartupTestViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();

        // Workout
        builder.Services.AddTransient<WorkoutViewModel>();
        builder.Services.AddTransient<WorkoutDetailsViewModel>();
        builder.Services.AddSingleton<StartedWorkoutViewModel>();

        // Exercise
        builder.Services.AddSingleton<CategoryViewModel>();
        builder.Services.AddTransient<MuscleGroupViewModel>();
        builder.Services.AddTransient<ExercisesViewModel>();
        builder.Services.AddTransient<ExerciseDetailsViewModel>();

        // Statistics
        builder.Services.AddSingleton<IntakeViewModel>();

        // Authentication
        builder.Services.AddSingleton<LoginViewModel>();
        #endregion


        #region Repositories

        // Startup Test
        builder.Services.AddSingleton<IStartupTestRepository, StartupTestRepository>();

        // Workout
        builder.Services.AddSingleton<IWorkoutRepository, WorkoutRepository>();
        builder.Services.AddSingleton<IWorkoutExercisesRepository, WorkoutExercisesRepository>();

        // Exercise
        builder.Services.AddSingleton<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddSingleton<IExerciseRecordsRepository, ExerciseRecordsRepository>();

        // Statistics
        builder.Services.AddSingleton<IIntakeRepository, IntakeRepository>();

        // Quote
        builder.Services.AddSingleton<IQuoteRepository, QuoteRepository>();

        #endregion


        #region Helpers
        // FIXME: Are these needed?
        builder.Services.AddSingleton<StartupTestMapper>();
        builder.Services.AddSingleton<IntakeMapper>();
        builder.Services.AddSingleton<QuoteMapper>();
        #endregion

        #region Services

        // Startup Test
        builder.Services.AddSingleton<IStartupTestService, StartupTestService>();

        // Workout
        builder.Services.AddSingleton<IWorkoutService, WorkoutService>();
        builder.Services.AddSingleton<IWorkoutExercisesService, WorkoutExercisesService>();

        // Exercise
        builder.Services.AddSingleton<IExerciseService, ExerciseService>();

        // Statistics
        builder.Services.AddSingleton<IIntakeService, IntakeService>();

        // Authentication
        builder.Services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();

        // Quote
        builder.Services.AddSingleton<IQuoteService, QuoteService>();

        // Extra
        builder.Services.AddSingleton<DateTimeService>(); ;

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);
        #endregion

        #endregion

        return builder.Build();
    }
}
