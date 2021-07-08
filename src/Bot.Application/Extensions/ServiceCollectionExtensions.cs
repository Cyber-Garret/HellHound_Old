using Bot.Application.Services;
using Bot.Domain.Abstraction;

using DSharpPlusNextGen;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class ServiceCollectionExtensions
	{
		internal static IServiceCollection AddDiscordClient(this IServiceCollection service, IConfiguration config) =>
			service.AddSingleton(new DiscordClient(new DiscordConfiguration
			{
				LoggerFactory = new LoggerFactory().AddSerilog(),
				Token = config["Token"],
				Intents = DiscordIntents.AllUnprivileged
			}));

		internal static IServiceCollection AddSlashHandler(this IServiceCollection service) =>
			service.AddSingleton<ISlashCommandHandler, SlashCommandHandler>();
	}
}
