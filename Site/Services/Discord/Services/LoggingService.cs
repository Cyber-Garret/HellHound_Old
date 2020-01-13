using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace Site.Services.Discord.Services
{
	public class LoggingService
	{
		// declare the fields used later in this class
		private readonly ILogger logger;
		private readonly DiscordSocketClient discord;

		public LoggingService(IServiceProvider service)
		{
			// get the services we need via DI, and assign the fields declared above to them
			discord = service.GetRequiredService<DiscordSocketClient>();
			//emote = service.GetRequiredService<EmoteService>();
			logger = service.GetRequiredService<ILogger<LoggingService>>();

			// hook into these events with the methods provided below
			discord.Ready += Discord_Ready;
			discord.Log += Discord_Log;
			discord.Disconnected += Discord_Disconnected;
		}

		private Task Discord_Log(LogMessage message)
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

		private Task Discord_Disconnected(Exception ex)
		{
			logger.LogWarning($"Bot disconnected. [{ex.Message}]");
			return Task.CompletedTask;
		}

		private Task Discord_Ready()
		{
			logger.LogInformation($"Connected as -> [{discord.CurrentUser}] :)");
			return Task.CompletedTask;
		}
	}
}
