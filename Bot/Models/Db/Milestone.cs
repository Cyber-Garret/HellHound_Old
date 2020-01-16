using Bot.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bot.Models.Db
{
	public class Milestone
	{
		[Key]
		public byte Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(50)]
		public string Alias { get; set; }
		[MaxLength(50)]
		public string Type { get; set; }
		[MaxLength(1000)]
		public string Icon { get; set; }
		/// <summary>
		/// Raid have 5 free space without leader, cycle for need 
		/// </summary>
		public byte MaxSpace { get; set; }

		public List<ActiveMilestone> ActiveMilestones { get; set; }
	}

	public class MilestoneUser
	{
		[Key]
		public int Id { get; set; }
		public ulong ActiveMilestoneMessageId { get; set; }
		public ActiveMilestone ActiveMilestone { get; set; }
		public ulong UserId { get; set; }
		public byte? Place { get; set; }
	}

	public class ActiveMilestone
	{
		[Key, Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public ulong MessageId { get; set; }
		[Required]
		public ulong GuildId { get; set; }
		[Required]
		public byte MilestoneId { get; set; }
		public Milestone Milestone { get; set; }

		[MaxLength(1000)]
		public string Memo { get; set; }
		[Required]
		public ulong Leader { get; set; }
		[Required]
		public DateTime DateExpire { get; set; }
		public MilestoneType MilestoneType { get; set; }

		public List<MilestoneUser> MilestoneUsers { get; set; }
	}
}
