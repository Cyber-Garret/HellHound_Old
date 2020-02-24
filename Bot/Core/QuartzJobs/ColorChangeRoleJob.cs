using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

using System;
using System.Threading.Tasks;

namespace Bot.Core.QuartzJobs
{
	[DisallowConcurrentExecution]
	public class ColorChangeRoleJob : IJob
	{
		private readonly DiscordSocketClient discord;
		private readonly IConfiguration config;

		public ColorChangeRoleJob(IServiceProvider service, IConfiguration configuration)
		{
			discord = service.GetRequiredService<DiscordSocketClient>();
			config = configuration;
		}

		public Task Execute(IJobExecutionContext context)
		{
			var guild = GuildData.guild;
			if (guild.RoleId != 0 && guild.Colors.Count > 1)
			{
				var guildId = config.GetSection("Bot:Guild").Get<ulong>();

				var role = discord.GetGuild(guildId).GetRole(guild.RoleId);
				var colors = guild.Colors;

				var rnd = new Random();
				var color = rnd.Next(colors.Count);
				var selectedColor = new Color(colors[color]);

				role.ModifyAsync(r => r.Color = selectedColor);
			}

			return Task.CompletedTask;
		}
	}
}
