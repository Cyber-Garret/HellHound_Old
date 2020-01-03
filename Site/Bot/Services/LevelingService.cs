using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Site.Bot.Services
{
	public class LevelingService
	{
		private readonly ILogger _logger;
		private readonly DiscordSocketClient _discord;
		public LevelingService(IServiceProvider service)
		{
			_logger = service.GetRequiredService<ILogger<LevelingService>>();
			_discord = service.GetRequiredService<DiscordSocketClient>();
		}

		/// <summary>
		/// Event for user level up in guild
		/// </summary>
		internal async Task Level(SocketGuildUser user)
		{
			try
			{
				using (var Db = new NeiraLinkContext())
				{
					var config = await DatabaseHelper.GetGuildAccountAsync(user.Guild.Id);
					if (!config.Economy) return;

					//Load or create user guild account
					var userAccount = await DatabaseHelper.GetGuildUserAccountAsync(user);

					DateTime now = DateTime.UtcNow;

					if (now < userAccount.LastXPMessage.AddSeconds(GlobalVariables.MessageXPCooldown))
						return;

					userAccount.LastXPMessage = now;

					//save old level value
					uint oldLevel = userAccount.LevelNumber;
					userAccount.XP += 13;

					await DatabaseHelper.SaveGuildUserAccountAsync(userAccount);

					//get new level value
					uint newLevel = userAccount.LevelNumber;
					//User level up?
					if (oldLevel != newLevel)
					{

						if (config.WelcomeChannel != 0)
						{
							await _discord.GetGuild(config.Id).GetTextChannel(config.WelcomeChannel)
							   .SendMessageAsync($"Бип! Поздравляю стража {user.Nickname ?? user.Username}, он только что поднялся до уровня **{newLevel}**!");
							return;
						}
					}
					else return;
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "levelUpInGuild");
			}
		}

		/// <summary>
		/// Event for user global level up
		/// </summary>
		internal async Task GlobalLevel(SocketGuildUser user)
		{
			using (var Db = new NeiraLinkContext())
			{
				UserAccount userAccount = null;
				//Check if user exist
				if (Db.UserAccounts.Any(u => u.Id == user.Id))
					userAccount = await Db.UserAccounts.SingleAsync(u => u.Id == user.Id);
				else
				{
					userAccount = new UserAccount
					{
						Id = user.Id
					};
					Db.UserAccounts.Add(userAccount);
				}

				DateTime now = DateTime.UtcNow;

				if (now < userAccount.LastXPMessage.AddSeconds(GlobalVariables.MessageXPCooldown))
					return;

				userAccount.LastXPMessage = now;
				//Save level old value
				uint oldLevel = userAccount.LevelNumber;
				userAccount.XP += 7;

				Db.UserAccounts.Update(userAccount);
				await Db.SaveChangesAsync();

				//Get user new level value
				uint newLevel = userAccount.LevelNumber;
				//User level up?
				if (oldLevel != newLevel)
				{
					await CheckEngramRewards(user);
				}
				return;
			}
		}

		/// <summary>
		/// Event for user message reward
		/// </summary>
		internal async Task MessageRewards(SocketGuildUser user, SocketMessage msg)
		{
			if (msg == null) return;
			//Check if user not spam in Neira DM channel.
			if (msg.Channel == msg.Author.GetOrCreateDMChannelAsync()) return;
			//Ignore all bots
			if (msg.Author.IsBot) return;

			using (var Db = new NeiraLinkContext())
			{
				UserAccount userAccount = null;
				//Check if user exist
				if (Db.UserAccounts.Any(u => u.Id == user.Id))
					userAccount = await Db.UserAccounts.SingleAsync(u => u.Id == user.Id);
				else
				{
					userAccount = new UserAccount
					{
						Id = user.Id
					};
					Db.UserAccounts.Add(userAccount);
				}

				DateTime now = DateTime.UtcNow;

				if (now < userAccount.LastMessage.AddSeconds(GlobalVariables.MessageRewardCooldown) || msg.Content.Length < GlobalVariables.MessageRewardMinLenght)
					return;

				// Generate a randomized reward in the configured boundries
				userAccount.Glimmer += (ulong)GlobalVariables.GetRandom.Next(1, 5);
				userAccount.LastMessage = now;

				Db.UserAccounts.Update(userAccount);
				await Db.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Check if user able to receive new engram based on level
		/// </summary>
		private async Task CheckEngramRewards(SocketGuildUser user)
		{
			using var Db = new NeiraLinkContext();
			UserAccount userAccount = null;
			//Check if user exist
			if (Db.UserAccounts.Any(u => u.Id == user.Id))
				userAccount = await Db.UserAccounts.SingleAsync(u => u.Id == user.Id);
			else
			{
				userAccount = new UserAccount
				{
					Id = user.Id
				};
				Db.UserAccounts.Add(userAccount);
			}
			int level = (int)userAccount.LevelNumber;

			int uc = level % 5;
			int rare = level % 10;
			int legendary = level % 15;
			int exotic = level % 20;

			if (exotic == 0)
				userAccount.ExoticEngrams += 1;
			else if (legendary == 0)
				userAccount.LegendaryEngrams += 1;
			else if (rare == 0)
				userAccount.RareEngrams += 1;
			else if (uc == 0)
				userAccount.UncommonEngrams += 1;
			else
				userAccount.CommonEngrams += 1;

			Db.UserAccounts.Update(userAccount);
			await Db.SaveChangesAsync();
		}
	}
}
