using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Bot.Core;
using Bot.Models;
using Discord.Addons.Interactive;
using System.Linq;

namespace Bot.Modules
{
	public class WarningModule : InteractiveBase
	{
		[Command("warn")]
		[RequireContext(ContextType.Guild)]
		public async Task WarnUser(SocketGuildUser user = null, string reason = null)
		{
			var initiator = (SocketGuildUser)Context.User;
			var role = Context.Guild.GetRole(GuildData.guild.ModRole);

			if (initiator.Roles.Contains(role) && role != null)
			{
				if (user == null || string.IsNullOrWhiteSpace(reason))
				{
					await ReplyAsync(":no_entry_sign: Нужно указать кто провинился и причину.\nФормат: **!warn @User причина**");
				}
				else
				{
					var target = UserAccounts.GetUser(user.Id);
					target.NumberOfWarnings++;
					target.Warnings.Add(new Warning
					{
						Reason = reason
					});
					UserAccounts.SaveAccount(user.Id);

					await ReplyAsync($":floppy_disk: Пользователь {user.Nickname ?? user.Username} получил предупреждение по причине:\n**{reason}**");
				}
			}
		}

		[Command("warns")]
		public async Task GetWarnings(SocketGuildUser mentionedUser = null)
		{
			var target = mentionedUser ?? (SocketGuildUser)Context.User;

			var user = UserAccounts.GetUser(target.Id);
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
						FooterFormat = "Страница {0}/{1}",
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
