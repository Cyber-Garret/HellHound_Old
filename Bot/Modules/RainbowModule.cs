﻿using Bot.Core;
using Bot.Models;
using Bot.Preconditions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Bot.Modules
{
	[Group("цвет"), RequireGuildOwner, Cooldown(5)]
	public class RainbowModule : ModuleBase<SocketCommandContext>
	{
		[Command("добавить")]
		public async Task AddColor(SocketRole role, string hex)
		{
			#region parseColor
			if (hex.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) ||
				hex.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
				hex = hex.Substring(2);

			if (hex.StartsWith("#", StringComparison.CurrentCultureIgnoreCase))
				hex = hex.Substring(1);
			#endregion

			bool parsedSuccessfully = uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out uint color);
			if (parsedSuccessfully)
			{
				var guild = GuildData.guild;
				var currentRole = guild.rainbowRoles.FirstOrDefault(r => r.RoleId == role.Id);

				if (currentRole.Colors.Contains(color))
				{
					await ReplyAsync("Цвет был добавлен ранее или ");
					return;
				}
				else
				{
					if (currentRole == null)
					{
						var newRole = new RainbowRole
						{
							RoleId = role.Id,
							Colors = new List<uint>()
						};
						newRole.Colors.Add(color);
						guild.rainbowRoles.Add(newRole);
					}
					else
						currentRole.Colors.Add(color);

					GuildData.SaveGuild();
					await ReplyAsync($"Для роли {role.Mention}, сохранен цвет #{color}.");
				}
			}
			else
				await ReplyAsync("Миша все хуйня, давай по новой.(Некоректный формат цвета)");
		}

		[Command("список")]
		public async Task ListColors()
		{
			var guild = GuildData.guild;
			if (guild.RoleId == 0 && guild.Colors.Count < 1) return;

			var role = Context.Guild.GetRole(guild.RoleId);

			var embed = new EmbedBuilder
			{
				Title = "Настройки цветовой роли",
				Footer = new EmbedFooterBuilder
				{
					Text = "Для проверки цвета используй - https://www.color-hex.com/"
				}
			};

			var text = $"Я меняю цвет каждую минуту у роли {role.Mention}\n В случайном порядке я использую один из этих цветов: ";

			foreach (var item in guild.Colors)
			{
				var color = UIntToColor(item);
				text += $"#{color.Name}, ";
			}

			embed.Description = text[0..^2];

			await ReplyAsync(embed: embed.Build());
		}

		[Command("очистить")]
		public async Task ClearColors()
		{
			var guild = GuildData.guild;
			guild.RoleId = 0;
			guild.Colors.Clear();

			GuildData.SaveGuild();
			await ReplyAsync("Цвета были удалены.");
		}

		[Command("роль")]
		public async Task AssociateRole(SocketRole role)
		{
			if (role.IsEveryone) return;
			GuildData.guild.RoleId = role.Id;
			GuildData.SaveGuild();

			await ReplyAsync("готово");
		}

		private System.Drawing.Color UIntToColor(uint color)
		{
			byte a = (byte)(color >> 24);
			byte r = (byte)(color >> 16);
			byte g = (byte)(color >> 8);
			byte b = (byte)(color >> 0);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}
	}
}
