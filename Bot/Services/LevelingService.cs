using Bot.Core;
using Bot.Properties;
using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Services
{
	public class LevelingService
	{
		private readonly ILogger<LevelingService> logger;
		private readonly DiscordSocketClient discord;

		private readonly double MessageXPCooldown;
		private readonly uint Exp;
		public LevelingService(IServiceProvider service, ILogger<LevelingService> logger, IConfiguration configuration)
		{
			this.logger = logger;
			discord = service.GetRequiredService<DiscordSocketClient>();
			MessageXPCooldown = configuration.GetSection("Economy:MessageXPCooldown").Get<double>();
			Exp = configuration.GetSection("Economy:MessageExp").Get<uint>();
		}
		/// <summary>
		/// Event for user level up in guild
		/// </summary>
		internal void Level(IUser user)
		{
			try
			{
				//Load or create user account
				var userAccount = UserAccounts.GetUser(user);

				var now = DateTime.Now;

				if (now < userAccount.LastXPMessage.AddSeconds(MessageXPCooldown))
				{
					userAccount.SendedMsg++;
					UserAccounts.SaveAccount(user);
				}
				else
				{
					userAccount.LastXPMessage = now;

					//save old level value
					uint oldLevel = userAccount.LevelNumber;
					//add exp and msg count
					userAccount.XP += Exp;
					userAccount.SendedMsg++;

					UserAccounts.SaveAccount(user);

					//get new level value
					uint newLevel = userAccount.LevelNumber;
					//User level up ?
					if (oldLevel != newLevel)
					{

						if (GuildData.guild.NotificationChannel != 0)
						{
							var message = string.Format(Resources.LvlUp, user.Username, newLevel);

							discord.Guilds.First().GetTextChannel(GuildData.guild.NotificationChannel).SendMessageAsync(message);
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "levelUpInGuild");
			}
		}

		internal void UpdMsgCount(IUser user)
		{
			//Get acc
			var userAccount = UserAccounts.GetUser(user);
			//Increment updated messages value
			userAccount.UpdatedMsg++;
			//Save account
			UserAccounts.SaveAccount(user);
		}
		internal async Task DelMsgCountAsync(ISocketMessageChannel channel)
		{
			if (channel is ITextChannel textChannel)
			{
				var log = await textChannel.Guild.GetAuditLogsAsync(limit: 1);
				var audit = log.ToList().First();

				//Ignore bots
				if (audit.User.IsBot) return;

				if (audit.Action == ActionType.MessageDeleted)
				{
					//Get acc
					var userAccount = UserAccounts.GetUser(audit.User);
					//Increment updated messages value
					userAccount.DeletedMsg++;
					//Save account
					UserAccounts.SaveAccount(audit.User);
				}
			}
		}
	}
}
