using Bot.Core;
using Bot.Preconditions;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Bot.Modules
{
	public class EconomyModule : ModuleBase<SocketCommandContext>
	{
		[Command("стата")]
		[Summary("Отображение твоей статистике на сервере (Уровень, опыт, репутация)")]
		[Remarks("!стата <страж которого ты хочешь проверить (Если никого не указать по умолчанию отобразит твою стату)> Пример: !стата @Cyber_Garret")]
		[Cooldown(10)]
		public async Task Stats(SocketUser mentionedUser = null)
		{
			SocketGuildUser target = (SocketGuildUser)mentionedUser ?? (SocketGuildUser)Context.User;

			var user = UserAccounts.GetUser(target);
			var requiredXp = (Math.Pow(user.LevelNumber + 1, 2) * 50);

			var auth = new EmbedAuthorBuilder()
			{
				Name = $"Статистика {target.Nickname ?? target.Username} среди Адских Гончих",
				IconUrl = target.GetAvatarUrl() ?? target.GetDefaultAvatarUrl(),
			};

			var embed = new EmbedBuilder()
			{
				Author = auth,
				Color = Color.DarkRed,
				Description = $"Дата создания аккаунта: {target.CreatedAt.Date.ToShortDateString()}\nДата присоединения к серверу: {target.JoinedAt.Value.Date.ToShortDateString() ?? "Неизвестно"}"
			};

			embed.AddField("Ур.", user.LevelNumber, true);
			embed.AddField("Опыт", $"{user.XP}/{requiredXp}", true);
			embed.AddField("Репутация", user.Reputation, true);
			embed.AddField("Косяки", user.NumberOfWarnings, true);
			embed.AddField("Текстовые сообщения:",
				$"- Написанно: **{user.SendedMsg}**\n" +
				$"- Исправлено: **{user.UpdatedMsg}**\n", true);
			embed.AddField("В голосе:", $"{user.InVoice.Hours}:{user.InVoice.Minutes}", true);

			await ReplyAsync(embed: embed.Build());
		}
	}
}
