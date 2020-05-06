using Bot.Core;
using Bot.Models;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Modules
{
	[RequireUserPermission(GuildPermission.Administrator)]
	public class AdminModule : InteractiveBase
	{
		[Command("settings")]
		public async Task GetGuildSettings()
		{
			var role = Context.Guild.GetRole(GuildData.guild.ModRole);
			var embed = new EmbedBuilder
			{
				Author = new EmbedAuthorBuilder
				{
					Name = "Мои настройки",
					IconUrl = Context.Client.CurrentUser.GetAvatarUrl()
				},
				Color = Color.LightOrange,
				Description =
				$"Роль модератора: **{role.Name ?? "Нет"}**" +
				$"Радужных ролей: {GuildData.guild.rainbowRoles.Count}"
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

				await ReplyAsync($"remove");
			}
			else
			{
				GuildData.guild.ModRole = role.Id;
				GuildData.SaveGuild();
				await ReplyAsync($"saved");
			}
		}
	}

}

