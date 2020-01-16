using Bot.Models;
using Bot.Services;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bot
{
	public class Program
	{
		public static IConfiguration Configuration { get; set; }

		public Program(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					services.Configure<DiscordSettings>(options => Configuration.GetSection("DiscordSettings").Bind(options));

					services.AddHostedService<Bot>();
					services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
					{
						ExclusiveBulkDelete = true,
						AlwaysDownloadUsers = true,
						LogLevel = LogSeverity.Verbose,
						DefaultRetryMode = RetryMode.AlwaysRetry,
						MessageCacheSize = 1000
					}));

					services.AddSingleton<CommandService>()
					.AddSingleton<LoggingService>()
					.AddSingleton<InteractiveService>()
					.AddSingleton<EmoteService>()
					.AddSingleton<MilestoneService>()
					.AddSingleton<LevelingService>()
					.AddSingleton<CommandHandlerService>()
					.AddSingleton<GuildEventHandlerService>()
					.AddSingleton<GuildSelfRoleService>()
					.AddSingleton<GuildSettingsService>();

					var connection = Configuration.GetConnectionString("DefaultConnection");

					//services.AddDbContext<HellContext>(options => options.UseSqlServer(connection));
				});
	}
}
