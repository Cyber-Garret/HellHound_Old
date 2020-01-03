using System;
using System.Globalization;

namespace Site.Bot
{
	internal static class GlobalVariables
	{
		// Economy
		internal const ulong DailyGlimmerGain = 25;
		public const int MessageRewardCooldown = 30;
		public const int MessageXPCooldown = 6;
		public const int MessageRewardMinLenght = 20;
		// Modules
		internal const string InvisibleString = "\u200b";
		internal const string NotInGuildText = ":x: | Эта команда не доступна в личных сообщениях.";

		internal static CultureInfo culture = new CultureInfo("ru-Ru");
		internal static Random GetRandom = new Random();
	}
}
