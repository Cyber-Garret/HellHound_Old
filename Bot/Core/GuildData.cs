using Bot.Models;

using System.IO;
using System.Threading.Tasks;

namespace Bot.Core
{
	internal static class GuildData
	{
		private const string FileName = "guild.json";
		public static Guild guild = new Guild();
		internal static Task LoadGuildDataAsync()
		{
			var result = DataStorage.LoadSingleJSONFromHDD<Guild>(Path.Combine(DataStorage.ResDirectory, FileName));

			if (result != null)
				guild = result;

			return Task.CompletedTask;
		}

		internal static void SaveGuild()
		{
			DataStorage.SaveObject(guild, Path.Combine(DataStorage.ResDirectory, FileName), true);
		}
	}
}
