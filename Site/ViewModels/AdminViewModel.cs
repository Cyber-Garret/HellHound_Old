using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
