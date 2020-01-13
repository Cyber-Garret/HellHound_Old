using Bot.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bot.Models.Db
{

	public class Config
	{
		[Key]
		public virtual int ConfigTypeId
		{
			get
			{
				return (int)ConfigType;
			}
			set
			{
				ConfigType = (ConfigType)value;
			}
		}
		[EnumDataType(typeof(ConfigType))]
		public ConfigType  ConfigType { get; set; }

		[MaxLength(128), Required]
		public string Value { get; set; }
	}
	public class GuildSelfRole
	{
		[Key]
		public int RowID { get; set; }
		public ulong EmoteID { get; set; }
		public ulong RoleID { get; set; }
	}
}
