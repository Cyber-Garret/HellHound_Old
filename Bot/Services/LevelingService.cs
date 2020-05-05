using Bot.Core;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Services
{
	public class LevelingService
	{
		private readonly ILogger<LevelingService> logger;

		private readonly double MessageXPCooldown;
		private readonly uint Exp;
		public LevelingService(IServiceProvider service, ILogger<LevelingService> logger, IConfiguration configuration)
		{
			this.logger = logger;
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
				var userAccount = UserAccounts.GetUser(user.Id);

				var now = DateTime.Now;

				if (now < userAccount.LastXPMessage.AddSeconds(MessageXPCooldown))
				{
					userAccount.SendedMsg++;
					UserAccounts.SaveAccount(user.Id);
				}
				else
				{
					userAccount.LastXPMessage = now;

					//save old level value
					uint oldLevel = userAccount.LevelNumber;
					//add exp and msg count
					userAccount.XP += Exp;
					userAccount.SendedMsg++;

					UserAccounts.SaveAccount(user.Id);

					//get new level value
					//uint newLevel = userAccount.LevelNumber;
					//User level up?
					//if (oldLevel != newLevel)
					//{

					//	if (config.WelcomeChannel != 0)
					//	{
					//		await _discord.GetGuild(config.Id).GetTextChannel(config.WelcomeChannel)
					//		   .SendMessageAsync($"Поздравляю {user.Nickname ?? user.Username}, он только что поднялся до уровня **{newLevel}**!");
					//		return;
					//	}
					//}
					//else return;
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
			var userAccount = UserAccounts.GetUser(user.Id);
			//Increment updated messages value
			userAccount.UpdatedMsg++;
			//Save account
			UserAccounts.SaveAccount(user.Id);
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
					var userAccount = UserAccounts.GetUser(audit.User.Id);
					//Increment updated messages value
					userAccount.DeletedMsg++;
					//Save account
					UserAccounts.SaveAccount(audit.User.Id);
				}
			}
		}
	}
}
