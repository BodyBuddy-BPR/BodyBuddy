using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using BodyBuddy.Database;
using Syncfusion.Maui.Core.Hosting;

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

		// Views
		builder.Services.AddSingleton<WorkoutPlansPage>();
		builder.Services.AddSingleton<MyExercisesPage>();
		builder.Services.AddTransient<NewExercisePage>();
		builder.Services.AddTransient<WorkoutPlanDetailsPage>();

		// ViewModels
		builder.Services.AddSingleton<WorkoutPlansViewModel>();
        builder.Services.AddSingleton<MyExercisesViewModel>();
		builder.Services.AddTransient<NewExerciseViewModel>();
		builder.Services.AddTransient<WorkoutPlanDetailsViewModel>();

		// Repositories

		// Database
		builder.Services.AddSingleton<LocalDatabase>();

		return builder.Build();
	}
}
