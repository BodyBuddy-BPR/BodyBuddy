﻿using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Services;
using BodyBuddy.Views.Profile;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace BodyBuddy.ViewModels.StartupTest
{
    public partial class StartupTestViewModel : BaseViewModel
    {


        #region ObservableProperties
        //IsVisible
        [ObservableProperty]
        private bool _isNameVisible, _isGenderVisible, _isWeightVisible, _isHeightVisible;
        [ObservableProperty]
        private bool _isBirthdayVisible, _isActiveVisible, _isPassiveCalorieBurnVisible, _isGoalVisible;
        [ObservableProperty]
        private bool _submitDataIsVisible, _nextIsVisible, _backIsVisible;

        //Saved Properties
        [ObservableProperty] private string name, gender, active, goal;
        [ObservableProperty] private double weight;
        [ObservableProperty] private int height, passiveCalorieBurn;
        [ObservableProperty] private DateTime selectedDate = new(2005, 1, 1);

        //Others
        [ObservableProperty]
        private string _questionaireText;

        [ObservableProperty] private DateTime minDate = new(1914, 7, 28);
        [ObservableProperty] private DateTime maxDate = DateTime.Now;


        //Setting lists for multiple choice questions (Add more here, if more options become available
        public List<string> GenderList { get; } = new List<string> { Strings.STARTUP_GENDER_FEMALE, Strings.STARTUP_GENDER_MALE, Strings.STARTUP_GENDER_NONE };
        public List<string> ActivityList { get; } = new List<string> { Strings.STARTUP_ACTIVITY_VERYACTIVE, Strings.STARTUP_ACTIVITY_ACTIVE,
            Strings.STARTUP_ACTIVITY_LITTLEACTIVE, Strings.STARTUP_ACTIVITY_NOTVERYACTIVE };
        public List<string> GoalList { get; } = new List<string> { Strings.STARTUP_GOAL_LOSEWEIGHT, Strings.STARTUP_GOAL_GAINMUSCLE };
        public List<string> TargetList { get; } = new List<string> { Strings.STARTUP_FOCUSAREA_UPPERBODY, Strings.STARTUP_FOCUSAREA_LOWERBODY,
            Strings.STARTUP_FOCUSAREA_ABSANDCORE, Strings.STARTUP_FOCUSAREA_OTHER };
        
        public List<bool> TargetSelectedStates { get; set; } = new List<bool> { false, false, false, false };

        public ICommand RadioButtonCheckedCommand { get; }

        private IStartupTestService _startupTestService;
        #endregion

        public StartupTestViewModel(IStartupTestService startupTestService)
        {
            _startupTestService = startupTestService;

            //SetStateProperties HAS TO BE FIRST!
            SetStateProperties();
            UpdateVisibility();
            RadioButtonCheckedCommand = new Command<string>(OnRadioButtonChecked);

            PropertyChanged += OnPropertyChange;
        }

        [RelayCommand]
        public void NextButton()
        {
            StateNext();
        }

        [RelayCommand]
        public void BackButton()
        {
            StatePrevious();
        }

        [RelayCommand]
        public async Task SubmitData()
        {
            StartupTestDto startupTestData = new()
            {
                Name = Name,
                Gender = Gender,
                ActiveAmount = Active,
                Goal = Goal,
                Weight = Weight,
                Height = Height,
                PassiveCalorieBurn = PassiveCalorieBurn,
                Birthday = SelectedDate
            };

            _startupTestService.SaveStartupTestData(startupTestData);

            await Task.Delay(100); // Add a short delay
            // The // in front resets the stack, so there is no back button
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
        }

        //Whenever radiobutton changes
        private void OnRadioButtonChecked(string selectedValue)
        {
            if (CurrentState == State.GenderSelection)
                Gender = selectedValue;
            else if (CurrentState == State.ActivitySelection)
                Active = selectedValue;
            else if (CurrentState == State.GoalSelection)
                Goal = selectedValue;
        }

        #region State Machine
        public enum State
        {
            NameEntry,
            GenderSelection,
            WeightEntry,
            HeightEntry,
            BirthdaySelection,
            ActivitySelection,
            PassiveCalorieBurnEntry,
            GoalSelection,
            Done
        }
        private State CurrentState { get; set; } = State.NameEntry;

        //Returning a bool depending on the CurrentState and the state properties
        private delegate bool CurrentStateDone();
        private CurrentStateDone currentStateDone;

        private void StateNext()
        {
            if (CurrentState != State.Done && currentStateDone())
            {
                CurrentState++;
                SetStateProperties();
                UpdateVisibility();
            }
        }

        private void StatePrevious()
        {
            if (CurrentState != State.NameEntry)
            {
                CurrentState--;
                SetStateProperties();
                UpdateVisibility();
            }
        }

        //Used to check on any property changed and UpdateActionButtons if any of the following below are changed
        private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Name):
                case nameof(Gender):
                case nameof(Active):
                case nameof(Goal):
                case nameof(Weight):
                case nameof(Height):
                case nameof(PassiveCalorieBurn):
                    UpdateActionButtonsVisibility();
                    break;
            }
        }

        private void UpdateVisibility()
        {
            IsNameVisible = (CurrentState == State.NameEntry);
            IsGenderVisible = (CurrentState == State.GenderSelection);
            IsWeightVisible = (CurrentState == State.WeightEntry);
            IsHeightVisible = (CurrentState == State.HeightEntry);
            IsBirthdayVisible = (CurrentState == State.BirthdaySelection);
            IsActiveVisible = (CurrentState == State.ActivitySelection);
            IsPassiveCalorieBurnVisible = (CurrentState == State.PassiveCalorieBurnEntry);
            IsGoalVisible = (CurrentState == State.GoalSelection);
            UpdateActionButtonsVisibility();
        }
        private void UpdateActionButtonsVisibility()
        {
            BackIsVisible = (CurrentState != State.NameEntry);
            NextIsVisible = (CurrentState != State.Done && currentStateDone());
            SubmitDataIsVisible = (CurrentState == State.Done && currentStateDone());
        }

        private void SetStateProperties()
        {
            switch (CurrentState)
            {
                case State.NameEntry:
                    QuestionaireText = "What is your Name?";
                    currentStateDone = () => { return !string.IsNullOrEmpty(Name); };
                    break;
                case State.GenderSelection:
                    QuestionaireText = "What is your gender?";
                    currentStateDone = () => { return !string.IsNullOrEmpty(Gender); };
                    break;
                case State.WeightEntry:
                    QuestionaireText = "What is your weight?";
                    currentStateDone = () => { return Weight > 0; };
                    break;
                case State.HeightEntry:
                    QuestionaireText = "When is your height?";
                    currentStateDone = () => { return Height > 0; };
                    break;
                case State.BirthdaySelection:
                    QuestionaireText = "When is your birthday?";
                    currentStateDone = () => { return true; };
                    break;
                case State.ActivitySelection:
                    QuestionaireText = "How active are you?";
                    currentStateDone = () => { return !string.IsNullOrEmpty(Active); };
                    break;
                case State.PassiveCalorieBurnEntry:
                    QuestionaireText = "What is your passive calorie burn?";
                    currentStateDone = () => { return PassiveCalorieBurn > 0; };
                    break;
                case State.GoalSelection:
                    QuestionaireText = "What are your goals?";
                    currentStateDone = () => { return !string.IsNullOrEmpty(Goal); };
                    break;
                case State.Done:
                    QuestionaireText = "You're done!";
                    currentStateDone = () => { return true; };
                    SubmitDataIsVisible = true;
                    break;
            }
        }
        #endregion
    }
}