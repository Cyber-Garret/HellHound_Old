using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace Bot.Services
{
	public class LoggingService
	{
		// declare the fields used later in this class
		private readonly ILogger logger;
		private readonly DiscordSocketClient discord;
		private readonly CommandService command;
		private readonly EmoteService emote;

		public LoggingService(IServiceProvider service)
		{
			// get the services we need via DI, and assign the fields declared above to them
			discord = service.GetRequiredService<DiscordSocketClient>();
			command = service.GetRequiredService<CommandService>();
			emote = service.GetRequiredService<EmoteService>();
			logger = service.GetRequiredService<ILogger<LoggingService>>();

			// hook into these events with the methods provided below
			discord.Ready += OnReadyAsync;
			discord.Log += OnLogAsync;
			discord.Disconnected += OnDisconnectedAsync;
			command.Log += OnLogAsync;
		}

		private Task OnLogAsync(LogMessage message)
		{
			Task.Run(() =>
			{
				string logText = $"{message.Source}: {message.Message}";
				switch (message.Severity)
				{
					case LogSeverity.Critical:
						{
							logger.LogCritical(logText);
							break;
						}
					case LogSeverity.Error:
						{
							logger.LogError(logText);
							break;
						}
					case LogSeverity.Warning:
						{
							logger.LogWarning(logText);
							break;
						}
					case LogSeverity.Info:
						{
							logger.LogInformation(logText);
							break;
						}
					case LogSeverity.Verbose:
						{
							logger.LogInformation(logText);
							break;
						}
					case LogSeverity.Debug:
						{
							logger.LogDebug(logText);
							break;
						}

					default:
						logger.LogInformation(logText);
						break;
				}
			});
			return Task.CompletedTask;
		}

		private Task OnDisconnectedAsync(Exception ex)
		{
			Task.Run(() =>
			{
				logger.LogWarning($"Bot disconnected. [{ex.Message}]");
			});
			return Task.CompletedTask;
		}

		private Task OnReadyAsync()
		{
			Task.Run(() =>
			{
				logger.LogInformation($"Connected as -> [{discord.CurrentUser}] :)");
				emote.Configure();
			});
			return Task.CompletedTask;
		}
	}
}
