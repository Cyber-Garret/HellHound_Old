namespace Bot.Models
{
	public class DiscordSettings
	{
		public string Token { get; set; }
		public ulong HoundDiscordServerId { get; set; } = 513825031525105684;
		public char Prefix { get; set; } = '!';
	}
}
