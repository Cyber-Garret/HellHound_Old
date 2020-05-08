using Bot.Core;
using Bot.Services;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot
{
	public class Bot : BackgroundService
	{
		private readonly IServiceProvider service;
		private readonly IConfiguration config;
		private readonly ILogger<Bot> logger;
		private readonly DiscordSocketClient discord;

		public Bot(IServiceProvider service, IConfiguration configuration)
		{
			this.service = service;
			config = configuration;
			logger = service.GetRequiredService<ILogger<Bot>>();
			discord = service.GetRequiredService<DiscordSocketClient>();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var token = config["Bot:Token"];

			if (string.IsNullOrWhiteSpace(token))
			{
				logger.LogError("Discord Token not provided");
				return;
			}
			try
			{
				service.GetRequiredService<LoggingService>();
				service.GetRequiredService<GuildEventHandlerService>().Configure();
				await service.GetRequiredService<CommandHandlerService>().ConfigureAsync();

				//Before bot login and ready for work, first need Load users and guild data from HDD
				await GuildData.LoadGuildDataAsync();
				await UserAccounts.LoadUserAccountsAsync();

				await discord.LoginAsync(TokenType.Bot, token);
				await discord.StartAsync();
				await discord.SetStatusAsync(UserStatus.Online);
				await discord.SetGameAsync(@"Destiny 2");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error starting bot");
				throw;
			}
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			await discord.SetStatusAsync(UserStatus.Offline);
			await discord.StopAsync();
			discord.Dispose();
		}
	}
}
