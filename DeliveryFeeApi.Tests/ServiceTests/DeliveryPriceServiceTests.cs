using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ServiceTests
{
    [ExcludeFromCodeCoverage]
    public class DeliveryPriceServiceTests
    {
        private readonly Mock<IAirTemperatureExtraFeeRepository> _mockAirTemperatureExtraFeeRepository;
        private readonly Mock<IWeatherPhenomenonExtraFeeRepository> _mockWeatherPhenomenonExtraFeeRepository;
        private readonly Mock<IRegionalBaseFeeRepository> _mockRegionalBaseFeeRepository;
        private readonly Mock<IWindSpeedExtraFeeRepository> _mockWindSpeedExtraFeeRepository;
        private readonly Mock<IStationWeatherRepository> _mockStationWeatherRepository;
        private readonly Mock<ILogger<DeliveryPriceService>> _mockLogger;
        private readonly DeliveryPriceService _service;

        public DeliveryPriceServiceTests()
        {
            _mockAirTemperatureExtraFeeRepository = new Mock<IAirTemperatureExtraFeeRepository>();
            _mockWeatherPhenomenonExtraFeeRepository = new Mock<IWeatherPhenomenonExtraFeeRepository>();
            _mockRegionalBaseFeeRepository = new Mock<IRegionalBaseFeeRepository>();
            _mockWindSpeedExtraFeeRepository = new Mock<IWindSpeedExtraFeeRepository>();
            _mockStationWeatherRepository = new Mock<IStationWeatherRepository>();
            _mockLogger = new Mock<ILogger<DeliveryPriceService>>();
            _service = new DeliveryPriceService(
                _mockAirTemperatureExtraFeeRepository.Object,
                _mockWeatherPhenomenonExtraFeeRepository.Object,
                _mockRegionalBaseFeeRepository.Object,
                _mockWindSpeedExtraFeeRepository.Object,
                _mockStationWeatherRepository.Object,
                _mockLogger.Object);
        }

        [Fact]
        public void GetAirTemperatureFee_return_one_euro_air_temperature_fee_if_vehicle_scooter_and_temperature_minus_11()
        {
            //Arrange
            var type = VehicleEnum.Scooter;
            decimal temp = -11;
            var airTempFees = new List<AirTemperatureExtraFee>
            {
               new AirTemperatureExtraFee{Id = 0, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Scooter, Price = 1},
               new AirTemperatureExtraFee{Id = 1, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Bike, Price = 1},
               new AirTemperatureExtraFee{Id = 2, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 0.5m},
               new AirTemperatureExtraFee{Id = 3, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Bike, Price = 0.5m},
            };

            _mockAirTemperatureExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(airTempFees
                                .Where(x => x.VehicleType == type)
                                .Where(x => x.LowerTemperature < temp)
                                .Where(x => x.UpperTemperature > temp)
                                .ToList());

            //Act
            var result = _service.GetAirTemperatureFee(VehicleEnum.Scooter, -11);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetAirTemperatureFee_return_half_euro_air_temperature_fee_if_vehicle_bike_and_temperature_minus_5()
        {
            //Arrange
            var type = VehicleEnum.Bike;
            decimal temp = -5;
            var airTempFees = new List<AirTemperatureExtraFee>
            {
               new AirTemperatureExtraFee{Id = 0, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Scooter, Price = 1},
               new AirTemperatureExtraFee{Id = 1, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Bike, Price = 1},
               new AirTemperatureExtraFee{Id = 2, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 0.5m},
               new AirTemperatureExtraFee{Id = 3, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Bike, Price = 0.5m},
            };

            _mockAirTemperatureExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(airTempFees
                                .Where(x => x.VehicleType == type)
                                .Where(x => x.LowerTemperature < temp)
                                .Where(x => x.UpperTemperature > temp)
                                .ToList());

            //Act
            var result = _service.GetAirTemperatureFee(VehicleEnum.Bike, -5);

            //Assert
            Assert.Equal(0.5m, result);
        }

        [Fact]
        public void GetAirTemperatureFee_return_zero_air_temperature_fee_if_vehicle_car_and_temperature_is_1()
        {
            //Arrange
            var type = VehicleEnum.Car;
            decimal temp = 1;
            var airTempFees = new List<AirTemperatureExtraFee>
            {
               new AirTemperatureExtraFee{Id = 0, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Scooter, Price = 1},
               new AirTemperatureExtraFee{Id = 1, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Bike, Price = 1},
               new AirTemperatureExtraFee{Id = 2, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 0.5m},
               new AirTemperatureExtraFee{Id = 3, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Bike, Price = 0.5m},
            };

            _mockAirTemperatureExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(airTempFees
                                .Where(x => x.VehicleType == type)
                                .Where(x => x.LowerTemperature < temp)
                                .Where(x => x.UpperTemperature > temp)
                                .ToList());

            //Act
            var result = _service.GetAirTemperatureFee(VehicleEnum.Car, 1);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetWeatherPhenomenonFee_return_one_euro_if_phenomenon_is_related_to_snow_or_sleet_and_vehicle_is_Bike()
        {
            //Arrange
            var type = VehicleEnum.Bike;
            string phenomenon = "Light snowfall";
            var weatherPhenomenonFees = new List<WeatherPhenomenonExtraFee>
            {
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light shower", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate shower", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy shower", VehicleType = VehicleEnum.Bike,  Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snowfall", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snowfall", VehicleType = VehicleEnum.Scooter, Price = 1,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snowfall", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light sleet", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light sleet", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate sleet", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate sleet", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Glaze", VehicleType = VehicleEnum.Scooter, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Glaze", VehicleType = VehicleEnum.Bike, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Scooter, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWeatherPhenomenonExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(weatherPhenomenonFees
                                .Where(x => x.VehicleType == type)
                                .Where(x => x.WeatherPhenomenon == phenomenon)
                                .ToList());

            //Act
            var result = _service.GetWeatherPhenomenonFee(type, phenomenon);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetWeatherPhenomenonFee_return_null_if_forbitten_true_and_phenomenon_is_related_to_glaze_and_vehicle_is_Scooter()
        {
            //Arrange
            var type = VehicleEnum.Scooter;
            string phenomenon = "Glaze";
            var weatherPhenomenonFees = new List<WeatherPhenomenonExtraFee>
            {
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light shower", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate shower", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy shower", VehicleType = VehicleEnum.Bike,  Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snowfall", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snowfall", VehicleType = VehicleEnum.Scooter, Price = 1,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snowfall", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light sleet", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light sleet", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate sleet", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate sleet", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Glaze", VehicleType = VehicleEnum.Scooter, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Glaze", VehicleType = VehicleEnum.Bike, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Scooter, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWeatherPhenomenonExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(weatherPhenomenonFees
                                .Where(x => x.VehicleType == type)
                                .Where(x => x.WeatherPhenomenon == phenomenon)
                                .ToList());

            //Act
            var result = _service.GetWeatherPhenomenonFee(type, phenomenon);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void GetWeatherPhenomenonFee_return_zero_if__phenomenon_is_clear_and_vehicle_is_Bike()
        {
            //Arrange
            var type = VehicleEnum.Bike;
            string phenomenon = "Clear";
            var weatherPhenomenonFees = new List<WeatherPhenomenonExtraFee>
            {
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy rain", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy rain", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light shower", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate shower", VehicleType = VehicleEnum.Bike, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy shower", VehicleType = VehicleEnum.Scooter, Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy shower", VehicleType = VehicleEnum.Bike,  Price = 0.5M, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snowfall", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snowfall", VehicleType = VehicleEnum.Scooter, Price = 1,},
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snowfall", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snowfall", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snow shower", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Heavy snow shower", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light sleet", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Light sleet", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate sleet", VehicleType = VehicleEnum.Scooter, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Moderate sleet", VehicleType = VehicleEnum.Bike, Price = 1, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Glaze", VehicleType = VehicleEnum.Scooter, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Glaze", VehicleType = VehicleEnum.Bike, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Scooter, Forbitten = true, },
               new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWeatherPhenomenonExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(weatherPhenomenonFees
                                .Where(x => x.VehicleType == type)
                                .Where(x => x.WeatherPhenomenon == phenomenon)
                                .ToList());

            //Act
            var result = _service.GetWeatherPhenomenonFee(type, phenomenon);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetWindSpeedFee_return_half_euro_if_wind_speed_is_between_10_20_ms_and_vehicle_is_Bike()
        {
            //Arrange
            var type = VehicleEnum.Bike;
            var speed = 12;
            var windSpeedFees = new List<WindSpeedExtraFee> {
                new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, },
                new WindSpeedExtraFee { LowerSpeed = 20, UpperSpeed = 1000, VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWindSpeedExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(windSpeedFees
                .Where(x => x.VehicleType == type)
                .Where(x => x.LowerSpeed < speed)
                .Where(x => x.UpperSpeed > speed)
                .ToList());

            //Act
            var result = _service.GetWindSpeedFee(type, speed);

            //Assert
            Assert.Equal(0.5m, result);
        }

        [Fact]
        public void GetWindSpeedFee_return_zero_if_wind_speed_is_lower_than_10_ms_and_vehicle_is_Bike()
        {
            //Arrange
            var type = VehicleEnum.Bike;
            var speed = 8;
            var windSpeedFees = new List<WindSpeedExtraFee> {
                new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, },
                new WindSpeedExtraFee { LowerSpeed = 20, UpperSpeed = 1000, VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWindSpeedExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(windSpeedFees
                .Where(x => x.VehicleType == type)
                .Where(x => x.LowerSpeed < speed)
                .Where(x => x.UpperSpeed > speed)
                .ToList());

            //Act
            var result = _service.GetWindSpeedFee(type, speed);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetWindSpeedFee_return_null_if_wind_speed_is_higher_than_20_ms_and_vehicle_is_Bike()
        {
            //Arrange
            var type = VehicleEnum.Bike;
            var speed = 25;
            var windSpeedFees = new List<WindSpeedExtraFee> {
                new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, },
                new WindSpeedExtraFee { LowerSpeed = 20, UpperSpeed = 1000, VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWindSpeedExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(windSpeedFees
                .Where(x => x.VehicleType == type)
                .Where(x => x.LowerSpeed < speed)
                .Where(x => x.UpperSpeed > speed)
                .ToList());

            //Act
            var result = _service.GetWindSpeedFee(type, speed);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void GetWindSpeedFee_return_zero_if__vehicle_is_Scooter()
        {
            //Arrange
            var type = VehicleEnum.Scooter;
            var speed = 8;
            var windSpeedFees = new List<WindSpeedExtraFee> {
                new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, },
                new WindSpeedExtraFee { LowerSpeed = 20, UpperSpeed = 1000, VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWindSpeedExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(windSpeedFees
                .Where(x => x.VehicleType == type)
                .Where(x => x.LowerSpeed < speed)
                .Where(x => x.UpperSpeed > speed)
                .ToList());

            //Act
            var result = _service.GetWindSpeedFee(type, speed);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetWindSpeedFee_return_zero_if__vehicle_is_Car()
        {
            //Arrange
            var type = VehicleEnum.Car;
            var speed = 8;
            var windSpeedFees = new List<WindSpeedExtraFee> {
                new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, },
                new WindSpeedExtraFee { LowerSpeed = 20, UpperSpeed = 1000, VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };

            _mockWindSpeedExtraFeeRepository.Setup(x => x.List())
                .ReturnsAsync(windSpeedFees
                .Where(x => x.VehicleType == type)
                .Where(x => x.LowerSpeed < speed)
                .Where(x => x.UpperSpeed > speed)
                .ToList());

            //Act
            var result = _service.GetWindSpeedFee(type, speed);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetBaseFee_return_right_fee_if_vehicle_and_station_is_right()
        {
            //Arrange
            var vehicle = VehicleEnum.Car;
            var station = StationEnum.Tallinn;
            var baseFees = new List<RegionalBaseFee>
            {
                new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = 4, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Scooter, StationName = StationEnum.Tallinn, Price = 3.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Bike, StationName = StationEnum.Tallinn, Price = 3, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tartu, Price = 3.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Scooter, StationName = StationEnum.Tartu, Price = 3, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Bike, StationName = StationEnum.Tartu, Price = 2.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Pärnu, Price = 3, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Scooter, StationName = StationEnum.Pärnu, Price = 2.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Bike, StationName = StationEnum.Pärnu, Price = 2, },
            };

            _mockRegionalBaseFeeRepository.Setup(x => x.List())
                .ReturnsAsync(baseFees
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.StationName == station)
                .ToList());

            //Act
            var result = _service.GetBaseFee(vehicle, station);

            //Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public void GetBaseFee_return_0_if_no_data_found()
        {
            //Arrange
            var vehicle = VehicleEnum.Car;
            var station = StationEnum.Tartu;

            _mockRegionalBaseFeeRepository.Setup(x => x.List())
                .ReturnsAsync([]);

            //Act
            var result = _service.GetBaseFee(vehicle, station);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetStationWeather_return_weather_data_if_station_name_is_right()
        {
            //Arrange
            string station = "Tallinn-Harku";

            var weatherData = new List<StationWeather>
            {
                new StationWeather {Id = 0, StationName = "Tallinn-Harku", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080412},
                new StationWeather {Id = 1, StationName = "Tallinn-Harku", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080413},
                new StationWeather {Id = 2, StationName = "Tallinn-Harku", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080414},

            };
            
            _mockStationWeatherRepository.Setup(x => x.List())
                .ReturnsAsync(weatherData
                .Where(x => x.StationName == station)
                .Where(x => x.Timestamp < DateTime.Now.ToLong())
                .OrderByDescending(x => x.Timestamp)
                .ToList());

            //Act
            var result = _service.GetStationWeather(StationEnum.Tallinn);

            //Assert
            Assert.Equal(weatherData[2], result);
        }

        [Fact]
        public void GetStationWeather_return_weather_data_if_station_name_for_Tartu_is_right()
        {
            //Arrange
            string station = "Tartu-Tõravere";

            var weatherData = new List<StationWeather>
            {
                new StationWeather {Id = 0, StationName = "Tartu-Tõravere", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080412},
                new StationWeather {Id = 1, StationName = "Tartu-Tõravere", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080413},
                new StationWeather {Id = 2, StationName = "Tartu-Tõravere", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080414},

            };

            _mockStationWeatherRepository.Setup(x => x.List())
                .ReturnsAsync(weatherData
                .Where(x => x.StationName == station)
                .Where(x => x.Timestamp < DateTime.Now.ToLong())
                .OrderByDescending(x => x.Timestamp)
                .ToList());

            //Act
            var result = _service.GetStationWeather(StationEnum.Tartu);

            //Assert
            Assert.Equal(weatherData[2], result);
        }

        [Fact]
        public void GetStationWeather_return_weather_data_if_station_name_for_Pärnu_is_right()
        {
            //Arrange
            string station = "Pärnu";

            var weatherData = new List<StationWeather>
            {
                new StationWeather {Id = 0, StationName = "Pärnu", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080412},
                new StationWeather {Id = 1, StationName = "Pärnu", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080413},
                new StationWeather {Id = 2, StationName = "Pärnu", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080414},

            };

            _mockStationWeatherRepository.Setup(x => x.List())
                .ReturnsAsync(weatherData
                .Where(x => x.StationName == station)
                .Where(x => x.Timestamp < DateTime.Now.ToLong())
                .OrderByDescending(x => x.Timestamp)
                .ToList());

            //Act
            var result = _service.GetStationWeather(StationEnum.Pärnu);

            //Assert
            Assert.Equal(weatherData[2], result);
        }

        [Fact]
        public void GetStationWeather_return_exception_if_station_name_is_empty_string()
        {
            //Arrange
            string station = "";

            var weatherData = new List<StationWeather>
            {
                new StationWeather {Id = 0, StationName = "Pärnu", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080412},
                new StationWeather {Id = 1, StationName = "Pärnu", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080413},
                new StationWeather {Id = 2, StationName = "Pärnu", VmoCode = 23553, AirTemp = 20, WindSpeed = 6, WeatherPhenomenon = "Clear", Timestamp = 638779064558080414},

            };

            _mockStationWeatherRepository.Setup(x => x.List())
                .ThrowsAsync(null);

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _service.GetStationWeather(null));

            _mockLogger.Verify(
              x => x.Log(
                  LogLevel.Error,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error occurred while retrieving station weather: Failed to retrieve station weather.")),
                  It.IsAny<ArgumentNullException>(),
                  It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
              Times.Once);
        }

        [Fact]
        public void ConvertStationEnumToName_return_empty_string()
        {
            //Act
            var result = _service.ConvertStationEnumToName(null);

            //Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ConvertStationNameToEnum_return_Tallinn_enum_if_station_string_right()
        {
            //Arrange
            string station = "Tallinn";

            //Act
            var result = _service.ConvertStationNameToEnum(station);

            //Assert
            Assert.Equal(StationEnum.Tallinn, result);
        }

        [Fact]
        public void ConvertStationNameToEnum_return_Tartu_enum_if_station_string_right()
        {
            //Arrange
            string station = "Tartu";

            //Act
            var result = _service.ConvertStationNameToEnum(station);

            //Assert
            Assert.Equal(StationEnum.Tartu, result);
        }

        [Fact]
        public void ConvertStationNameToEnum_return_Pärnu_enum_if_station_string_right()
        {
            //Arrange
            string station = "Pärnu";

            //Act
            var result = _service.ConvertStationNameToEnum(station);

            //Assert
            Assert.Equal(StationEnum.Pärnu, result);
        }

        [Fact]
        public void ConvertStationNameToEnum_return_null_if_station_string_wrong()
        {
            //Arrange
            string station = "Toronto";

            //Act
            var result = _service.ConvertStationNameToEnum(station);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void ConvertVehicleTypeToEnum_return_bike_enum_if_vehicle_string_right()
        {
            //Arrange
            string vehicle = "Bike";

            //Act
            var result = _service.ConvertVehicleTypeToEnum(vehicle);

            //Assert
            Assert.Equal(VehicleEnum.Bike, result);
        }

        [Fact]
        public void ConvertVehicleTypeToEnum_return_scooter_enum_if_vehicle_string_right()
        {
            //Arrange
            string vehicle = "Scooter";

            //Act
            var result = _service.ConvertVehicleTypeToEnum(vehicle);

            //Assert
            Assert.Equal(VehicleEnum.Scooter, result);
        }

        [Fact]
        public void ConvertVehicleTypeToEnum_return_car_enum_if_vehicle_string_right()
        {
            //Arrange
            string vehicle = "Car";

            //Act
            var result = _service.ConvertVehicleTypeToEnum(vehicle);

            //Assert
            Assert.Equal(VehicleEnum.Car, result);
        }

        [Fact]
        public void ConvertVehicleTypeToEnum_return_null_if_vehicle_string_wrong()
        {
            //Arrange
            string vehicle = "Legs";

            //Act
            var result = _service.ConvertVehicleTypeToEnum(vehicle);

            //Assert
            Assert.Equal(null, result);
        }
    }
}
