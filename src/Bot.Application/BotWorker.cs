using DSharpPlusNextGen;

using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Bot.Domain.Abstraction;

namespace Bot.Application
{
	public class BotWorker : IHostedService
	{
		private readonly DiscordClient _discord;
		private readonly ISlashCommandHandler _slash;

		public BotWorker(DiscordClient discord, ISlashCommandHandler slash)
		{
			_discord = discord;
			_slash = slash;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _slash.Initialize();
			await _discord.ConnectAsync();
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _discord.DisconnectAsync();
			_discord.Dispose();
		}
	}
}
