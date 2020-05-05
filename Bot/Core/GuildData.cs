using Bot.Models;

namespace Bot.Core
{
	internal static class GuildData
	{
		private const string filename = "guild";
		public static Guild guild;
		static GuildData()
		{
			var result = DataStorage.LoadJSONFromHDD<Guild>(filename);

			if (result != null)
				guild = result;
			else
				guild = new Guild();
		}

		internal static void SaveGuild()
		{
			DataStorage.SaveObject(guild, filename, true);
		}
	}
}
