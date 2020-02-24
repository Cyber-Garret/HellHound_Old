using System.Collections.Generic;

namespace Bot.Models
{
	public class Guild
	{
		public ulong RoleId { get; set; } = 0;
		public List<uint> Colors { get; set; } = new List<uint>();
	}
}
