using System;
using System.Collections.Generic;

namespace Bot.Models
{
	public class User
	{
		public ulong Id { get; set; }
		public uint Reputation { get; set; } = 0;
		public uint XP { get; set; } = 0;

		/// <summary>
		/// User total sended text messages
		/// </summary>
		public uint SendedMsg { get; set; } = 0;
		/// <summary>
		/// User total updated text messages
		/// </summary>
		public uint UpdatedMsg { get; set; } = 0;
		/// <summary>
		/// User total deleted text messages
		/// </summary>
		public uint DeletedMsg { get; set; } = 0;
		/// <summary>
		/// User total spend time in voice chats in Hours:Minutes
		/// </summary>
		public TimeSpan InVoice { get; set; } = new TimeSpan(0);
		public DateTime LastRep { get; set; } = DateTime.Now.AddDays(-2);
		public DateTime LastXPMessage { get; set; } = DateTime.Now;
		public uint LevelNumber
		{
			get
			{
				return (uint)Math.Sqrt(XP / 50);
			}
		}
		public uint NumberOfWarnings { get; set; }
		public List<Warning> Warnings { get; private set; } = new List<Warning>();
	}

	public class Warning
	{
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public string Reason { get; set; }
	}
}
