using Bot.Helpers;
using Bot.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;

namespace Bot.Services
{
	public class GuildSettingsService
	{
		public ulong NotificationChannel = 0;
		public ulong LoggingChannel = 0;
		public ulong WelcomeChannel = 0;
		public string WelcomeMessage = string.Empty;
		public string LeaveMessage = string.Empty;
		public ulong AutoroleID = 0;
		public string GlobalMention = string.Empty;
		public bool Economy = false;
		public ulong SelfRoleMessageId = 0;

		private readonly HellContext Db;
		public GuildSettingsService(IServiceProvider service)
		{
			Db = service.GetRequiredService<HellContext>();
		}

		public void Configure()
		{
			var NotifDb = Db.Configs.FirstOrDefault(c => c.ConfigType == ConfigType.NotificationChannel);
			if (!string.IsNullOrEmpty(NotifDb.Value))
				NotificationChannel = Convert.ToUInt64(NotifDb);

			var logDb = Db.Configs.FirstOrDefault(c => c.ConfigType == ConfigType.LoggingChannel);
			if (!string.IsNullOrEmpty(logDb.Value))
				LoggingChannel = Convert.ToUInt64(logDb.Value);
		}
	}
}
