using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

namespace Bot.Services
{
	public class GuildEventHandlerService
	{
		private readonly DiscordSocketClient discord;
		private readonly CommandHandlerService commandHandler;


		public GuildEventHandlerService(IServiceProvider services)
		{
			discord = services.GetRequiredService<DiscordSocketClient>();
			commandHandler = services.GetRequiredService<CommandHandlerService>();
		}

		public void Configure()
		{
			discord.MessageReceived += Client_MessageReceived;
		}

		private Task Client_MessageReceived(SocketMessage message)
		{
			//Ignore messages from bots
			if (message.Author.IsBot) return Task.CompletedTask;

			//New Task for fix disconeting from Discord WebSockets by 1001 if current Task not completed.
			Task.Run(async () =>
			{
				await commandHandler.HandleCommandAsync(message);
			});
			return Task.CompletedTask;
		}

	}
}
