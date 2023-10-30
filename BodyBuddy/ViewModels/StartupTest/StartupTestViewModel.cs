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
	private bool _isNameVisible, _isGenderVisible, _isWeightVisible, _isHeightVisible, _isNextButtonVisible = true;
	[ObservableProperty]
	private bool _isBirthdayVisible, _isActiveVisible, _isPassiveCalorieBurnVisible, _isGoalVisible, _isWelcomeVisible;
	[ObservableProperty]
	private bool _submitDataIsVisible, _nextIsEnabled, _backIsVisible = false;

	//Saved Properties
	[ObservableProperty] private StartupTestDto _startupTestDto;

	//Others
	[ObservableProperty]
	private string _questionnaireText;
	[ObservableProperty]
	private double _startupTestProgress;

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
	private StartupTestStates CurrentState { get; set; } = StartupTestStates.Welcome;

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
		if (CurrentState == StartupTestStates.Welcome)
			return;

		CurrentState--;
		SetStateProperties();
		UpdateVisibility();
	}

	private void UpdateVisibility()
	{
		IsWelcomeVisible = CurrentState == StartupTestStates.Welcome;
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
		BackIsVisible = CurrentState != StartupTestStates.Welcome;
		NextIsEnabled = CurrentState != StartupTestStates.Done && currentStateDone();
		SubmitDataIsVisible = CurrentState == StartupTestStates.Done && currentStateDone();
	}

	private void SetStateProperties()
	{
		switch (CurrentState)
		{
			case StartupTestStates.Welcome:
				QuestionnaireText = "";
				StartupTestProgress = (double)0 / 8;
				currentStateDone = () => true;
				BackIsVisible = false;
				IsWelcomeVisible = true;
				break;
			case StartupTestStates.NameEntry:
				QuestionnaireText = "What is your name?";
				StartupTestProgress = (double)0 / 8;
				BackIsVisible = true;
				IsWelcomeVisible = false;
				currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.Name);
				break;
			case StartupTestStates.GenderSelection:
				QuestionnaireText = "What is your gender?";
				StartupTestProgress = (double)1 / 8;
				currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.Gender);
				break;
			case StartupTestStates.WeightEntry:
				QuestionnaireText = "What do you weigh?";
				StartupTestProgress = (double)2 / 8;
				currentStateDone = () => StartupTestDto.Weight > 0;
				break;
			case StartupTestStates.HeightEntry:
				QuestionnaireText = "How tall are you?";
				StartupTestProgress = (double)3 / 8;
				currentStateDone = () => StartupTestDto.Height > 0;
				break;
			case StartupTestStates.BirthdaySelection:
				QuestionnaireText = "When is your birthday?";
				StartupTestProgress = (double)4 / 8;
				currentStateDone = () => true;
				break;
			case StartupTestStates.ActivitySelection:
				QuestionnaireText = "How active are you?";
				StartupTestProgress = (double)5 / 8;
				currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.ActiveAmount);
				break;
			case StartupTestStates.PassiveCalorieBurnEntry:
				QuestionnaireText = "Passive Calorie Burn";
				StartupTestDto.PassiveCalorieBurn = CalculatePCB();
				StartupTestProgress = (double)6 / 8;
				currentStateDone = () => StartupTestDto.PassiveCalorieBurn > 0;
				break;
			case StartupTestStates.GoalSelection:
				QuestionnaireText = "What are your goals?";
				StartupTestProgress = (double)7 / 8;
				currentStateDone = () => !string.IsNullOrEmpty(StartupTestDto.Goal);
				IsNextButtonVisible = true;
				break;
			case StartupTestStates.Done:
				QuestionnaireText = "You're all set!";
				StartupTestProgress = (double)8 / 8;
				currentStateDone = () => true;
				SubmitDataIsVisible = true;
				IsNextButtonVisible = false;
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

	private int CalculatePCB()
	{
		int age = DateTime.Now.Year - StartupTestDto.Birthday.Year;

		double pcb;

		if (StartupTestDto.Gender == "Female")
		{
			pcb = (655.1 + (9.247 * StartupTestDto.Weight) + (3.098 * StartupTestDto.Height) - (4.330 * age));
		}
		else
		{
			pcb = (int)(66.0 + (13.75 * StartupTestDto.Weight) + (5.003 * StartupTestDto.Height) - (6.755 * age));
		}

		double activityFactor = 1.2;

		switch (StartupTestDto.ActiveAmount)
		{
			case "A Little Active":
				activityFactor = 1.375;
				break;
			case "Active":
				activityFactor = 1.55;
				break;
			case "Very Active":
				activityFactor = 1.725;
				break;
		}

		return (int)(pcb * activityFactor);
	}
}