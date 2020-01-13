namespace Bot.Helpers
{
	public enum MilestoneType : byte
	{
		Default,
		OldStyle
	}

	public enum ConfigType
	{
		NotificationChannel,
		LoggingChannel,
		WelcomeChannel,
		WelcomeMessage,
		LeaveMessage,
		AutoroleID,
		GlobalMention,
		Economy,
		SelfRoleMessageId
	}
}
