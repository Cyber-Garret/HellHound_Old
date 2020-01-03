using Microsoft.EntityFrameworkCore;

using Site.Database.Models;

namespace Site.Database
{
    public class HellContext : DbContext
    {
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Clan_Member> Clan_Members { get; set; }
        public HellContext(DbContextOptions<HellContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
