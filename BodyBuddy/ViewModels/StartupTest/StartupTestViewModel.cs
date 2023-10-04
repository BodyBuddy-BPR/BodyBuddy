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
        #region ObservableProperties
        [ObservableProperty]
        private bool _isNameVisible, _isGenderVisible, _isWeightVisible, _isHeightVisible;
        [ObservableProperty]
        private bool _isAgeVisible, _isActiveVisible, _isPassiveCalorieBurnVisible, _isGoalVisible;

        [ObservableProperty]
        private string _questionaireText, _forwardButtonText;

        //Saved Properties
        [ObservableProperty]
        private string name;
        
        [ObservableProperty]
        private double weight;
        
        [ObservableProperty]
        private int height;

        #endregion

        public StartupTestViewModel()
        {
            IsNameVisible = true;
            QuestionaireText = "What is your Name?";
            ForwardButtonText = "Next Question";
        }



        [RelayCommand]
        public void ButtonTest()
        {
            var steps = new List<(string PropertyName, Func<bool> Getter, Action<bool> Setter, string NextQuestionText)>
{
    ("IsNameVisible", () => IsNameVisible, value => IsNameVisible = value, "What is your gender?"),
    ("IsGenderVisible", () => IsGenderVisible, value => IsGenderVisible = value, "What is your weight?"),
    ("IsWeightVisible", () => IsWeightVisible, value => IsWeightVisible = value, "What is your height?"),
    ("IsHeightVisible", () => IsHeightVisible, value => IsHeightVisible = value, "What is your age?"),
    ("IsAgeVisible", () => IsAgeVisible, value => IsAgeVisible = value, "How active are you?"),
    ("IsActiveVisible", () => IsActiveVisible, value => IsActiveVisible = value, "What is your passive calorie burn?"),
    ("IsPassiveCalorieBurnVisible", () => IsPassiveCalorieBurnVisible, value => IsPassiveCalorieBurnVisible = value, "What are your goals?"),
    ("IsGoalVisible", () => IsGoalVisible, value => IsGoalVisible = value, "The end, well done!")
};

            for (int i = 0; i < steps.Count; i++)
            {
                if (steps[i].Getter())
                {
                    // Set the current flag to false
                    steps[i].Setter(false);

                    // If there's a next step, set its flag to true and update the question text
                    if (i + 1 < steps.Count)
                    {
                        steps[i + 1].Setter(true);
                        QuestionaireText = steps[i].NextQuestionText;
                    }
                    else
                    {
                        ForwardButtonText = steps[i].NextQuestionText;
                    }

                    return;
                }
            }

        }

        //private void DisableAllText()
        //{
        //    IsNameVisible = false;
        //    IsGenderVisible = false;
        //    IsWeightVisible = false;
        //    IsHeightVisible = false;
        //    IsAgeVisible = false;
        //    IsActiveVisible = false;
        //    IsPassiveCalorieBurnVisible = false;
        //    IsGoalVisible = false;
        //}



    }
}