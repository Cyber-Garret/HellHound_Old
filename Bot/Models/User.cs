using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Models
{
	public class User
	{
        public ulong Id { get; set; }
        public uint Reputation { get; set; } = 0;
        public uint XP { get; set; } = 0;

        public uint SendedMsg { get; set; } = 0;
        public uint UpdatedMsg { get; set; } = 0;
        public uint DeletedMsg { get; set; } = 0;
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
        public List<string> Warnings { get; private set; } = new List<string>();
    }
}
