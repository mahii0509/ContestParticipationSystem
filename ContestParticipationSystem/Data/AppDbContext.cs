using Microsoft.EntityFrameworkCore;
using ContestParticipationSystem.Models;

namespace ContestParticipationSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<ContestParticipant> ContestParticipants { get; set; }

        public DbSet<Answer> Answers { get; set; }
    }
}