namespace ContestParticipationSystem.DTOs
{
    public class AnswerDTO
    {
        public int QuestionId { get; set; }

        public List<int> OptionIds { get; set; }
    }
}