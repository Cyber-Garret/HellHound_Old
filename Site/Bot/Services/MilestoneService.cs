using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Site.Bot.Services
{
	public class MilestoneService
	{
		private readonly ILogger _logger;
		private readonly DiscordSocketClient _discord;
		private readonly EmoteService _emote;

		private const byte Second = 2;
		private const byte Three = 3;
		private const byte Four = 4;
		private const byte Five = 5;
		private const byte Six = 6;

		public MilestoneService(IServiceProvider service)
		{
			_logger = service.GetRequiredService<ILogger<MilestoneService>>();
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_emote = service.GetRequiredService<EmoteService>();
		}

		public async Task MilestoneReactionAdded(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();

				using var Db = new NeiraLinkContext();
				//get milestone
				var milestone = await Db.ActiveMilestones
					.Include(mu => mu.MilestoneUsers)
					.Include(r => r.Milestone)
					.Where(r => r.MessageId == cache.Id)
					.FirstOrDefaultAsync();

				if (milestone == null) return;

				if (reaction.Emote.Equals(_emote.Raid))
				{
					//check reaction
					var UserExist = milestone.MilestoneUsers.Any(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId);

					if (reaction.UserId != milestone.Leader && !UserExist && milestone.MilestoneUsers.Count + 1 < milestone.Milestone.MaxSpace)
					{
						Db.MilestoneUsers.Add(new MilestoneUser
						{
							ActiveMilestoneMessageId = milestone.MessageId,
							UserId = reaction.UserId
						});
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
					else
					{
						var user = _discord.GetUser(reaction.UserId);
						await msg.RemoveReactionAsync(_emote.Raid, user);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactSecond))
				{
					if (!milestone.MilestoneUsers.Any(u => u.Place == 2))
					{
						Db.MilestoneUsers.Add(new MilestoneUser
						{
							ActiveMilestoneMessageId = milestone.MessageId,
							UserId = reaction.UserId,
							Place = 2
						});
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactThird))
				{
					if (!milestone.MilestoneUsers.Any(u => u.Place == 3))
					{
						Db.MilestoneUsers.Add(new MilestoneUser
						{
							ActiveMilestoneMessageId = milestone.MessageId,
							UserId = reaction.UserId,
							Place = 3
						});
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactFourth))
				{
					if (!milestone.MilestoneUsers.Any(u => u.Place == 4))
					{
						Db.MilestoneUsers.Add(new MilestoneUser
						{
							ActiveMilestoneMessageId = milestone.MessageId,
							UserId = reaction.UserId,
							Place = 4
						});
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactFifth))
				{
					if (!milestone.MilestoneUsers.Any(u => u.Place == 5))
					{
						Db.MilestoneUsers.Add(new MilestoneUser
						{
							ActiveMilestoneMessageId = milestone.MessageId,
							UserId = reaction.UserId,
							Place = 5
						});
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactSixth))
				{
					if (!milestone.MilestoneUsers.Any(u => u.Place == 6))
					{
						Db.MilestoneUsers.Add(new MilestoneUser
						{
							ActiveMilestoneMessageId = milestone.MessageId,
							UserId = reaction.UserId,
							Place = 6
						});
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Reaction Added in Milestone");
			}
		}

		public async Task MilestoneReactionRemoved(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();

				using var Db = new NeiraLinkContext();
				//get milestone
				var milestone = await Db.ActiveMilestones
					.Include(r => r.Milestone)
					.Include(mu => mu.MilestoneUsers)
					.Where(r => r.MessageId == cache.Id)
					.FirstOrDefaultAsync();

				if (milestone == null) return;

				if (reaction.Emote.Equals(_emote.Raid))
				{
					//check reaction
					var UserExist = milestone.MilestoneUsers.Any(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId);

					if (reaction.UserId != milestone.Leader && UserExist)
					{
						var milestoneUser = Db.MilestoneUsers.First(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId);

						Db.Remove(milestoneUser);
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
					else
					{
						var user = _discord.GetUser(reaction.UserId);
						await msg.RemoveReactionAsync(_emote.Raid, user);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactSecond))
				{
					if (milestone.MilestoneUsers.Any(m => m.Place == Second && m.UserId == reaction.UserId && m.ActiveMilestoneMessageId == reaction.MessageId))
					{
						var user = Db.MilestoneUsers.First(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId && u.Place == Second);
						Db.Remove(user);
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactThird))
				{
					if (milestone.MilestoneUsers.Any(m => m.Place == Three && m.UserId == reaction.UserId && m.ActiveMilestoneMessageId == reaction.MessageId))
					{
						var user = Db.MilestoneUsers.First(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId && u.Place == Three);
						Db.Remove(user);
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactFourth))
				{
					if (milestone.MilestoneUsers.Any(m => m.Place == Four && m.UserId == reaction.UserId && m.ActiveMilestoneMessageId == reaction.MessageId))
					{
						var user = Db.MilestoneUsers.First(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId && u.Place == Four);
						Db.Remove(user);
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactFifth))
				{
					if (milestone.MilestoneUsers.Any(m => m.Place == Five && m.UserId == reaction.UserId && m.ActiveMilestoneMessageId == reaction.MessageId))
					{
						var user = Db.MilestoneUsers.First(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId && u.Place == Five);
						Db.Remove(user);
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}
				else if (reaction.Emote.Equals(_emote.ReactSixth))
				{
					if (milestone.MilestoneUsers.Any(m => m.Place == Six && m.UserId == reaction.UserId && m.ActiveMilestoneMessageId == reaction.MessageId))
					{
						var user = Db.MilestoneUsers.First(u => u.UserId == reaction.UserId && u.ActiveMilestoneMessageId == milestone.MessageId && u.Place == Six);
						Db.Remove(user);
						await Db.SaveChangesAsync();
						HandleReaction(msg, milestone);
					}
				}

			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Reaction Removed in Milestone");
			}
		}

		private async void HandleReaction(IUserMessage message, ActiveMilestone activeMilestone)
		{
			var newEmbed = EmbedsHelper.MilestoneRebuild(_discord, activeMilestone, _emote.Raid);
			if (newEmbed.Length != 0)
				await message.ModifyAsync(m => m.Embed = newEmbed);
			if (activeMilestone.Milestone.MaxSpace == activeMilestone.MilestoneUsers.Count + 1 && activeMilestone.DateExpire < DateTime.Now.AddMinutes(15))
			{
				await message.RemoveAllReactionsAsync();
				await message.ModifyAsync(c => c.Embed = EmbedsHelper.MilestoneEnd(_discord, activeMilestone));

				await RaidNotificationAsync(activeMilestone, RemindType.FullCount);
			}
		}

		private async Task UpdateBotStatAsync()
		{
			using var Db = new NeiraLinkContext();
			//Update Bot stat for website.
			var stats = Db.BotInfos.FirstOrDefault();
			stats.Milestones++;
			stats.Servers = _discord.Guilds.Count;
			stats.Users = _discord.Guilds.Sum(u => u.Users.Count);
			Db.BotInfos.Update(stats);
			await Db.SaveChangesAsync();
		}

		public async Task RegisterMilestoneAsync(ulong msgId, SocketCommandContext context, DateTime dateExpire, MilestoneType type, byte raidInfoId, string userMemo)
		{
			try
			{
				using var Db = new NeiraLinkContext();
				ActiveMilestone newMilestone = new ActiveMilestone
				{
					MessageId = msgId,
					GuildId = context.Guild.Id,
					MilestoneId = raidInfoId,
					Memo = userMemo,
					DateExpire = dateExpire,
					Leader = context.User.Id,
					MilestoneType = type
				};

				Db.ActiveMilestones.Add(newMilestone);
				await Db.SaveChangesAsync();

				_ = Task.Run(async () => await UpdateBotStatAsync());
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Register Milestone Method");
			}

		}

		public async Task RaidNotificationAsync(ActiveMilestone milestone, RemindType type)
		{
			try
			{
				var Guild = _discord.GetGuild(milestone.GuildId);
				var Leader = _discord.GetUser(milestone.Leader);
				var LeaderDM = await Leader.GetOrCreateDMChannelAsync();

				Embed BakedEmbed = null;
				if (type == RemindType.FullCount)
					BakedEmbed = EmbedsHelper.MilestoneRemindByFullCount(_discord, milestone, Guild);
				else
					BakedEmbed = EmbedsHelper.MilestoneRemindByTimer(_discord, milestone, Guild);


				await LeaderDM.SendMessageAsync(embed: BakedEmbed);

				foreach (var user in milestone.MilestoneUsers)
				{
					try
					{
						if (user.UserId == milestone.Leader) continue;
						var LoadedUser = _discord.GetUser(user.UserId);

						var DM = await LoadedUser.GetOrCreateDMChannelAsync();
						await DM.SendMessageAsync(embed: BakedEmbed);
					}
					catch (Exception ex)
					{
						_logger.LogWarning(ex, "RaidNotification in DM of user");
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "RaidNotification Global");
			}

		}

		public enum RemindType
		{
			FullCount,
			ByTimer
		}
	}
}
