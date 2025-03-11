using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Quartz;

namespace DeliveryFeeApi.CronJobs
{
    [DisallowConcurrentExecution]
    public class LoadStationWeatherJob : IJob
    {
        private readonly ILogger _logger;
        private readonly IStationWeatherService _stationWeatherService;
        public LoadStationWeatherJob(ILogger<LoadStationWeatherJob> logger, IStationWeatherService stationWeatherService)
        {
            _logger = logger;
            _stationWeatherService = stationWeatherService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _stationWeatherService.LoadToDatabase();
            _logger.LogInformation($"Load Weather data job executed on {DateTime.UtcNow} ");
            
        }
    }
}
