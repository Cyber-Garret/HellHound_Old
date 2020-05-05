using Bot.Models;
using System.IO;
using System.Linq;

namespace Bot.Core
{
	internal static class GuildData
	{
		public static Guild guild;
		static GuildData()
		{
			var result = DataStorage.LoadJSONFromHDD<Guild>(DataStorage.ResDirectory);

			if (result.Count > 0)
				guild = result.First();
			else
				guild = new Guild();
		}

		internal static void SaveGuild()
		{
			DataStorage.SaveObject(guild, Path.Combine(DataStorage.ResDirectory, "guild.json"), true);
		}
	}
}
