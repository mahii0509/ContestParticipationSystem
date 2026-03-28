using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContestParticipationSystem.Data;
using ContestParticipationSystem.Models;
using ContestParticipationSystem.DTOs;
using ContestParticipationSystem.Services;
using System.Security.Claims;
using System.Linq;

namespace ContestParticipationSystem.Controllers
{
    [ApiController]
    [Route("api/participation")]
    public class ParticipationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ScoringService _scoringService;

        public ParticipationController(AppDbContext context, ScoringService scoringService)
        {
            _context = context;
            _scoringService = scoringService;
        }

        [Authorize]
        [HttpPost("join/{contestId}")]
        public IActionResult JoinContest(int contestId)
        {
            var userId = int.Parse(User.FindFirst("id").Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            var contest = _context.Contests.Find(contestId);

            if (contest == null)
                return NotFound("Contest not found");

            if (contest.AccessLevel == ContestAccess.VIP && role == "USER")
                return Forbid("VIP contest only");

            bool alreadyJoined = _context.ContestParticipants
                .Any(x => x.UserId == userId && x.ContestId == contestId);

            if (alreadyJoined)
                return BadRequest("Already joined");

            var participant = new ContestParticipant
            {
                UserId = userId,
                ContestId = contestId
            };

            _context.ContestParticipants.Add(participant);
            _context.SaveChanges();

            return Ok("Joined successfully");
        }

        [Authorize]
        [HttpPost("submit")]
        public IActionResult SubmitAnswers(SubmitAnswerDTO dto)
        {
            var userId = int.Parse(User.FindFirst("id").Value);

            var participant = _context.ContestParticipants
                .FirstOrDefault(x => x.UserId == userId && x.ContestId == dto.ContestId);

            if (participant == null)
                return BadRequest("User not joined");

            if (participant.IsSubmitted)
                return BadRequest("Already submitted");

            var questions = _context.Questions
                .Where(q => q.ContestId == dto.ContestId)
                .ToList();

            int score = _scoringService.CalculateScore(dto.Answers, questions);

            participant.Score = score;
            participant.IsSubmitted = true;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Submission successful",
                score = score
            });
        }
    }
}