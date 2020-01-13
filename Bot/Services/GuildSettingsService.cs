using Bot.Models;
using Bot.Models.Db;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Services
{
    public class GuildSettingsService
    {
        public GuildConfig config;

        private readonly HellContext Db;
        public GuildSettingsService(IServiceProvider service)
        {
            Db = service.GetRequiredService<HellContext>();
        }

        public void LoadConfig()
        {
            //take config for db;
            config = Db.GuildConfigs.First();
        }

        public async Task SaveAndReloadConfigAsync(GuildConfig config)
        {
            //update and save in db;
            Db.Update(config);
            await Db.SaveChangesAsync();
            //reload config
            this.config = config;
        }
    }
}
