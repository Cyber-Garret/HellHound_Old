using Discord;
using Discord.WebSocket;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Site.Services.Discord.Services;

[assembly: HostingStartup(typeof(Site.Services.Discord.DiscordHostingStartup))]
namespace Site.Services.Discord
{
	public class DiscordHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{

				//Register Quartz dedicated service
				services.AddHostedService<DiscordHostedService>();
				// Add Discord bot service
				services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
				{
					AlwaysDownloadUsers = true,
					LogLevel = LogSeverity.Verbose,
					DefaultRetryMode = RetryMode.AlwaysRetry
				}));

				services.AddSingleton<LoggingService>();
			});
		}
	}
}
