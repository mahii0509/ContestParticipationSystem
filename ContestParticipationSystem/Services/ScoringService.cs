using ContestParticipationSystem.Data;
using ContestParticipationSystem.DTOs;
using ContestParticipationSystem.Models;

namespace ContestParticipationSystem.Services
{
    public class ScoringService
    {
        private readonly AppDbContext _context;

        public ScoringService(AppDbContext context)
        {
            _context = context;
        }

        public int CalculateScore(List<QuestionAnswerDTO> answers, List<Question> questions)
        {
            int score = 0;

            foreach (var answer in answers)
            {
                var correctOptions = _context.Options
                    .Where(o => o.QuestionId == answer.QuestionId && o.IsCorrect)
                    .Select(o => o.Id)
                    .ToList();

                if (correctOptions.Count == answer.SelectedOptionIds.Count &&
                    !correctOptions.Except(answer.SelectedOptionIds).Any())
                {
                    score += 10;
                }
            }

            return score;
        }
    }
}