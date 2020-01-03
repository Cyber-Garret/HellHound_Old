using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Site.Database.Models
{
	public class Clan
	{
		[Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public ulong Id { get; set; }
		public string Name { get; set; }
		public DateTimeOffset CreateDate { get; set; }
		public string Motto { get; set; }
		public string About { get; set; }
		public byte MemberCount { get; set; }
		public IEnumerable<Clan_Member> Members { get; set; }
	}
}
