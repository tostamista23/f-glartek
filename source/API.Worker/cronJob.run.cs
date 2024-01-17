using API.Application;
using API.Model;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

namespace API.Worker;

public class CronJobRun : IHostedService
{
    public CronJobRun(ICronJobService cronJobService)
    {
        CronJobService = cronJobService;
    }

    private ICronJobService CronJobService { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        List<CronJobDTO> list = await CronJobService.ListAsync();

        StdSchedulerFactory sf = new ();
        IScheduler scheduler = await sf.GetScheduler();
        await scheduler.Start();

        foreach (CronJobDTO cron in list)
        {
            JobDataMap dataMap = new JobDataMap
            {
                { "Uri", cron.Uri ?? "" },
                { "HttpMethod", cron.HttpMethod.ToString() },
                { "Body", cron.Body ?? "" }
            };

            IJobDetail job = JobBuilder.Create<CronJobProcessor>()
                .UsingJobData(dataMap)
                .WithIdentity("CronJob", "CronJob")
                .Build();

            try
            {
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("CronTrigger", "CronJob")
                    //TODO TIMEZONE
                    .WithCronSchedule(cron.Schecule)
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Schedule invalid?" + ex.ToString());
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
