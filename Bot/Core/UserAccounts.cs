using Bot.Models;

using Discord;

using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Bot.Core
{
	internal static class UserAccounts
	{
		private static ConcurrentDictionary<ulong, User> userAccounts = new ConcurrentDictionary<ulong, User>();

		/// <summary>
		/// Load all json files into thread-safe collection for future work
		/// </summary>
		internal static Task LoadUserAccountsAsync()
		{
			var result = DataStorage.LoadJSONFromHDD<User>(DataStorage.UsersDirectory);

			if (result != null)
				userAccounts = result.ToConcurrentDictionary(k => k.Id);

			return Task.CompletedTask;
		}


		/// <summary>
		/// Get or create and return user account
		/// </summary>
		/// <param name="user">Discord generic user</param>
		/// <returns>user account</returns>
		internal static User GetUser(IUser user)
		{
			if (user.IsBot) return null;

			return userAccounts.GetOrAdd(user.Id, (key) =>
			{
				var newAccount = new User { Id = user.Id };
				DataStorage.SaveObject(newAccount, Path.Combine(DataStorage.UsersDirectory, $"{user.Id}.json"), useIndentations: true);
				return newAccount;
			});
		}

		/// <summary>
		/// Rewrite user file on hard drive
		/// </summary>
		/// <param name="user">Discord generic user</param>
		internal static void SaveAccount(IUser user)
		{
			if (user.IsBot) return;

			DataStorage.SaveObject(GetUser(user), Path.Combine(DataStorage.UsersDirectory, $"{user.Id}.json"), useIndentations: true);

		}
	}
}
