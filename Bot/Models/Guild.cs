using System.Collections.Generic;

namespace Bot.Models
{
	public class Guild
	{
		public List<RainbowRole> rainbowRoles = new List<RainbowRole>();
	}

	public class RainbowRole
	{
		public ulong RoleId { get; set; } = 0;
		public List<uint> Colors { get; set; } = new List<uint>();
	}
}
