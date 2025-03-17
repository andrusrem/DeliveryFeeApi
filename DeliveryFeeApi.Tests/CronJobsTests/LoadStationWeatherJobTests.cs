using DeliveryFeeApi.CronJobs;
using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Moq;
using Quartz;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.CronJobsTests
{
    [ExcludeFromCodeCoverage]
    public class LoadStationWeatherJobTests
    {
        private readonly Mock<ILogger<LoadStationWeatherJob>> _mockLogger;
        private readonly Mock<IStationWeatherService> _mockService;
        private readonly LoadStationWeatherJob _job;

        public LoadStationWeatherJobTests()
        {
            _mockLogger = new Mock<ILogger<LoadStationWeatherJob>>();
            _mockService = new Mock<IStationWeatherService>();
            _job = new LoadStationWeatherJob(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task Execute_call_GetWeatherData_and_LoadToDataBase_and_Log()
        {
            //Arrange
            var weatherData = new List<StationWeather>
            {
                new StationWeather  { StationName = "Tallinn-Harku", AirTemp = 25.0m },
                new StationWeather { StationName = "Tartu-Tõravere", AirTemp = 18.5m },
                new StationWeather { StationName = "SomeOtherStation", AirTemp = 22.0m }
            };

            _mockService.Setup(x => x.GetWeatherData()).ReturnsAsync(weatherData);
            _mockService.Setup(x => x.LoadToDatabase(weatherData)).Returns(Task.CompletedTask);

            var mockContext = new Mock<IJobExecutionContext>();
            //Act
            await _job.Execute(mockContext.Object);

            //Assert
            _mockService.Verify(x => x.GetWeatherData(), Times.Once());
            _mockService.Verify(x => x.LoadToDatabase(weatherData), Times.Once());
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Load Weather data job executed on {DateTime.UtcNow} ")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
