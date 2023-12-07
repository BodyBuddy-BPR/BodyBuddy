namespace BodyBuddy.Services.Implementations
{
    public class LoginDatabaseFlowService : ILoginDatabaseFlowService
    {
        private readonly IStartupTestService _startupTestService;
        private readonly IStepService _stepService;
        private readonly IIntakeService _intakeService;
        private readonly IWorkoutService _workoutService;
        private readonly IWorkoutExercisesService _workoutExercisesService;
        private readonly IExerciseRecordsService _exerciseRecordsService;

        public LoginDatabaseFlowService(IStartupTestService startupTestService, IStepService stepService, IIntakeService intakeService,
            IWorkoutService workoutService, IWorkoutExercisesService workoutExercisesService, IExerciseRecordsService exerciseRecordsService)
        {
            _startupTestService = startupTestService;
            _stepService = stepService;
            _intakeService = intakeService;
            _workoutService = workoutService;
            _workoutExercisesService = workoutExercisesService;
            _exerciseRecordsService = exerciseRecordsService;
        }

        public async Task StartLoginDatabaseFlow()
        {
            await ReplaceStartupTestData();
        }

        private async Task ReplaceStartupTestData()
        {
            await _startupTestService.ReplaceSQLiteDataWithRemoteData();
            await _stepService.ReplaceSQLiteDataWithRemoteData();
            await _intakeService.ReplaceSQLiteDataWithRemoteData();
            //Deleting Workout and WorkoutExercises
            await _workoutService.ReplaceSQLiteDataWithRemoteData();
            //Only adding WorkoutExercises
            
            await _workoutExercisesService.ReplaceSQLiteDataWithRemoteData();
            await _exerciseRecordsService.ReplaceSQLiteDataWithRemoteData();

        }
    }
}
