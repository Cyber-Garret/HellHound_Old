using Microsoft.Extensions.Logging;

using Quartz;

using System.Threading.Tasks;

namespace Site.Services.Quartz.Jobs
{
	[DisallowConcurrentExecution]
	public class HelloWorldJob : IJob
	{
		private readonly ILogger<HelloWorldJob> _logger;
		public HelloWorldJob(ILogger<HelloWorldJob> logger)
		{
			_logger = logger;
		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogInformation("Hello world!");
			return Task.CompletedTask;
		}
	}
}
