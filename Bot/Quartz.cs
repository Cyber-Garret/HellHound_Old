using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bot
{
	public class Quartz : BackgroundService
	{
		public IScheduler Scheduler { get; set; }

		private readonly ISchedulerFactory _schedulerFactory;
		private readonly IJobFactory _jobFactory;
		private readonly IEnumerable<JobSchedule> _jobSchedules;

		public Quartz(IServiceProvider service)
		{
			_schedulerFactory = service.GetRequiredService<ISchedulerFactory>();
			_jobFactory = service.GetRequiredService<IJobFactory>();
			_jobSchedules = service.GetRequiredService<IEnumerable<JobSchedule>>();
		}
		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
			Scheduler.JobFactory = _jobFactory;

			foreach (var jobSchedule in _jobSchedules)
			{
				var job = CreateJob(jobSchedule);
				var trigger = CreateTrigger(jobSchedule);

				await Scheduler.ScheduleJob(job, trigger, cancellationToken);
			}

			await Scheduler.Start(cancellationToken);
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			await Scheduler?.Shutdown(cancellationToken);
		}

		private static IJobDetail CreateJob(JobSchedule schedule)
		{
			var jobType = schedule.JobType;
			return JobBuilder
				.Create(jobType)
				.WithIdentity(jobType.FullName)
				.WithDescription(jobType.Name)
				.Build();
		}

		private static ITrigger CreateTrigger(JobSchedule schedule)
		{
			return TriggerBuilder
				.Create()
				.WithIdentity($"{schedule.JobType.FullName}.trigger")
				.WithCronSchedule(schedule.CronExpression)
				.WithDescription(schedule.CronExpression)
				.Build();
		}
	}

	public class SingletonJobFactory : IJobFactory
	{
		private readonly IServiceProvider _serviceProvider;
		public SingletonJobFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
		{
			return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
		}

		public void ReturnJob(IJob job) { }
	}

	public class JobSchedule
	{
		public JobSchedule(Type jobType, string cronExpression)
		{
			JobType = jobType;
			CronExpression = cronExpression;
		}

		public Type JobType { get; }
		public string CronExpression { get; }
	}
}
