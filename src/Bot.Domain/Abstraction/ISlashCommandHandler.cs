using System.Threading.Tasks;

namespace Bot.Domain.Abstraction
{
	public interface ISlashCommandHandler
	{
		Task Initialize();
	}
}
