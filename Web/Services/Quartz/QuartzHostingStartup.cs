using Microsoft.AspNetCore.Hosting;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Web.Services.Quartz.Jobs;

[assembly: HostingStartup(typeof(Web.Services.Quartz.QuartzHostingStartup))]
namespace Web.Services.Quartz
{
    public class QuartzHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

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
