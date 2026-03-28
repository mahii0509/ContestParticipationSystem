namespace ContestParticipationSystem.Models
{
    public enum ContestAccess
    {
        NORMAL,
        VIP
    }

    public class Contest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ContestAccess AccessLevel { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Prize { get; set; }
    }
}