namespace BodyBuddy.Dtos
{
    public class ChallengeDto
    {
        public int ActiveChallengeId { get; set; }
        public string Type { get; set; }
        public int Goal { get; set; }
        public int Progress { get; set; }
        public string Difficulty { get; set; }
        public DateTime From {get; set; }
        public DateTime To { get; set; }
        public List<UserTotalSteps> UserTotalSteps { get; set; }
    }
}
