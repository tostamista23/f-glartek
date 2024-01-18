using API.CrossCutting;
using API.Database;
using API.Domain;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using static Quartz.Logging.OperationName;

namespace API.Scheduler;

public interface ISchedulerService
{
    Task StartAsync(CronJob cron);
    Task StopAsync(long id);
    Task InitializeAsync();
}

public class SchedulerService : ISchedulerService
{
    private IScheduler? _scheduler;
    private IJobDetail? _job;
    private readonly string JOBIDENTITY = "CronJob";
    private readonly string TRIGGERIDENTITY = "CronTrigger";
    private ICronJobRepository CronJobRepository { get; }

    public SchedulerService(ICronJobRepository cronJobRepository)
    {
        CronJobRepository = cronJobRepository;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _scheduler = await InitializeSchedulerAsync();
            _job = InitializeJob();
            await _scheduler.AddJob(_job, true);
            InitializeCurrentCronsAsync().GetAwaiter().GetResult();
            await _scheduler.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task InitializeCurrentCronsAsync()
    {
        foreach (CronJob cron in await CronJobRepository.Queryable.ToListAsync())
        {
            await StartAsync(cron);
        }
    }

    private async Task<IScheduler> InitializeSchedulerAsync()
    {
        StdSchedulerFactory sf = new();
        IScheduler scheduler = await sf.GetScheduler();
        return scheduler;
    }

    private IJobDetail InitializeJob()
    {
        return JobBuilder.Create<CronJobProcessor>().StoreDurably().WithIdentity(JOBIDENTITY, JOBIDENTITY).Build();
    }

    public async Task StartAsync(CronJob cron)
    {
        try
        {
            if (_scheduler != null && _job != null)
            {
                TimeZoneDTO? timezone = TimeZoneService.GetTimeZone(cron.TimeZone);

                if (timezone == null)
                {
                    Console.WriteLine("Invalid timezone " + cron.TimeZone);
                    return;
                }
                
                ITrigger trigger = TriggerBuilder.Create()
                    .ForJob(_job)
                    .UsingJobData("Uri", cron.Uri ?? "")
                    .UsingJobData("HttpMethod", cron.HttpMethod.ToString())
                    .UsingJobData("Body", cron.Body ?? "")
                    .WithIdentity(TRIGGERIDENTITY + cron.Id, JOBIDENTITY)
                    .StartAt(DateTime.Now.AddHours(timezone.offset))
                    .Build();

                await _scheduler.ScheduleJob(trigger);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Schedule invalid? " + ex.ToString());
        }
    }

    public async Task StopAsync(long id)
    {
        try
        {
            if (_scheduler != null && _job != null)
            {
                var d = await _scheduler.UnscheduleJob(new TriggerKey(TRIGGERIDENTITY + id, JOBIDENTITY));
                var d1 = await _scheduler.Interrupt(new JobKey(TRIGGERIDENTITY + id, JOBIDENTITY));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("StopAsync: " + ex.Message);
        }
    }
}

