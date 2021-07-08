using Bot.Domain.Models.Repository;

using DSharpPlusNextGen;
using DSharpPlusNextGen.Entities;
using DSharpPlusNextGen.SlashCommands;

using System;
using System.Threading.Tasks;

namespace Bot.Application.SlashModules.Economy
{
	public class Statistics : SlashCommandModule
	{
		[SlashCommand("stat", "Отображение твоей статистики на сервере (Уровень, опыт, репутация)")]
		public static async Task Command(InteractionContext context, [Option("discord_user", "Cтраж которого ты хочешь проверить (Если никого не указать по умолчанию отобразит твою статистику).")] DiscordUser? mentionedUser = null)
		{
			var target = mentionedUser ?? context.User;

			var user = new User();
			var requiredXp = Math.Pow(user.LevelNumber + 1, 2) * 50;

			var embed = new DiscordEmbedBuilder
			{
				Color = DiscordColor.Red
			};

			embed.WithAuthor($"Статистика {target.Username} среди Адских Гончих",
				iconUrl: target.AvatarUrl ?? target.DefaultAvatarUrl);

			embed.WithDescription($"Дата регистрации аккаунта: {target.CreationTimestamp.Date.ToShortDateString()}");

			embed.AddField("Ур.", $"{user.LevelNumber}", inline: true);

			embed.AddField("Опыт", $"{user.Xp}/{requiredXp}", inline: true);

			embed.AddField("Репутация", $"{user.Reputation}", inline: true);

			embed.AddField("Текстовые сообщения:",
				$"- Написано: **{user.SentMessages}**\n- Исправлено: **{user.UpdatedMessages}**\n",
				inline: true);

			await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()));
		}
	}
}
