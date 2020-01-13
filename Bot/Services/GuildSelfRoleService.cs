using Bot.Models;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Services
{
	public class GuildSelfRoleService
	{
		private readonly DiscordSocketClient discord;
		private readonly ILogger<GuildSelfRoleService> logger;
		private readonly HellContext Db;
		private readonly GuildSettingsService settingsService;
		public GuildSelfRoleService(IServiceProvider service)
		{
			discord = service.GetRequiredService<DiscordSocketClient>();
			logger = service.GetRequiredService<ILogger<GuildSelfRoleService>>();
			Db = service.GetRequiredService<HellContext>();
			settingsService = service.GetRequiredService<GuildSettingsService>();
		}
		public async Task SelfRoleReactionAdded(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();

				if (settingsService.config.SelfRoleMessageId != msg.Id) return;

				foreach (var item in Db.GuildSelfRoles)
				{
					var emote = await discord.Guilds.First().GetEmoteAsync(item.EmoteID);
					if (reaction.Emote.Equals(emote))
					{
						var user = discord.Guilds.First().GetUser(reaction.UserId);
						var role = discord.Guilds.First().GetRole(item.RoleID);
						await user.AddRoleAsync(role);
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "SelfRoleReactionAdded");
			}
		}
		public async Task SelfRoleReactionRemoved(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();

				if (settingsService.config.SelfRoleMessageId != msg.Id) return;

				foreach (var item in Db.GuildSelfRoles)
				{
					var emote = await discord.Guilds.First().GetEmoteAsync(item.EmoteID);
					if (reaction.Emote.Equals(emote))
					{
						var user = discord.Guilds.First().GetUser(reaction.UserId);
						var role = discord.Guilds.First().GetRole(item.RoleID);
						await user.RemoveRoleAsync(role);
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "SelfRoleReactionRemoved");
			}
		}
	}
}
