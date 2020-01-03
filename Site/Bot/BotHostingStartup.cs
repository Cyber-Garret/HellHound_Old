using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Site.Bot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
[assembly: HostingStartup(typeof(Site.Bot.BotHostingStartup))]
namespace Site.Bot
{
	public class BotHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
				services.AddHostedService<BotHostedService>();

				//Bot services for DI
				services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
				{
					ExclusiveBulkDelete = true,
					AlwaysDownloadUsers = true,
					LogLevel = LogSeverity.Verbose,
					DefaultRetryMode = RetryMode.AlwaysRetry,
					MessageCacheSize = 300
				}));

				services.AddSingleton<CommandService>()
				.AddSingleton<LoggingService>()
				.AddSingleton<InteractiveService>()
				.AddSingleton<EmoteService>()
				.AddSingleton<MilestoneService>()
				.AddSingleton<LevelingService>()
				.AddSingleton<CommandHandlerService>()
				.AddSingleton<GuildEventHandlerService>()
				.AddSingleton<GuildSelfRoleService>();
			});
		}
	}
}
