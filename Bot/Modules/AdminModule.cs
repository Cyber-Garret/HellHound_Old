using Bot.Core;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Bot.Modules
{
	[RequireUserPermission(GuildPermission.Administrator)]
	public class AdminModule : InteractiveBase
	{
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
	}

}

