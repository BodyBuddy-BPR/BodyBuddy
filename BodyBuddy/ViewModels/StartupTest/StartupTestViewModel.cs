using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Services;
using BodyBuddy.Views.Profile;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace BodyBuddy.ViewModels.StartupTest;

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
    [ObservableProperty] private string _name, _gender, _active, _goal;
    [ObservableProperty] private double _weight;
    [ObservableProperty] private int _height, _passiveCalorieBurn;
    [ObservableProperty] private DateTime _selectedDate = new(2005, 1, 1);

    //Others
    [ObservableProperty]
    private string _questionnaireText;

    [ObservableProperty] private DateTime _minDate = new(1914, 7, 28);
    [ObservableProperty] private DateTime _maxDate = DateTime.Now;


    //Setting lists for multiple choice questions (Add more here, if more options become available
    public List<string> GenderList { get; } = new()
        { Strings.STARTUP_GENDER_FEMALE, Strings.STARTUP_GENDER_MALE, Strings.STARTUP_GENDER_NONE };

    public List<string> ActivityList { get; } = new()
    {
        Strings.STARTUP_ACTIVITY_VERYACTIVE, Strings.STARTUP_ACTIVITY_ACTIVE,
        Strings.STARTUP_ACTIVITY_LITTLEACTIVE, Strings.STARTUP_ACTIVITY_NOTVERYACTIVE
    };

    public List<string> GoalList { get; } = new() { Strings.STARTUP_GOAL_LOSEWEIGHT, Strings.STARTUP_GOAL_GAINMUSCLE };

    public List<string> TargetList { get; } = new()
    {
        Strings.STARTUP_FOCUSAREA_UPPERBODY, Strings.STARTUP_FOCUSAREA_LOWERBODY,
        Strings.STARTUP_FOCUSAREA_ABSANDCORE, Strings.STARTUP_FOCUSAREA_OTHER
    };

    public List<bool> TargetSelectedStates { get; set; } = new() { false, false, false, false };

    public ICommand RadioButtonCheckedCommand { get; }

    private readonly IStartupTestService _startupTestService;

    #endregion

    public StartupTestViewModel(IStartupTestService startupTestService)
    {
        _startupTestService = startupTestService;

        //SetStateProperties HAS TO BE FIRST! (Timing)
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
        IsNameVisible = CurrentState == State.NameEntry;
        IsGenderVisible = CurrentState == State.GenderSelection;
        IsWeightVisible = CurrentState == State.WeightEntry;
        IsHeightVisible = CurrentState == State.HeightEntry;
        IsBirthdayVisible = CurrentState == State.BirthdaySelection;
        IsActiveVisible = CurrentState == State.ActivitySelection;
        IsPassiveCalorieBurnVisible = CurrentState == State.PassiveCalorieBurnEntry;
        IsGoalVisible = CurrentState == State.GoalSelection;
        UpdateActionButtonsVisibility();
    }

    private void UpdateActionButtonsVisibility()
    {
        BackIsVisible = CurrentState != State.NameEntry;
        NextIsVisible = CurrentState != State.Done && currentStateDone();
        SubmitDataIsVisible = CurrentState == State.Done && currentStateDone();
    }

    private void SetStateProperties()
    {
        switch (CurrentState)
        {
            case State.NameEntry:
                QuestionnaireText = "What is your name?";
                currentStateDone = () => !string.IsNullOrEmpty(Name);
                break;
            case State.GenderSelection:
                QuestionnaireText = "What is your gender?";
                currentStateDone = () => !string.IsNullOrEmpty(Gender);
                break;
            case State.WeightEntry:
                QuestionnaireText = "What is your weight?";
                currentStateDone = () => Weight > 0;
                break;
            case State.HeightEntry:
                QuestionnaireText = "When is your height?";
                currentStateDone = () => Height > 0;
                break;
            case State.BirthdaySelection:
                QuestionnaireText = "When is your birthday?";
                currentStateDone = () => true;
                break;
            case State.ActivitySelection:
                QuestionnaireText = "How active are you?";
                currentStateDone = () => !string.IsNullOrEmpty(Active);
                break;
            case State.PassiveCalorieBurnEntry:
                QuestionnaireText = "What is your passive calorie burn?";
                currentStateDone = () => PassiveCalorieBurn > 0;
                break;
            case State.GoalSelection:
                QuestionnaireText = "What are your workout goals?";
                currentStateDone = () => !string.IsNullOrEmpty(Goal);
                break;
            case State.Done:
                QuestionnaireText = "You're done!";
                currentStateDone = () => true;
                SubmitDataIsVisible = true;
                break;
            default:
                throw new InvalidOperationException($"Unexpected state: {CurrentState}");

        }
    }

    #endregion
}