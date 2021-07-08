using Bot.Domain.Abstraction;

using DSharpPlusNextGen;
using DSharpPlusNextGen.SlashCommands;
using DSharpPlusNextGen.SlashCommands.EventArgs;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Bot.Application.Services
{
	public class SlashCommandHandler : ISlashCommandHandler
	{
		private readonly ILogger _logger;
		private readonly IServiceProvider _provider;
		private readonly DiscordClient _discord;


		public SlashCommandHandler(ILogger<SlashCommandHandler> logger, IServiceProvider provider, DiscordClient discord)
		{
			_logger = logger;
			_provider = provider;
			_discord = discord;
		}

		public Task Initialize()
		{
			var config = new SlashCommandsConfiguration
			{
				Services = _provider
			};

			var slashCommandsExtension = _discord.UseSlashCommands(config);
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(SlashCommandModule).IsAssignableFrom(type) && !type.IsNested))
			{
				slashCommandsExtension.RegisterCommands(type);
				_logger.LogInformation($"Registered {type.Name} slash command...");
			}

			slashCommandsExtension.SlashCommandExecuted += SlashCommandExecuted;
			slashCommandsExtension.SlashCommandErrored += SlashCommandError;

			return Task.CompletedTask;
		}

		private Task SlashCommandError(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
		{
			_logger.LogError(e.Exception, $"Error with {e.Context.CommandName} command.");
			return Task.CompletedTask;
		}

		private Task SlashCommandExecuted(SlashCommandsExtension sender, SlashCommandExecutedEventArgs e)
		{
			_logger.LogInformation($"{e.Context.User.Username} used {e.Context.CommandName}");
			return Task.CompletedTask;
		}
	}
}
