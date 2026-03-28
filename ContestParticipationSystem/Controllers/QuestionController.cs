using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContestParticipationSystem.Data;
using ContestParticipationSystem.Models;
using System.Linq;

namespace ContestParticipationSystem.Controllers
{
    [ApiController]
    [Route("api/question")]
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("add")]
        public IActionResult AddQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();

            return Ok(question);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("add-option")]
        public IActionResult AddOption(Option option)
        {
            _context.Options.Add(option);
            _context.SaveChanges();

            return Ok(option);
        }

        [HttpGet("contest/{contestId}")]
        public IActionResult GetQuestions(int contestId)
        {
            var questions = _context.Questions
                .Where(x => x.ContestId == contestId)
                .Select(q => new
                {
                    q.Id,
                    q.Text,
                    q.Type,
                    Options = q.Options.Select(o => new
                    {
                        o.Id,
                        o.Text
                    })
                }).ToList();

            return Ok(questions);
        }
    }
}