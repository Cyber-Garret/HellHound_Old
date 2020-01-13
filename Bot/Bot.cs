using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.Models;
using Bot.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot
{
	public class Bot : BackgroundService
	{
		private readonly IServiceProvider service;
		private readonly ILogger<Bot> logger;
		private readonly DiscordSocketClient discord;
		private readonly DiscordSettings settings;

		public Bot(IServiceProvider service)
		{
			this.service = service;
			logger = service.GetRequiredService<ILogger<Bot>>();
			discord = service.GetRequiredService<DiscordSocketClient>();
			settings = service.GetRequiredService<IOptions<DiscordSettings>>().Value;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			if (string.IsNullOrWhiteSpace(settings.Token))
			{
				logger.LogError("Discord Token not provided");
				return;
			}
			try
			{
				service.GetRequiredService<LoggingService>();
				service.GetRequiredService<GuildEventHandlerService>().Configure();
				await service.GetRequiredService<CommandHandlerService>().ConfigureAsync();


				await discord.LoginAsync(TokenType.Bot, settings.Token);
				await discord.StartAsync();
				await discord.SetStatusAsync(UserStatus.Online);
				await discord.SetGameAsync(@"hell-hounds.ru");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error starting bot");
				throw;
			}
		}
	}
}
