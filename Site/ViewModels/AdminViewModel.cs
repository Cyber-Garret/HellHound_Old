using Discord.WebSocket;

using System.Collections.Generic;

namespace Site.ViewModels
{
	public class AdminViewModel
	{
		public SocketGuildUser Owner { get; set; }
		public IEnumerable<SocketGuildUser> D2Cerberus { get; set; }
		public IEnumerable<SocketGuildUser> LACerberus { get; set; }
		public IEnumerable<SocketGuildUser> WFCerberus { get; set; }
	}
}
