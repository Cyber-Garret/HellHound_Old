using Discord;
using Discord.Commands;

using System;
using System.Threading.Tasks;

namespace Site.Bot.Preconditions
{
	/// <summary>
	/// Disallow use user parameter on self
	/// </summary>
	public sealed class NoSelf : ParameterPreconditionAttribute
	{
		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, ParameterInfo parameter, object value, IServiceProvider services)
		{
			var user = value is IUser ? (IUser)value : null;
			if ((user != null) && (context.User.Id == user.Id))
				return Task.FromResult(PreconditionResult.FromError("Страж, ты не можешь использовать эту команду на себя."));

			return Task.FromResult(PreconditionResult.FromSuccess());
		}
	}
}
