using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: HostingStartup(typeof(Web.Bot.BotHostingStartup))]
namespace Web.Bot
{
    public class BotHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //TODO New Intergrated Bot
            });
        }
    }
}
