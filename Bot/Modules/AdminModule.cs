using Bot.Core;
using Bot.Properties;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Modules
{
	[RequireUserPermission(GuildPermission.Administrator)]
	public class AdminModule : ModuleBase<SocketCommandContext>
	{
		private readonly ILogger<AdminModule> _logger;
		private readonly EventId _sendMessagEvent;

		public AdminModule(ILogger<AdminModule> logger)
		{
			_logger = logger;
			_sendMessagEvent = new EventId(1, "SendMessage");
		}

		[Command("settings")]
		public async Task GetGuildSettings()
		{
			var roleName = "Нет";

			var role = Context.Guild.GetRole(GuildData.guild.ModRole);
			if (role != null)
				roleName = role.Name;

			var embed = new EmbedBuilder
			{
				Author = new EmbedAuthorBuilder
				{
					Name = "Мои настройки",
					IconUrl = Context.Client.CurrentUser.GetAvatarUrl()
				},
				Color = Color.LightOrange,
				Description =
				$"- Роль модератора: **{roleName}**\n" +
				$"- Радужных ролей: **{GuildData.guild.rainbowRoles.Count}**\n"
			};

			await ReplyAsync(embed: embed.Build());
		}

		[Command("mod")]
		public async Task AddModeRole(SocketRole role = null)
		{
			if (role == null)
			{
				GuildData.guild.ModRole = 0;
				GuildData.SaveGuild();

				await ReplyAsync($":no_entry_sign: Роль исключена из списка модераторов.");
			}
			else
			{
				GuildData.guild.ModRole = role.Id;
				GuildData.SaveGuild();
				await ReplyAsync($":floppy_disk: Роль сохранена как модераторская.");
			}
		}

		[Command("notif")]
		public async Task SetNotifChannel(ITextChannel channel)
		{

			if (channel == null)
			{
				GuildData.guild.NotificationChannel = 0;
				await ReplyAsync(Resources.NotifOff);
			}
			else
			{
				GuildData.guild.NotificationChannel = channel.Id;
				await ReplyAsync(string.Format(Resources.NotifOn, channel.Mention));
			}
			GuildData.SaveGuild();
		}

		[Command("mailing"), Alias("рассылка")]
		public async Task SendMessage(IRole role, [Remainder] string message)
		{
			var workMessage = await Context.Channel.SendMessageAsync(
				$"Приступаю к рассылке сообщений. Всем пользователям с ролью **{role.Name}**");

			var successCount = 0;
			var failCount = new List<SocketGuildUser>();
			var embed = MailingEmbed(message);

			//Update user cache
			await Context.Guild.DownloadUsersAsync();

			var users = Context.Guild.Users;
			_logger.LogInformation($"{Context.Guild.Name} loaded: {users.Count} users.");

			foreach (var user in users)
			{
				if (!user.Roles.Contains(role) && role.Name != "everyone") continue;

				try
				{
					var dm = await user.CreateDMChannelAsync();

					await dm.SendMessageAsync(embed: embed);
					successCount++;
				}
				catch (Exception ex)
				{
					failCount.Add(user);

					_logger.LogWarning(_sendMessagEvent, "Can't send message for {user} by reason {reason} ",
						(user.Nickname ?? user.Username + "#" + user.Discriminator), ex.Message);

					_logger.LogWarning(ex, "SendMessage command");
				}
			}
			await workMessage.ModifyAsync(m => m.Content =
				$"Готово. Я разослал сообщением всем у кого есть роль **{role.Name}**.\n" +
				$"- Всего получателей: **{successCount + failCount.Count}**\n" +
				$"- Успешно доставлено: **{successCount}**\n" +
				$"- Не удалось отправить: **{failCount}**");
		}

		private Embed MailingEmbed(string message)
		{
			return new EmbedBuilder()
				.WithTitle(
					$":mailbox_with_mail: Вам сообщение от {Context.User.Username} с сервера **`{Context.Guild.Name}`**")
				.WithColor(Color.LightOrange)
				.WithThumbnailUrl(Context.Guild.IconUrl)
				.WithDescription(message)
				.WithCurrentTimestamp()
				.Build();
		}
	}
}

