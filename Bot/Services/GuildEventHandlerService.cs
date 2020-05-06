using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Bot.Services
{
	public class GuildEventHandlerService
	{
		private readonly DiscordSocketClient discord;
		private readonly CommandHandlerService commandHandler;
		private readonly LevelingService levelingService;

		public GuildEventHandlerService(IServiceProvider services)
		{
			discord = services.GetRequiredService<DiscordSocketClient>();
			commandHandler = services.GetRequiredService<CommandHandlerService>();
			levelingService = services.GetRequiredService<LevelingService>();
		}

		public void Configure()
		{
			discord.MessageReceived += MessageReceived;
			discord.MessageUpdated += MessageUpdated;
			discord.MessageDeleted += MessageDeleted;
			discord.UserVoiceStateUpdated += UserVoiceStateUpdated;
		}

		private Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState oldState, SocketVoiceState newState)
		{
			return Task.CompletedTask;
		}

		private Task MessageReceived(SocketMessage message)
		{
			//Ignore messages from bots
			if (message.Author.IsBot) return Task.CompletedTask;

			//New Task for fix disconeting from Discord WebSockets by 1001 if current Task not completed.
			Task.Run(async () =>
			{
				await commandHandler.HandleCommandAsync(message);
				levelingService.Level(message.Author);
			});
			return Task.CompletedTask;
		}

		private async Task MessageUpdated(Cacheable<IMessage, ulong> cache, SocketMessage message, ISocketMessageChannel channel)
		{
			//Ignore messages from bots
			if (message.Author.IsBot || message.Author.Id == 0) return;

			await Task.Run(() =>
			 {
				 levelingService.UpdMsgCount(message.Author);
			 });
		}

		private async Task MessageDeleted(Cacheable<IMessage, ulong> cache, ISocketMessageChannel channel)
		{
			await Task.Run(async () =>
			 {
				 await levelingService.DelMsgCountAsync(channel);
			 });
		}
	}
}
