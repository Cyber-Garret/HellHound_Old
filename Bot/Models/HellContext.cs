using Bot.Models.Db;

using Microsoft.EntityFrameworkCore;

namespace Bot.Models
{
	public class HellContext : DbContext
	{
		#region Milestones
		public DbSet<Milestone> Milestones { get; set; }
		public DbSet<ActiveMilestone> ActiveMilestones { get; set; }
		public DbSet<MilestoneUser> MilestoneUsers { get; set; }
		#endregion
		#region Discord
		public DbSet<Config> Configs { get; set; }
		public DbSet<GuildSelfRole> GuildSelfRoles { get; set; }
		#endregion

		public HellContext(DbContextOptions<HellContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
