using System;

namespace Bot.Domain.Models.Repository
{
	public class User
	{
		public ulong Id { get; set; }
		public sbyte Reputation { get; set; } = 0;
		public uint Xp { get; set; } = 0;

		/// <summary>
		/// User total sent text messages
		/// </summary>
		public uint SentMessages { get; set; } = 0;
		/// <summary>
		/// User total updated text messages
		/// </summary>
		public uint UpdatedMessages { get; set; } = 0;
		/// <summary>
		/// User total deleted text messages
		/// </summary>
		public uint DeletedMessages { get; set; } = 0;

		public DateTime LastReply { get; set; } = DateTime.Now.AddDays(-2);

		public DateTime LastXpMessage { get; set; } = DateTime.Now;

		public uint LevelNumber => (uint)Math.Sqrt(Xp / 50);
	}
}
