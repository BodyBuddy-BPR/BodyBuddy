using BodyBuddy.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.Dtos
{
    public class ChallengeDto : ObservableObject
    {
        public int ActiveChallengeId { get; set; }
        public string Type { get; set; }
        public int Goal { get; set; }
        public int Progress { get; set; }
        public double ProgressInPercent => (double)Progress / Goal;
        public string Difficulty { get; set; }
        public DateTime From {get; set; }
        public DateTime To { get; set; }
        public List<UserTotalSteps> UserTotalSteps { get; set; }
        public double UserProgressInPercent
        {
            get
            {
                int totalUserSteps = UserTotalSteps.Sum(uts => uts.TotalSteps);
                return (double)totalUserSteps / Goal;
            }
        }
    }
}
