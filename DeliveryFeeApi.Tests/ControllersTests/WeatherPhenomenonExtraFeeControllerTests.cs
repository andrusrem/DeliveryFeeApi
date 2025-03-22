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
    public class WeatherPhenomenonExtraFeeControllerTests
    {
        private readonly Mock<IWeatherPhenomenonExtraFeeService> _mockService;
        private readonly WeatherPhenomenonExtraFeeController _controller;

        public WeatherPhenomenonExtraFeeControllerTests()
        {
            _mockService = new Mock<IWeatherPhenomenonExtraFeeService>();
            _controller = new WeatherPhenomenonExtraFeeController(_mockService.Object);
        }

        [Fact]
        public void GetAll_return_Ok_result_and_list_of_objects()
        {
            //Arrange
            _mockService.Setup(x => x.FindAll()).Returns(new List<WeatherPhenomenonExtraFee>());

            //Act
            var result = _controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_BadRequest_if_vehicleEnum_is_null()
        {
            //Arrange
            var vehicle = "Legs";
            string phenomenon = "Clear";
            decimal? price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns((VehicleEnum?)null);

            //Act
            var result = _controller.UpdateFee(vehicle, phenomenon, price, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_NotFound_if_fee_not_found_in_database()
        {
            //Arrange
            var vehicle = "Car";
            string phenomenon = "Clear";
            decimal? price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeAndPhenomenon(phenomenon, VehicleEnum.Car)).Returns((WeatherPhenomenonExtraFee?)null);

            //Act
            var result = _controller.UpdateFee(vehicle, phenomenon, price, null);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_Ok_if_fee_found_in_database_and_updated()
        {
            //Arrange
            var vehicle = "Car";
            string phenomenon = "Light rain";
            decimal? price = 1;
            var fee = new WeatherPhenomenonExtraFee { Id = 1, WeatherPhenomenon = phenomenon, Price = 0.5m, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeAndPhenomenon(phenomenon, VehicleEnum.Car)).Returns(fee);
            _mockService.Setup(x => x.UpdateFee(fee, price, null)).Verifiable();

            //Act
            var result = _controller.UpdateFee(vehicle, phenomenon, price, null);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void CreateFee_return_BadRequest_if_vehicleEnum_is_null()
        {
            //Arrange
            var vehicle = "Legs";
            string phenomenon = "Light rain";
            decimal? price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns((VehicleEnum?)null);

            //Act
            var result = _controller.CreateFee(vehicle, phenomenon, price, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFee_return_BadRequest_if_fee_found_in_database()
        {
            //Arrange
            var vehicle = "Car";
            string phenomenon = "Light rain";
            decimal? price = 1;
            var fee = new WeatherPhenomenonExtraFee { Id = 1, WeatherPhenomenon = phenomenon, Price = 0.5m, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeAndPhenomenon(phenomenon, VehicleEnum.Car)).Returns(fee);

            //Act
            var result = _controller.CreateFee(vehicle, phenomenon, price, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFee_return_CreateAtAction_if_fee_not_found_in_database_and_created()
        {
            //Arrange
            var vehicle = "Car";
            string phenomenon = "Light rain";
            decimal? price = 1;
            var fee = new WeatherPhenomenonExtraFee { Id = 1, WeatherPhenomenon = phenomenon, Price = price, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeAndPhenomenon(phenomenon, VehicleEnum.Car)).Returns((WeatherPhenomenonExtraFee?)null);
            _mockService.Setup(x => x.CreateFee(VehicleEnum.Car, phenomenon, price, null)).ReturnsAsync(fee);

            //Act
            var result = _controller.CreateFee(vehicle, phenomenon, price, null);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async void DeleteFee_return_BadRequest_if_function_return_null()
        {
            //Arrange
            var id = 1;
            _mockService.Setup(x => x.DeleteFee(id)).ReturnsAsync((WeatherPhenomenonExtraFee?)null);

            //Act
            var result = await _controller.DeleteFee(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void DeleteFee_return_NoContent_if_function_return()
        {
            //Arrange
            var id = 1;
            var fee = new WeatherPhenomenonExtraFee { Id = 1, WeatherPhenomenon = "Heavy snowfall", Price = 1, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.DeleteFee(id)).ReturnsAsync(fee);

            //Act
            var result = await _controller.DeleteFee(id);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
