using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Dtos
{
    public partial class ChallengeDto : ObservableObject
    {
        [ObservableProperty] private int _activeChallengeId;
        [ObservableProperty] private string _type;
        [ObservableProperty] private int _goal;
        [ObservableProperty] private int _progress;
        [ObservableProperty] private string _difficulty;
        [ObservableProperty] private DateTime _from;
        [ObservableProperty] private DateTime _to;
        [ObservableProperty] private List<UserTotalSteps> _userTotalSteps;
        private double userProgressInPercent;
        private double progressInPercent;

        public double UserProgressInPercent
        {
            get => userProgressInPercent;
            set => SetProperty(ref userProgressInPercent, (double)UserTotalSteps.Sum(uts => uts.TotalSteps) / Goal);
        }

        public double ProgressInPercent
        {
            get => progressInPercent;
            set => SetProperty(ref progressInPercent, (double)Progress / Goal);
        }
    }
}
