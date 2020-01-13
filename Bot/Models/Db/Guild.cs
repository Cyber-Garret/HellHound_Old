using Bot.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bot.Models.Db
{

	public class GuildConfig
	{
		[Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public ulong Id { get; set; }
		public ulong NotificationChannel { get; set; } = 0;
		public ulong LoggingChannel { get; set; } = 0;
		public ulong WelcomeChannel { get; set; } = 0;
		public string WelcomeMessage { get; set; } = string.Empty;
		public string LeaveMessage { get; set; } = string.Empty;
		public ulong AutoroleID { get; set; } = 0;
		public string GlobalMention { get; set; } = string.Empty;
		public bool Economy { get; set; } = false;
		public ulong SelfRoleMessageId { get; set; } = 0;
	}
	public class GuildSelfRole
	{
		[Key]
		public int RowID { get; set; }
		public ulong EmoteID { get; set; }
		public ulong RoleID { get; set; }
	}
}
