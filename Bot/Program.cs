using Bot.Core.QuartzJobs;
using Bot.Services;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

using Serilog;
using Serilog.Core;

using System;

namespace Bot
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			var builtConfig = CreateConfigBuilder(args);

			var log = CreateSerilogLogger(builtConfig);

			try
			{
				return Host.CreateDefaultBuilder(args)
					.ConfigureLogging(logger =>
					{
						logger.ClearProviders();
						logger.AddSerilog(logger: log, dispose: true);
					})
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
						MessageCacheSize = 1000
					}));
					services.AddSingleton<CommandService>()
					.AddSingleton<LoggingService>()
					.AddSingleton<CommandHandlerService>()
					.AddSingleton<GuildEventHandlerService>()
					.AddSingleton<LevelingService>()
					.AddSingleton<InteractiveService>();

					//Quartz Jobs 
					services.AddSingleton<ColorChangeRoleJob>();
					services.AddSingleton(new JobSchedule(typeof(ColorChangeRoleJob), "0 0/1 * * * ?"));
				})
					.ConfigureAppConfiguration((hostContext, config) =>
				{
					config.AddConfiguration(builtConfig);
				});
			}
			catch (Exception ex)
			{
				log.Fatal(ex, "Host builder error");
				throw;
			}
			finally
			{
				log.Dispose();
			}

		}

		private static IConfigurationRoot CreateConfigBuilder(string[] args)
		{
			return new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddCommandLine(args)
			.Build();
		}

		private static Logger CreateSerilogLogger(IConfigurationRoot configuration)
		{
			// create logger with console output by default
			var logger = new LoggerConfiguration()
				.WriteTo.Console();
			// get path for logging in file from appsettings.json
			var logPath = configuration["Logging:FilePath"];

			// check if filepath for logging presented
			if (!string.IsNullOrWhiteSpace(logPath))
			{
				logger.WriteTo.File(logPath);
				return logger.CreateLogger();
			}
			else
			{
				return logger.CreateLogger();
			}
		}
	}
}
