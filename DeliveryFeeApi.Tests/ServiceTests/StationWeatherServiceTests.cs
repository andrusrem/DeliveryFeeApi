using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NuGet.ContentModel;
using System.Net;
using Xunit;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ServiceTests
{
    public class StationWeatherServiceTests
    {
        private readonly Mock<IStationWeatherRepository> _mockRepository;
        private readonly StationWeatherService _service;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ILogger<StationWeatherService>> _mockLogger;

        public StationWeatherServiceTests()
        {
            _mockRepository = new Mock<IStationWeatherRepository>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<StationWeatherService>>();
            _service = new StationWeatherService( _mockRepository.Object, _mockHttpClientFactory.Object, _mockLogger.Object );
        }
        [Fact]
        public async Task GetWeatherData_return_list_of_weather_data()
        {
            // Arrange
            var fakeResponse = @"<?xml version=""1.0""?>
        <observations>
            <station>
                <name>Kuressaare linn</name>
                <longitude>22.48944444411111</longitude>
                <latitude>58.26416666666667</latitude>
                <relativehumidity>77</relativehumidity>
                <airtemperature>0.2</airtemperature>
            </station>
            <station>
                <name>Tallinn-Harku</name>
                <longitude>24.602891666624284</longitude>
                <latitude>59.398122222355134</latitude>
                <phenomenon>Overcast</phenomenon>
                <visibility>35.0</visibility>
                <airpressure>1003</airpressure>
                <relativehumidity>72</relativehumidity>
                <airtemperature>-0.2</airtemperature>
            </station>
        </observations>";
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeResponse, System.Text.Encoding.UTF8, "application/xml")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _service.GetWeatherData();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
