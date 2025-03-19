using DeliveryFeeApi.Controllers;
using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class DeliveryPriceControllerTests
    {
        private readonly Mock<IDeliveryPriceService> _mockPriceService;
        private readonly DeliveryPriceController _controller;

        public DeliveryPriceControllerTests()
        {
            _mockPriceService = new Mock<IDeliveryPriceService>();
            _controller = new DeliveryPriceController( _mockPriceService.Object );
        }

        [Fact]
        public void GetDeliveryPrice_return_badreques_if_vehicle_input_is_invalid()
        {
            //Arrange
            var station = "Tallinn";
            var vehicle = "Right leg";

            _mockPriceService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(Data.StationEnum.Tallinn);
            _mockPriceService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns((VehicleEnum?)null);
            
            //Act
            var result = _controller.GetDeliveryPrice(station, vehicle).Result;
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetDeliveryPrice_return_badreques_if_station_input_is_invalid()
        {
            //Arrange
            var station = "Porvo";
            var vehicle = "Car";

            _mockPriceService.Setup(x => x.ConvertStationNameToEnum(station)).Returns((StationEnum?)null);
            _mockPriceService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);

            //Act
            var result = _controller.GetDeliveryPrice(station, vehicle).Result;
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetDeliveryPrice_return_forbitten_true_and_Ok_if_wind_speed_fee_is_null_and_vihicle_is_bike()
        {
            //Arrange
            var station = "Tallinn";
            var vehicle = "Bike";
            var tallinnBikeBaseFee = 3;
            var weather = new StationWeather { Id = 0, AirTemp = 10, WindSpeed = 22, WeatherPhenomenon = "Clear" };

            var response = new ResponseBody();
            response.Forbitten = true;

            _mockPriceService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(Data.StationEnum.Tallinn);
            _mockPriceService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Bike);

            _mockPriceService.Setup(x => x.GetStationWeather(StationEnum.Tallinn)).Returns(weather);
            _mockPriceService.Setup(x => x.GetBaseFee(VehicleEnum.Bike, StationEnum.Tallinn)).Returns(tallinnBikeBaseFee);
            _mockPriceService.Setup(x => x.GetWindSpeedFee(VehicleEnum.Bike, (decimal)weather.WindSpeed)).Returns((decimal?)null);
            _mockPriceService.Setup(x => x.GetWeatherPhenomenonFee(VehicleEnum.Bike, (string)weather.WeatherPhenomenon)).Returns(0);

            //Act
            var result = _controller.GetDeliveryPrice(station, vehicle).Result;
            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), objectResult.Value.ToString());
        }

        [Fact]
        public void GetDeliveryPrice_return_total_fee_and_Ok_if_all_input_is_valid()
        {
            //Arrange
            var station = "Tallinn";
            var vehicle = "Bike";
            var baseFee = 3.0m;
            var weather = new StationWeather { Id = 0, AirTemp = 10, WindSpeed = 15, WeatherPhenomenon = "Clear" };

            var response = new ResponseBody();
            var windSpeedFee = 0.5m;
            var airFee = 0m;
            var phenomenonFee = 0m;

            _mockPriceService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(Data.StationEnum.Tallinn);
            _mockPriceService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Bike);

            _mockPriceService.Setup(x => x.GetStationWeather(StationEnum.Tallinn)).Returns(weather);
            _mockPriceService.Setup(x => x.GetAirTemperatureFee(VehicleEnum.Bike, (decimal)weather.AirTemp)).Returns(airFee);
            _mockPriceService.Setup(x => x.GetBaseFee(VehicleEnum.Bike, StationEnum.Tallinn)).Returns(baseFee);
            _mockPriceService.Setup(x => x.GetWindSpeedFee(VehicleEnum.Bike, (decimal)weather.WindSpeed)).Returns(windSpeedFee);
            _mockPriceService.Setup(x => x.GetWeatherPhenomenonFee(VehicleEnum.Bike, (string)weather.WeatherPhenomenon)).Returns(phenomenonFee);

            var totalFee = baseFee + airFee + windSpeedFee + phenomenonFee;

            response.Total = totalFee;
            //Act
            var result = _controller.GetDeliveryPrice(station, vehicle).Result;
            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response.ToString(), objectResult.Value.ToString());
        }
    }
}
