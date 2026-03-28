namespace ContestParticipationSystem.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public int ParticipantId { get; set; }

        public int QuestionId { get; set; }

        public int OptionId { get; set; }
    }
}