using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Site.Bot.Services;
using Site.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Site.Bot
{
	public class BotHostedService : IHostedService
	{
		private readonly IServiceProvider service;
		private readonly DiscordSocketClient discord;
		private readonly Settings settings;
		public BotHostedService(IServiceProvider service)
		{
			this.service = service;
			discord = service.GetRequiredService<DiscordSocketClient>();
			settings = service.GetRequiredService<IOptions<Settings>>().Value;
		}
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			service.GetRequiredService<LoggingService>();
			service.GetRequiredService<GuildEventHandlerService>().Configure();
			await service.GetRequiredService<CommandHandlerService>().ConfigureAsync();


			await discord.LoginAsync(TokenType.Bot, settings.Token);
			await discord.StartAsync();
			await discord.SetStatusAsync(UserStatus.Online);
			await discord.SetGameAsync(@"hell-hounds.ru");
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await discord.SetStatusAsync(UserStatus.Offline);
			await discord.LogoutAsync();
		}
	}
}
