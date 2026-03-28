using ContestParticipationSystem.Data;
using ContestParticipationSystem.Models;

namespace ContestParticipationSystem.Services
{
    public class ContestService
    {
        private readonly AppDbContext _context;

        public ContestService(AppDbContext context)
        {
            _context = context;
        }

        public Contest CreateContest(Contest contest)
        {
            _context.Contests.Add(contest);
            _context.SaveChanges();
            return contest;
        }

        public List<Contest> GetContests()
        {
            return _context.Contests.ToList();
        }

        public List<ContestParticipant> GetLeaderboard(int contestId)
        {
            return _context.ContestParticipants
                .Where(x => x.ContestId == contestId)
                .OrderByDescending(x => x.Score)
                .ToList();
        }
    }
}