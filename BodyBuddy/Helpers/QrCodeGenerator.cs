using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BodyBuddy.Dtos;
using BodyBuddy.Models;

namespace BodyBuddy.Helpers
{
    public class QrCodeGenerator
    {

        #region Workout

        public static string GenerateWorkoutCode(WorkoutDto workoutDetails, List<ExerciseModel> exercises)
        {
            StringBuilder qrCodeData = new StringBuilder();

            // Append WorkoutDetails.Id
            AppendKeyValue(qrCodeData, QrCodeConstants.WorkoutName, Escape(workoutDetails.Name));
            AppendKeyValue(qrCodeData, QrCodeConstants.WorkoutDescription, Escape(workoutDetails.Description));

            // Append exercise details
            foreach (var exercise in exercises)
            {
                AppendKeyValue(qrCodeData, QrCodeConstants.ExerciseId, exercise.Id.ToString());
                AppendKeyValue(qrCodeData, QrCodeConstants.Sets, exercise.Sets.ToString());
                AppendKeyValue(qrCodeData, QrCodeConstants.Reps, exercise.Reps.ToString());
            }

            return qrCodeData.ToString();
        }

        public static void ReadWorkoutCode2(string qrCodeData, Action<string, string> setPropertyValue)
        {
            // Unescape the values before splitting
            qrCodeData = Unescape(qrCodeData);

            // Split the data into separate parts based on the delimiter ';'
            string[] parts = qrCodeData.Split(';');

            // Extract details
            foreach (var part in parts)
            {
                string[] keyValue = part.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    string value = keyValue[1];

                    setPropertyValue?.Invoke(key, Unescape(value));
                }
            }
        }

        public static List<ExerciseModel> ReadWorkoutCode(string qrCodeData, Action<string, string> setPropertyValue)
        {
            // Unescape the values before splitting
            qrCodeData = Unescape(qrCodeData);

            // Split the data into separate parts based on the delimiter ';'
            string[] parts = qrCodeData.Split(';');

            // Extract details
            List<ExerciseModel> exercises = new List<ExerciseModel>();

            ExerciseModel currentExercise = null;

            foreach (var part in parts)
            {
                string[] keyValue = part.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    string value = keyValue[1];

                    if (key == QrCodeConstants.ExerciseId)
                    {
                        int exerciseId;
                        if (int.TryParse(value, out exerciseId))
                        {
                            // Create a new exercise model for each ExerciseId
                            currentExercise = new ExerciseModel
                            {
                                Id = exerciseId,
                                Sets = 0, // Default value
                                Reps = 0   // Default value
                            };

                            exercises.Add(currentExercise);
                        }
                    }
                    else if (currentExercise != null)
                    {
                        // Set sets and reps for the current exercise
                        if (key == QrCodeConstants.Sets)
                        {
                            int sets;
                            if (int.TryParse(value, out sets))
                            {
                                currentExercise.Sets = sets;
                            }
                        }
                        else if (key == QrCodeConstants.Reps)
                        {
                            int reps;
                            if (int.TryParse(value, out reps))
                            {
                                currentExercise.Reps = reps;
                            }
                        }
                    }
                    else
                    {
                        setPropertyValue?.Invoke(key, Unescape(value));
                    }
                }
            }

            return exercises;
        }

        #endregion


        #region Helpers

        public static void AppendKeyValue(StringBuilder sb, string key, string value)
        {
            sb.Append($"{key}:{Escape(value)};");
        }

        public static string Escape(string value)
        {
            // Replace any ';' in the value with a placeholder 
            return value?.Replace(";", QrCodeConstants.SemicolonPlaceholder) ?? "";
        }

        public static string Unescape(string value)
        {
            // Replace the placeholder with ';'
            return value?.Replace(QrCodeConstants.SemicolonPlaceholder, ";") ?? "";
        }

        #endregion
    }

    public static class QrCodeConstants
    {
        public const string WorkoutName = "WorkoutName";
        public const string WorkoutDescription = "WorkoutDescription";
        public const string ExerciseId = "ExerciseId";
        public const string Sets = "Sets";
        public const string Reps = "Reps";
        public const string SemicolonPlaceholder = "##semicolon##";
    }
}
