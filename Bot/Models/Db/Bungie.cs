using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bot.Models.Db
{
	public class Clan
	{
		[Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public uint Id { get; set; }
		public string Name { get; set; }
		public DateTimeOffset CreateDate { get; set; }
		public string Motto { get; set; }
		public string About { get; set; }
		public byte MemberCount { get; set; }
		public List<Clan_Member> Members { get; set; }

	}

	public class Clan_Member
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public byte DestinyMembershipType { get; set; }
		public ulong DestinyMembershipId { get; set; }
		public string IconPath { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		public DateTimeOffset? ClanJoinDate { get; set; }
		[DisplayFormat(DataFormatString = "{0:HH:mm dd.MM.yyyy}")]
		public DateTimeOffset? DateLastPlayed { get; set; }
		public uint ClanId { get; set; }
		public Clan Clan { get; set; }
	}
}
