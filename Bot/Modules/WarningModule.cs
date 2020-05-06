using Bot.Core;
using Bot.Models;
using Bot.Properties;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Modules
{
	public class WarningModule : InteractiveBase
	{
		[Command("предупреждение")]
		[RequireContext(ContextType.Guild)]
		public async Task WarnUser(SocketGuildUser user = null, string reason = null)
		{
			var initiator = (SocketGuildUser)Context.User;
			var role = Context.Guild.GetRole(GuildData.guild.ModRole);

			if (initiator.Roles.Contains(role) && role != null)
			{
				if (user == null || string.IsNullOrWhiteSpace(reason))
				{
					await ReplyAsync(":no_entry_sign: Нужно указать кто провинился и причину.\nФормат: **!предупреждение @User причина**");
				}
				else
				{
					var target = UserAccounts.GetUser(user);
					target.NumberOfWarnings++;
					target.Warnings.Add(new Warning
					{
						Reason = reason
					});

					await ReplyAsync($":floppy_disk: Пользователь {user.Nickname ?? user.Username} получил предупреждение по причине:\n**{reason}**");

					UserAccounts.SaveAccount(user);
				}
			}
			else
			{
				await ReplyAsync(Resources.Disallow);
			}
		}

		[Command("предупреждения")]
		public async Task GetWarnings(SocketGuildUser mentionedUser = null)
		{
			var target = mentionedUser ?? (SocketGuildUser)Context.User;

			var user = UserAccounts.GetUser(target);
			if (user.Warnings.Count > 0)
			{
				var message = new PaginatedMessage
				{
					Author = new EmbedAuthorBuilder
					{
						Name = $"Предупреждения {target.Nickname ?? target.Username}",
						IconUrl = target.GetAvatarUrl() ?? target.GetDefaultAvatarUrl()
					},
					Color = Color.DarkOrange,
					Options = new PaginatedAppearanceOptions
					{
						DisplayInformationIcon = false,
						JumpDisplayOptions = JumpDisplayOptions.Never,
						FooterFormat = "Предупреждение {0} из {1}",
						Timeout = TimeSpan.FromMinutes(1)
					}
				};

				var warns = new List<PaginatedMessage.Page>();
				foreach (var item in user.Warnings)
				{
					warns.Add(new PaginatedMessage.Page
					{
						Description = $"**Предупреждение было выдано:** {item.CreateDate:dd.MM.yyyy HH:mm}\n**Причина:** {item.Reason}"
					});
				}
				message.Pages = warns;

				var reactionList = new ReactionList
				{
					Forward = true,
					Backward = true,
					Trash = true
				};

				await PagedReplyAsync(message, reactionList);
			}
		}
	}
}
