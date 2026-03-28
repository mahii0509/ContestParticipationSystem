using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContestParticipationSystem.Data;
using ContestParticipationSystem.Models;
using System.Linq;

namespace ContestParticipationSystem.Controllers
{
    [ApiController]
    [Route("api/contest")]
    public class ContestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContestController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("create")]
        public IActionResult CreateContest(Contest contest)
        {
            _context.Contests.Add(contest);
            _context.SaveChanges();

            return Ok(contest);
        }

        [HttpGet]
        public IActionResult GetAllContests()
        {
            var contests = _context.Contests.ToList();
            return Ok(contests);
        }

        [HttpGet("{contestId}/leaderboard")]
        public IActionResult Leaderboard(int contestId)
        {
            var leaderboard = _context.ContestParticipants
                .Where(x => x.ContestId == contestId && x.IsSubmitted)
                .OrderByDescending(x => x.Score)
                .Select(x => new
                {
                    user = x.User.Username,
                    score = x.Score
                })
                .ToList();

            return Ok(leaderboard);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{contestId}/declare-winner")]
        public IActionResult DeclareWinner(int contestId)
        {
            var winner = _context.ContestParticipants
                .Where(x => x.ContestId == contestId && x.IsSubmitted)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (winner == null)
                return BadRequest("No submissions yet");

            return Ok(new
            {
                Winner = winner.User.Username,
                Score = winner.Score
            });
        }
    }
}