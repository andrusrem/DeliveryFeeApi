using Quartz;

namespace DeliveryFeeApi.CronJobs
{
    [DisallowConcurrentExecution]
    public class LoadStationWeatherJob : IJob
    {
        private readonly ILogger _logger;
        public LoadStationWeatherJob(ILogger<LoadStationWeatherJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Load Weather data job executed on {DateTime.UtcNow}");

            return Task.CompletedTask;
        }
    }
}
