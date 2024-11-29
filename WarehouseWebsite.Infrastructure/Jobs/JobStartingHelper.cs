using Quartz;

namespace WarehouseWebsite.Infrastructure.Jobs
{
    public class JobStartingHelper
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public JobStartingHelper(ISchedulerFactory factory)
        {
            _schedulerFactory = factory;
        }

        public async Task StartAsync(string jobKey)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var job = new JobKey(jobKey);
            await scheduler.TriggerJob(job);
        }
    }
}
