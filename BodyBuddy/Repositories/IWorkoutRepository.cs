﻿using BodyBuddy.Models;

namespace BodyBuddy.Repositories
{
    public interface IWorkoutRepository
    {
        Task<List<WorkoutModel>> GetWorkoutPlansAsync(int isPreMade);

        Task<WorkoutModel> GetSpecificWorkoutAsync(string name);

		Task<int> PostWorkoutPlanAsync(WorkoutModel workout);

		Task<bool> DeleteWorkout(WorkoutModel workout);

		Task<bool> DoesWorkoutAlreadyExist(string name);
    }


}
