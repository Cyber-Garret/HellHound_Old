using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Core.QuartzJobs
{
	[DisallowConcurrentExecution]
	public class ColorChangeRoleJob : IJob
	{
		private readonly DiscordSocketClient discord;
		private readonly IConfiguration config;
		private readonly ILogger<ColorChangeRoleJob> logger;

		public ColorChangeRoleJob(IServiceProvider service, IConfiguration configuration, ILogger<ColorChangeRoleJob> logger)
		{
			discord = service.GetRequiredService<DiscordSocketClient>();
			config = configuration;
			this.logger = logger;
		}

		public Task Execute(IJobExecutionContext context)
		{
			var guild = GuildData.guild;
			//Roles have in list?
			if (guild.rainbowRoles.Count > 0)
			{
				foreach (var item in guild.rainbowRoles)
				{
					//Role have more 2 colors?
					if (item.Colors.Count > 1)
					{
						try
						{
							//Get guild ID
							var guildId = config.GetSection("Bot:Guild").Get<ulong>();
							//Get saved role by ID
							var role = discord.GetGuild(guildId).GetRole(item.RoleId);

							//Take random color
							var rnd = new Random();
							var color = rnd.Next(item.Colors.Count);
							var selectedColor = new Color(item.Colors[color]);

							//Wait 1 second  for avoid preemptive rate limit before we update role color.
							Thread.Sleep(1000);
							//Change color on role
							role.ModifyAsync(r => r.Color = selectedColor);

							logger.LogInformation($"On role {role.Name} changed color to {selectedColor}");
						}
						catch (Exception ex)
						{
							logger.LogError(ex, "Change color job");
						}
					}
				}
			}

			return Task.CompletedTask;
		}
	}
}
