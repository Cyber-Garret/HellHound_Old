using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

[assembly: HostingStartup(typeof(Site.Services.Quartz.QuartzHostingStartup))]
namespace Site.Services.Quartz
{
	public class QuartzHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{

				//Register Quartz dedicated service
				services.AddHostedService<QuartzHostedService>();
				// Add Quartz services
				services.AddSingleton<IJobFactory, SingletonJobFactory>();
				services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

				// Add our job
				//services.AddSingleton<BungieJob>();
				//services.AddSingleton(new JobSchedule(typeof(BungieJob), "0 0/15 * * * ?")); // run every 15 minute
			});
		}
	}
}
