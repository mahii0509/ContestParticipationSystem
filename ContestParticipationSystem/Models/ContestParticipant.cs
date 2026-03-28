namespace ContestParticipationSystem.Models
{
    public class ContestParticipant
    {
        public int Id { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int Score { get; set; } = 0;

        public bool IsSubmitted { get; set; } = false;

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}