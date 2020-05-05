﻿using Discord.Commands;

using System;
using System.Threading.Tasks;

namespace Bot.Preconditions
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public sealed class RequireGuildOwner : PreconditionAttribute
	{
		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			if (context.Guild.OwnerId == context.User.Id)
				return Task.FromResult(PreconditionResult.FromSuccess());
			else
				return Task.FromResult(PreconditionResult.FromError("Эта команда доступна только владельцу сервера."));
		}
	}
}
