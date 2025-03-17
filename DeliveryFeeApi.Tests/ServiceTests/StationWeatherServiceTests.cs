using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Xunit;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ServiceTests
{
    [ExcludeFromCodeCoverage]
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
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetWeatherData_return_exception_due_to_bad_request()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
        
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });
        
            var httpClient = new HttpClient(handlerMock.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            //Act and Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetWeatherData());
        }

        [Fact]
        public async Task GetWeatherData_return_logError()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(handlerMock.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetWeatherData());

            // Verify logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error fetching weather data")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public void ParseWeatherData_return_data_from_valid_xml()
        {
            //Arrange
            var fakeResponse = @"<?xml version=""1.0""?>
                     <observations>
                         <station>
                            <name>Tallinn-Harku</name>
                            <wmocode>26038</wmocode>
                            <longitude>24.602891666624284</longitude>
                            <latitude>59.398122222355134</latitude>
                            <phenomenon>Clear</phenomenon>
                            <visibility>35.0</visibility>
                            <precipitations>0</precipitations>
                            <airpressure>1021.2</airpressure>
                            <relativehumidity>56</relativehumidity>
                            <airtemperature>-1.3</airtemperature>
                            <winddirection>291</winddirection>
                            <windspeed>2.8</windspeed>
                            <windspeedmax>5.3</windspeedmax>
                            <waterlevel/>
                            <waterlevel_eh2000/>
                            <watertemperature/>
                            <uvindex>1.4</uvindex>
                            <sunshineduration>150</sunshineduration>
                            <globalradiation>223</globalradiation>
                        </station>
                     </observations>";

            //Act
            var result = _service.ParseWeatherData(fakeResponse);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal("Tallinn-Harku", result[0].StationName);
            Assert.Equal(26038, result[0].VmoCode);
            Assert.Equal(-1.3m, result[0].AirTemp);
            Assert.Equal(2.8m, result[0].WindSpeed);
            Assert.Equal("Clear", result[0].WeatherPhenomenon);
        }

        [Fact]
        public void ParseWeatherData_return_empty_list_from_invalied_xml()
        {
            //Arrange
            var fakeResponse = @"<?xml version=""1.0""?>
                     <observations>
                         <not_station>
                            <name>Tallinn-Harku</name>
                            <wmocode>wrong</wmocode>
                        </not_station>
                     </observations>";

            //Act
            var result = _service.ParseWeatherData(fakeResponse);

            //Assert
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void ParseWeatherData_return_error_log_and_empty_list()
        {
            //Arrange
            

            //Act
            var result = _service.ParseWeatherData("");

            //Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error parsing weather XML data")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
            Assert.Empty(result);
        }

        [Fact]
        public async Task LoadToDatabase_save_to_the_database_only_valid_options()
        {
            // Arrange
            var weatherData = new List<StationWeather>
            {
                new StationWeather  { StationName = "Tallinn-Harku", AirTemp = 25.0m },
                new StationWeather { StationName = "Tartu-Tõravere", AirTemp = 18.5m },
                new StationWeather { StationName = "SomeOtherStation", AirTemp = 22.0m }
            };

            
            // Act
            await _service.LoadToDatabase(weatherData);

            // Assert
            _mockRepository.Verify(r => r.Save(It.Is<StationWeather>(s => s.StationName == "Tallinn-Harku")), Times.Once);
            _mockRepository.Verify(r => r.Save(It.Is<StationWeather>(s => s.StationName == "Tartu-Tõravere")), Times.Once);
            _mockRepository.Verify(r => r.Save(It.Is<StationWeather>(s => s.StationName == "SomeOtherStation")), Times.Never);
        }

        [Fact]
        public async Task LoadToDatabase_return_warning_if_no_data()
        {
            // Arrange

            // Act
            await _service.LoadToDatabase([]);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No weather data available to load into the database.")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task LoadToDatabase_save_to_the_database_all_valid_options()
        {
            // Arrange
            var weatherData = new List<StationWeather>
            {
                new StationWeather { StationName = "Tallinn-Harku" },
                new StationWeather { StationName = "Pärnu" },
                new StationWeather { StationName = "Tartu-Tõravere" },
            };

            // Act
            await _service.LoadToDatabase(weatherData);

            // Assert
            _mockRepository.Verify(r => r.Save(It.Is<StationWeather>(s => s.StationName == "Tallinn-Harku")), Times.Once);
            _mockRepository.Verify(r => r.Save(It.Is<StationWeather>(s => s.StationName == "Tartu-Tõravere")), Times.Once);
            _mockRepository.Verify(r => r.Save(It.Is<StationWeather>(s => s.StationName == "Pärnu")), Times.Once);
        }


    }
}
