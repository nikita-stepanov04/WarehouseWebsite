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
        
        public async Task StartAsync(string jobKey, Dictionary<string, string>? paramDict = default)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var job = new JobKey(jobKey);

            var jobDataMap = new JobDataMap();
            if (paramDict != null && paramDict.Any())
            {
                foreach (var kvp in paramDict)
                {
                    jobDataMap.Add(kvp.Key, kvp.Value!);
                }
            }

            await scheduler.TriggerJob(job, jobDataMap);
        }
    }
}
