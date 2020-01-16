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
		private readonly DiscordSocketClient _discord;
		private readonly ILogger<GuildSelfRoleService> _logger;
		public GuildSelfRoleService(IServiceProvider service)
		{
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_logger = service.GetRequiredService<ILogger<GuildSelfRoleService>>();
		}
		public async Task SelfRoleReactionAdded(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();

				using var Db = new NeiraLinkContext();

				var guild = Db.Guilds.FirstOrDefault(g => g.SelfRoleMessageId == msg.Id);

				if (guild == null) return;

				foreach (var item in Db.GuildSelfRoles.Where(g => g.GuildID == guild.Id))
				{
					var emote = await _discord.GetGuild(item.GuildID).GetEmoteAsync(item.EmoteID);
					if (reaction.Emote.Equals(emote))
					{
						var user = _discord.GetGuild(item.GuildID).GetUser(reaction.UserId);
						var role = _discord.GetGuild(item.GuildID).GetRole(item.RoleID);
						await user.AddRoleAsync(role);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SelfRoleReactionAdded");
			}
		}
		public async Task SelfRoleReactionRemoved(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();

				using var Db = new NeiraLinkContext();

				var guild = Db.Guilds.FirstOrDefault(g => g.SelfRoleMessageId == msg.Id);

				if (guild == null) return;

				foreach (var item in Db.GuildSelfRoles.Where(g => g.GuildID == guild.Id))
				{
					var emote = await _discord.GetGuild(item.GuildID).GetEmoteAsync(item.EmoteID);
					if (reaction.Emote.Equals(emote))
					{
						var user = _discord.GetGuild(item.GuildID).GetUser(reaction.UserId);
						var role = _discord.GetGuild(item.GuildID).GetRole(item.RoleID);
						await user.RemoveRoleAsync(role);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SelfRoleReactionRemoved");
			}
		}
	}
}
