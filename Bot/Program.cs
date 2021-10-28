using Bot.Core.QuartzJobs;
using Bot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Serilog;
using System;

namespace Bot
{

	[UsedImplicitly]
	public class Program
	{
		public static int Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			Log.Information("Bootstraping HellHound bot...");

			try
			{
				CreateHostBuilder(args).Build().Run();
				Log.Information("Stopped cleanly");
				return 0;
			}
			catch (Exception exception)
			{
				Log.Fatal(exception, "An unhandled exception occurred during bootstrapping");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.UseSerilog((context, services, configuration) => configuration
					.ReadFrom.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.WriteTo.Console())
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<Bot>();
					//Quartz services
					services.AddHostedService<Quartz>();
					services.AddSingleton<IJobFactory, SingletonJobFactory>();
					services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
					//Bot services
					services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
					{
						AlwaysDownloadUsers = true,
						LogLevel = LogSeverity.Verbose,
						DefaultRetryMode = RetryMode.AlwaysRetry,
						MessageCacheSize = 1000,
						GatewayIntents = GatewayIntents.All
					}));
					services.AddSingleton<CommandService>()
						.AddSingleton<LoggingService>()
						.AddSingleton<CommandHandlerService>()
						.AddSingleton<GuildEventHandlerService>()
						.AddSingleton<LevelingService>();

					//Quartz Jobs
					services.AddSingleton<ColorChangeRoleJob>();
					services.AddSingleton(new JobSchedule(typeof(ColorChangeRoleJob),
						hostContext.Configuration["Bot:RainbowRolesCronExp"]));
					Log.Information(
						$"Rainbow role cron expression: \"{hostContext.Configuration["Bot:RainbowRolesCronExp"]}\"");
				});
		}

	}
}
