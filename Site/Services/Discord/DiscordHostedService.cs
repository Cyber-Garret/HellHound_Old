using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Site.Models;
using Site.Services.Discord.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Site.Services.Discord
{
	public class DiscordHostedService : IHostedService
	{
		private readonly IServiceProvider service;
		private readonly ILogger<DiscordHostedService> logger;
		public DiscordSocketClient discord;
		private readonly DiscordSettings settings;

		public DiscordHostedService(IServiceProvider service)
		{
			this.service = service;
			logger = service.GetRequiredService<ILogger<DiscordHostedService>>();
			discord = service.GetRequiredService<DiscordSocketClient>();
			settings = service.GetRequiredService<IOptions<DiscordSettings>>().Value;
		}
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				service.GetRequiredService<LoggingService>();

				await discord.LoginAsync(TokenType.Bot, settings.Token);
				await discord.StartAsync();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Discord hosted service exception in start");
			}
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await discord?.LogoutAsync();
		}
	}
}
