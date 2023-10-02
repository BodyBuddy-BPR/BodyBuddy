using BodyBuddy.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.StartupTest
{
    public partial class StartupTestViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _questionaireText;

        public StartupTestViewModel()
        {
            IsNameVisible = true;
            QuestionaireText = "What is your GG";
        }

        [RelayCommand]
        public void ButtonTest()
        {
            var properties = new Dictionary<string, Func<bool>>
        {
            { "IsNameVisible", () => IsNameVisible },
            { "IsGenderVisible", () => IsGenderVisible },
            { "IsWeightVisible", () => IsWeightVisible },
            { "IsHeightVisible", () => IsHeightVisible },
            { "IsAgeVisible", () => IsAgeVisible },
            { "IsActiveVisible", () => IsActiveVisible },
            { "IsPassiveCalorieBurnVisible", () => IsPassiveCalorieBurnVisible },
            { "IsGoalVisible", () => IsGoalVisible },
        };
            var activeProperty = properties.FirstOrDefault(kvp => kvp.Value()).Key;

            switch (activeProperty)
            {
                case "IsNameVisible":
                    IsNameVisible = !IsNameVisible;
                    IsGenderVisible = !IsGenderVisible;
                    QuestionaireText = "What is your gender ?";
                    break;
                case
                    "IsGenderVisible":
                    IsGenderVisible = !IsGenderVisible;
                    IsWeightVisible = !IsWeightVisible;
                    QuestionaireText = "What is your weight?";
                    break;
                case
                    "IsWeightVisible":
                    IsWeightVisible = !IsWeightVisible;
                    IsHeightVisible = !IsHeightVisible;
                    QuestionaireText = "What is your height?";
                    break;
                case "IsHeightVisible":
                    IsHeightVisible = !IsHeightVisible;
                    IsAgeVisible = !IsAgeVisible;
                    QuestionaireText = "What is your age?";
                    break;
                case "IsAgeVisible":
                    IsAgeVisible = !IsAgeVisible;
                    IsActiveVisible = !IsActiveVisible;
                    QuestionaireText = "How active are you?";
                    break;
                case "IsActiveVisible":
                    IsActiveVisible = !IsActiveVisible;
                    IsPassiveCalorieBurnVisible = !IsPassiveCalorieBurnVisible;
                    QuestionaireText = "What is your passive calorie burn?";
                    break;
                case "IsPassiveCalorieBurnVisible":
                    IsPassiveCalorieBurnVisible = !IsPassiveCalorieBurnVisible;
                    IsGoalVisible = !IsGoalVisible;
                    QuestionaireText = "What is your goals?";
                    break;
                case "IsGoalVisible":
                    break;
                default:
                    break;

            }
        }

        private void DisableAllText()
        {
            IsNameVisible = false;
            IsGenderVisible = false;
            IsWeightVisible = false;
            IsHeightVisible = false;
            IsAgeVisible = false;
            IsActiveVisible = false;
            IsPassiveCalorieBurnVisible = false;
            IsGoalVisible = false;
        }


        [ObservableProperty]
        private bool _isNameVisible;
        [ObservableProperty]
        private bool _isGenderVisible;
        [ObservableProperty]
        private bool _isWeightVisible;
        [ObservableProperty]
        private bool _isHeightVisible;
        [ObservableProperty]
        private bool _isAgeVisible;
        [ObservableProperty]
        private bool _isActiveVisible;
        [ObservableProperty]
        private bool _isPassiveCalorieBurnVisible;
        [ObservableProperty]
        private bool _isGoalVisible;
    }
}