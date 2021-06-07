using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.DataAccess
{
    public class TeamDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = C:\Users\lucas\Desktop\exam-dnp-300709\WebApplication\Team.db");
        }
    }
}