using Bot.Core;
using Bot.Preconditions;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Site.Bot.Preconditions;

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

		[Command("лайк")]
		//[Cooldown(60)]
		public async Task UpRep([NoSelf]IUser user = null)
		{
			if (user == null || user.IsBot)
			{
				await ReplyAsync("Должен быть указан получатель.\nНапример: **!лайк @Миша**");
			}
			else
			{
				//Get user can add reputation?
				var sender = UserAccounts.GetUser(Context.User);
				var difference = DateTime.Now - sender.LastRep.AddDays(1);

				if (difference.TotalMinutes < 1)
				{
					var embed = new EmbedBuilder
					{
						Color = Color.Red,
						Description = $":diamond_shape_with_a_dot_inside: :clock1:  | **{Context.User.Username}, ты уже кому-то повышал репутацию.\nТы сможешь кому-то поднять репутацию через {difference:%h} ч. {difference:%m} мин.**"
					};
					await ReplyAsync(embed: embed.Build());
				}
				else
				{
					sender.LastRep = DateTime.UtcNow;
					UserAccounts.SaveAccount(Context.User);
					//Add Reputation to mentioned user
					var recipient = UserAccounts.GetUser(user);
					recipient.Reputation++;
					UserAccounts.SaveAccount(user);

					var embed = new EmbedBuilder
					{
						Color = Color.Gold,
						Description = $":diamond_shape_with_a_dot_inside: | Внимание {user.Mention}!\nТвоя репутация была повышена {Context.User.Mention}, не забудь его поблагодарить."
					};

					await ReplyAsync(embed: embed.Build());
				}
			}
		}
		[Command("дизлайк")]
		public async Task DownRep(SocketGuildUser user)
		{

		}
	}
}
