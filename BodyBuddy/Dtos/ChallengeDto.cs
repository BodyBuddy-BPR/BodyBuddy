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
        [ObservableProperty] private double _progressInPercent;

        public void RecalculateProgressInPercent()
        {
            ProgressInPercent = Goal != 0 && Progress != 0 ? (double)Progress / Goal : 0;
        }
    }
}