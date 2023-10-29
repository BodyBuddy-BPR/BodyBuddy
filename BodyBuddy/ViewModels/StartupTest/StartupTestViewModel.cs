using BodyBuddy.Dtos;
using BodyBuddy.Helpers;
using BodyBuddy.Services;
using BodyBuddy.Views.Profile;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;
using BodyBuddy.Enums;
using BodyBuddy.Mappers;

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
    [ObservableProperty] private StartupTestDto _startupTestDto;

    //Others
    [ObservableProperty]
    private string _questionnaireText;

    public DateTime MinDate { get; } = new(1914, 7, 28);
    public DateTime MaxDate { get; } = DateTime.Now;

    public List<string> GenderList { get; }
    public List<string> ActivityList { get; }
    public List<string> GoalList { get; }
    public List<string> FocusAreaList { get; }
    //public List<bool> TargetSelectedStates { get; set; } = new() { false, false, false, false };
    #endregion

    public ICommand RadioButtonCheckedCommand { get; }

    private readonly IStartupTestService _startupTestService;

    public StartupTestViewModel(IStartupTestService startupTestService)
    {
        _startupTestService = startupTestService;
        StartupTestDto = new StartupTestDto();
        StartupTestDto.PropertyChanged += StartupTestDtoPropertyChanged;

        //Default selected birthday
        StartupTestDto.Birthday = new DateTime(2005, 1, 1);
        GenderList = InitializeGenderList();
        ActivityList = InitializeActivityList();
        GoalList = InitializeGoalList();
        FocusAreaList = InitializeFocusAreaList();

        //SetStateProperties Has to be first due timing
        SetStateProperties();
        UpdateVisibility();
        RadioButtonCheckedCommand = new Command<string>(OnRadioButtonChecked);
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
        _startupTestService.SaveStartupTestData(StartupTestDto);
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
    }


    //If any properties change, UpdateActionButtonsVisibility
    private void StartupTestDtoPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(StartupTestDto.Name):
            case nameof(StartupTestDto.Gender):
            case nameof(StartupTestDto.ActiveAmount):
            case nameof(StartupTestDto.Goal):
            case nameof(StartupTestDto.Weight):
            case nameof(StartupTestDto.Height):
            case nameof(StartupTestDto.PassiveCalorieBurn):
                UpdateActionButtonsVisibility();
                break;
        }
    }

    #region State Machine
    private StartupTestStates CurrentState { get; set; } = StartupTestStates.NameEntry;

    //Returning a bool depending on the CurrentState and the state properties
    private delegate bool CurrentStateDone();

    private CurrentStateDone currentStateDone;

    private void StateNext()
    {
        if (CurrentState == StartupTestStates.Done || !currentStateDone()) 
            return;

        CurrentState++;
        SetStateProperties();
        UpdateVisibility();
    }

    private void StatePrevious()
    {
        if (CurrentState == StartupTestStates.NameEntry)
            return;

        CurrentState--;
        SetStateProperties();
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        IsNameVisible = CurrentState == StartupTestStates.NameEntry;
        IsGenderVisible = CurrentState == StartupTestStates.GenderSelection;
        IsWeightVisible = CurrentState == StartupTestStates.WeightEntry;
        IsHeightVisible = CurrentState == StartupTestStates.HeightEntry;
        IsBirthdayVisible = CurrentState == StartupTestStates.BirthdaySelection;
        IsActiveVisible = CurrentState == StartupTestStates.ActivitySelection;
        IsPassiveCalorieBurnVisible = CurrentState == StartupTestStates.PassiveCalorieBurnEntry;
        IsGoalVisible = CurrentState == StartupTestStates.GoalSelection;
        UpdateActionButtonsVisibility();
    }

    private void UpdateActionButtonsVisibility()
    {
        BackIsVisible = CurrentState != StartupTestStates.NameEntry;
        NextIsVisible = CurrentState != StartupTestStates.Done && currentStateDone();
        SubmitDataIsVisible = CurrentState == StartupTestStates.Done && currentStateDone();
    }

    private void SetStateProperties()
    {
        switch (CurrentState)
        {
            case StartupTestStates.NameEntry:
                QuestionnaireText = "What is your name?";
                currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.Name);
                break;
            case StartupTestStates.GenderSelection:
                QuestionnaireText = "What is your gender?";
                currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.Gender);
                break;
            case StartupTestStates.WeightEntry:
                QuestionnaireText = "What is your weight?";
                currentStateDone = () => StartupTestDto.Weight > 0;
                break;
            case StartupTestStates.HeightEntry:
                QuestionnaireText = "When is your height?";
                currentStateDone = () => StartupTestDto.Height > 0;
                break;
            case StartupTestStates.BirthdaySelection:
                QuestionnaireText = "When is your birthday?";
                currentStateDone = () => true;
                break;
            case StartupTestStates.ActivitySelection:
                QuestionnaireText = "How active are you?";
                currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.ActiveAmount);
                break;
            case StartupTestStates.PassiveCalorieBurnEntry:
                QuestionnaireText = "What is your passive calorie burn?";
                currentStateDone = () => StartupTestDto.PassiveCalorieBurn > 0;
                break;
            case StartupTestStates.GoalSelection:
                QuestionnaireText = "What are your workout goals?";
                currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.Goal);
                break;
            case StartupTestStates.Done:
                QuestionnaireText = "You're done!";
                currentStateDone = () => true;
                SubmitDataIsVisible = true;
                break;
            default:
                throw new InvalidOperationException($"Unexpected state: {CurrentState}");

        }
    }

    //Whenever radiobutton changes
    private void OnRadioButtonChecked(string selectedValue)
    {
        switch (CurrentState)
        {
            case StartupTestStates.GenderSelection:
                StartupTestDto.Gender = selectedValue;
                break;
            case StartupTestStates.ActivitySelection:
                StartupTestDto.ActiveAmount = selectedValue;
                break;
            case StartupTestStates.GoalSelection:
                StartupTestDto.Goal = selectedValue;
                break;
        }
    }

    #endregion
    private static List<string> InitializeGenderList()
    {
        return Enum.GetValues(typeof(Gender))
            .Cast<Gender>()
            .Select(EnumMapper.GetDisplayString)
            .ToList();
    }

    private static List<string> InitializeActivityList()
    {
        return Enum.GetValues(typeof(UserActivity))
            .Cast<UserActivity>()
            .Select(EnumMapper.GetDisplayString)
            .ToList();
    }

    private static List<string> InitializeGoalList()
    {
        return Enum.GetValues(typeof(Goal))
            .Cast<Goal>()
            .Select(EnumMapper.GetDisplayString)
            .ToList();
    }
    private static List<string> InitializeFocusAreaList()
    {
        return Enum.GetValues(typeof(FocusArea))
            .Cast<FocusArea>()
            .Select(EnumMapper.GetDisplayString)
            .ToList();
    }
}