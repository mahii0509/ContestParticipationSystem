namespace ContestParticipationSystem.DTOs
{
    public class SubmitAnswerDTO
    {
        public int ContestId { get; set; }
        public List<QuestionAnswerDTO> Answers { get; set; }
    }

    public class QuestionAnswerDTO
    {
        public int QuestionId { get; set; }
        public List<int> SelectedOptionIds { get; set; }
    }
}