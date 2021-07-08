
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;

namespace Bot.Application
{
	public class Program
	{
		private const string AppName = "HellHound";

		public static int Main(string[] args)
		{
			Console.Title = AppName;

			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			Log.Information($"Starting {AppName} bot.");

			try
			{
				CreateHostBuilder(args).Build().Run();
				Log.Information("Stopped cleanly");
				return 0;
			}
			catch (Exception exception)
			{
				Log.Fatal(exception, "An unhandled exception occured during bootstrapping.");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.UseSerilog((context, services, configuration) => configuration
					.ReadFrom.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.WriteTo.Console())
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<BotWorker>();

					services.AddDiscordClient(hostContext.Configuration)
						.AddSlashHandler();
				});
	}
}
