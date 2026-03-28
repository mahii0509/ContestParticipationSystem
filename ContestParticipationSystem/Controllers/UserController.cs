using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContestParticipationSystem.Data;
using System.Security.Claims;
using System.Linq;

namespace ContestParticipationSystem.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("history")]
        public IActionResult History()
        {
            var userId = int.Parse(User.FindFirst("id").Value);

            var history = _context.ContestParticipants
                .Where(x => x.UserId == userId && x.IsSubmitted)
                .Select(x => new
                {
                    contest = x.Contest.Name,
                    score = x.Score
                }).ToList();

            return Ok(history);
        }

        [Authorize]
        [HttpGet("in-progress")]
        public IActionResult InProgress()
        {
            var userId = int.Parse(User.FindFirst("id").Value);

            var contests = _context.ContestParticipants
                .Where(x => x.UserId == userId && !x.IsSubmitted)
                .Select(x => new
                {
                    contest = x.Contest.Name,
                    joinedAt = x.JoinedAt
                }).ToList();

            return Ok(contests);
        }
    }
}