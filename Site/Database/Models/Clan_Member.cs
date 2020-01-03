using System;
using System.ComponentModel.DataAnnotations;

namespace Site.Database.Models
{
	public class Clan_Member
	{
		[Key]
		public ushort Id { get; set; }
		public string Name { get; set; }
		public byte DestinyMembershipType { get; set; }
		public ulong DestinyMembershipId { get; set; }
		public string IconPath { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTimeOffset ClanJoinDate { get; set; }
		[DisplayFormat(DataFormatString = "{0:HH:mm dd.MM.yyyy}")]
		public DateTimeOffset DateLastPlayed { get; set; }
		public ulong ClanId { get; set; }
		public Clan Clan { get; set; }
		public ushort ErrorCode { get; set; }
	}
}
