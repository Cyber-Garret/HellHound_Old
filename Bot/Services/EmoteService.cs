using Bot.Models;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;
using System.Linq;

namespace Bot.Services
{
	public class EmoteService
	{
		public IEmote Raid;
		public IEmote Arc;
		public IEmote Solar;
		public IEmote Void;
		public IEmote Glimmer;

		public IEmote ExoticEngram;
		public IEmote LegendaryEngram;
		public IEmote RareEngram;
		public IEmote UncommonEngram;
		public IEmote CommonEngram;
		public IEmote ReactSecond => new Emoji("2\u20e3");
		public IEmote ReactThird => new Emoji("3\u20e3");
		public IEmote ReactFourth => new Emoji("4\u20e3");
		public IEmote ReactFifth => new Emoji("5\u20e3");
		public IEmote ReactSixth => new Emoji("6\u20e3");

		private readonly DiscordSocketClient discord;
		private readonly DiscordSettings settings;

		public EmoteService(IServiceProvider service)
		{
			discord = service.GetRequiredService<DiscordSocketClient>();
			settings = service.GetRequiredService<IOptions<DiscordSettings>>().Value;
		}

		public void Configure()
		{
			var HoundHome = discord.GetGuild(settings.HoundDiscordServerId);
			//Raid and milestone
			Raid = HoundHome.Emotes.First(e => e.Name == "Hound_Raid");
			//Elemens
			Arc = HoundHome.Emotes.First(e => e.Name == "arc");
			Solar = HoundHome.Emotes.First(e => e.Name == "solar");
			Void = HoundHome.Emotes.First(e => e.Name == "void");
			//Currency
			Glimmer = HoundHome.Emotes.First(e => e.Name == "glimmer");
			//Engrams
			ExoticEngram = HoundHome.Emotes.First(e => e.Name == "Exotic_Engram");
			LegendaryEngram = HoundHome.Emotes.First(e => e.Name == "Legendary_Engram");
			RareEngram = HoundHome.Emotes.First(e => e.Name == "Rare_Engram");
			UncommonEngram = HoundHome.Emotes.First(e => e.Name == "Uncommon_Engram");
			CommonEngram = HoundHome.Emotes.First(e => e.Name == "Common_Engram");

		}
	}
}
