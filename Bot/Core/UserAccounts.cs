using Bot.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bot.Core
{
	internal static class UserAccounts
	{
		private static readonly ConcurrentDictionary<ulong, User> userAccounts;

		static UserAccounts()
		{
			var result = DataStorage.LoadJSONFromHDD<User>(DataStorage.UsersDirectory);

			if (result != null)
				userAccounts = result.ToConcurrentDictionary(k => k.Id);
			else
				userAccounts = new ConcurrentDictionary<ulong, User>();
		}

		internal static User GetUser(ulong UserId)
		{
			return userAccounts.GetOrAdd(UserId, (key) =>
			{
				var newAccount = new User { Id = UserId };
				DataStorage.SaveObject(newAccount, Path.Combine(DataStorage.UsersDirectory, $"{UserId}.json"), useIndentations: true);
				return newAccount;
			});
		}

		internal static void SaveAccount(ulong UserId)
		{
			DataStorage.SaveObject(GetUser(UserId), Path.Combine(DataStorage.UsersDirectory, $"{UserId}.json"), useIndentations: true);

		}
	}
}
