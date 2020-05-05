using Bot.Core;
using Bot.Models;
using Bot.Preconditions;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Modules
{
	[RequireUserPermission(GuildPermission.Administrator), RequireContext(ContextType.Guild), Cooldown(5)]
	public class RainbowModule : ModuleBase<SocketCommandContext>
	{
		[Command("add color")]
		public async Task AddColor(SocketRole role = null, string hex = null)
		{
			if (role == null || string.IsNullOrWhiteSpace(hex))
			{
				await ReplyAsync("Не указана роль и-или цвет.\nФормат: !add color @Role #fcba03");
			}
			else
			{
				try
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
						//Role in list?
						var currentRole = guild.rainbowRoles.FirstOrDefault(r => r.RoleId == role.Id);

						//New role
						if (currentRole == null)
						{
							var newRole = new RainbowRole
							{
								RoleId = role.Id,
								Colors = new List<uint>()
							};
							newRole.Colors.Add(color);
							guild.rainbowRoles.Add(newRole);

							//Save data ad reply about this
							GuildData.SaveGuild();
							await ReplyAsync($":floppy_disk: Для роли **{role.Name}**, сохранен цвет #{hex}.");
						}
						//Role in list and contain color
						else if (currentRole.Colors.Contains(color))
						{
							await ReplyAsync($":no_entry_sign: Цвет {hex} для роли **{role.Name}** был добавлен ранее.");
						}
						//Role in list and not contain color?
						else
						{
							//Because role in list just add color, save data and reply about this.
							currentRole.Colors.Add(color);
							GuildData.SaveGuild();
							await ReplyAsync($":floppy_disk: Для роли **{role.Name}**, сохранен цвет #{hex}.");
						}
					}
					//Unsuccessful parsing color from string to uint
					else
						await ReplyAsync(":no_entry_sign: Миша все хуйня, давай по новой.(Некоректный формат цвета)");
				}
				catch (Exception ex)
				{
					await ReplyAsync(ex.Message);
				}
			}
		}

		[Command("list")]
		public async Task ListColors()
		{
			var guild = GuildData.guild;
			if (guild.rainbowRoles.Count > 0)
			{
				//Base embed with Title and simple footer
				var embed = new EmbedBuilder
				{
					Title = "Настройки цветов для ролей",
					Footer = new EmbedFooterBuilder
					{
						Text = "Для проверки цвета используй - https://www.color-hex.com/"
					}
				};

				foreach (var rainbowRole in guild.rainbowRoles)
				{
					var role = Context.Guild.GetRole(rainbowRole.RoleId);
					var embField = new EmbedFieldBuilder
					{
						Name = $"Настройки цветов для роли {role.Name}"
					};
					string text = string.Empty;
					foreach (var item in rainbowRole.Colors)
					{
						var color = UIntToColor(item);
						text += $"#{color.Name}, ";
					}

					embField.Value = text[0..^2];
					embed.AddField(embField);
				}
				await ReplyAsync(embed: embed.Build());
			}
			else
			{
				await ReplyAsync(":no_entry_sign: Не сохранено ни одной роли.");
			}
		}

		[Command("clear")]
		public async Task ClearColors(SocketRole role = null)
		{
			if (role == null)
			{
				await ReplyAsync(":no_entry_sign: Надо указать роль которую я удалю из списка.\nФормат: !clear @Role");
				return;
			}

			var guild = GuildData.guild;
			var roleForDelete = guild.rainbowRoles.FirstOrDefault(r => r.RoleId == role.Id);
			if (roleForDelete != null)
			{
				guild.rainbowRoles.Remove(roleForDelete);
				GuildData.SaveGuild();
				await ReplyAsync($":floppy_disk: Цвета и роль **{role.Name}** были удалены.");
			}
			else
			{
				await ReplyAsync($":no_entry_sign: Роль {role.Name} не используеться в функционале смены цветов.");
			}

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
