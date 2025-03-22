using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace DeliveryFeeApi.Controllers
{
    [ExcludeFromCodeCoverage]
    public class WindSpeedExtraFeeControllerTests
    {
        private readonly Mock<IWindSpeedExtraFeeService> _mockService;
        private readonly WindSpeedExtraFeeController _controller;

        public WindSpeedExtraFeeControllerTests()
        {
            _mockService = new Mock<IWindSpeedExtraFeeService>();
            _controller = new WindSpeedExtraFeeController( _mockService.Object );
        }

        [Fact]
        public void GetAll_return_Ok_result_and_list_of_objects()
        {
            //Arrange
            _mockService.Setup(x => x.FindAll()).Returns(new List<WindSpeedExtraFee>());

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
            decimal lower = 10;
            decimal? upper = 20;
            decimal? price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns((VehicleEnum?)null);

            //Act
            var result = _controller.UpdateFee(vehicle, lower, upper, price, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_NotFound_if_fee_not_found_in_database()
        {
            //Arrange
            var vehicle = "Car";
            decimal lower = 10;
            decimal? upper = 20;
            decimal? price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeLowerAndUpperSpeed(lower, upper, VehicleEnum.Car)).Returns((WindSpeedExtraFee?)null);

            //Act
            var result = _controller.UpdateFee(vehicle, lower, upper, price, null);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_Ok_if_fee_found_in_database_and_updated()
        {
            //Arrange
            var vehicle = "Car";
            decimal lower = 10;
            decimal? upper = 20;
            decimal? price = 1;
            var fee = new WindSpeedExtraFee { Id = 1, LowerSpeed = lower, UpperSpeed = upper, Price = 0.5m, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeLowerAndUpperSpeed(lower, upper, VehicleEnum.Car)).Returns(fee);
            _mockService.Setup(x => x.UpdateFee(fee, price, null)).Verifiable();

            //Act
            var result = _controller.UpdateFee(vehicle, lower, upper, price, null);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void CreateFee_return_BadRequest_if_vehicleEnum_is_null()
        {
            //Arrange
            var vehicle = "Legs";
            decimal lower = 10;
            decimal? upper = 20;
            decimal? price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns((VehicleEnum?)null);

            //Act
            var result = _controller.CreateFee(vehicle, lower, upper, price, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFee_return_BadRequest_if_fee_found_in_database()
        {
            //Arrange
            var vehicle = "Car";
            decimal lower = 10;
            decimal? upper = 20;
            decimal? price = 1;
            var fee = new WindSpeedExtraFee { Id = 1, LowerSpeed = lower, UpperSpeed = upper, Price = price, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeLowerAndUpperSpeed(lower, upper, VehicleEnum.Car)).Returns(fee);

            //Act
            var result = _controller.CreateFee(vehicle, lower, upper, price, null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFee_return_CreateAtAction_if_fee_not_found_in_database_and_created()
        {
            //Arrange
            var vehicle = "Car";
            decimal lower = 10;
            decimal? upper = 20;
            decimal? price = 1;
            var fee = new WindSpeedExtraFee { Id = 1, LowerSpeed = lower, UpperSpeed = upper, Price = price, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.FindByVehicleTypeLowerAndUpperSpeed(lower, upper, VehicleEnum.Car)).Returns((WindSpeedExtraFee?)null);
            _mockService.Setup(x => x.CreateFee(VehicleEnum.Car, lower, upper, price, null)).ReturnsAsync(fee);

            //Act
            var result = _controller.CreateFee(vehicle, lower, upper, price, null);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async void DeleteFee_return_BadRequest_if_function_return_null()
        {
            //Arrange
            var id = 1;
            _mockService.Setup(x => x.DeleteFee(id)).ReturnsAsync((WindSpeedExtraFee?)null);

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
            var fee = new WindSpeedExtraFee { Id = 1, LowerSpeed = 10, UpperSpeed = 20, Price = 1, VehicleType = VehicleEnum.Car };
            _mockService.Setup(x => x.DeleteFee(id)).ReturnsAsync(fee);

            //Act
            var result = await _controller.DeleteFee(id);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
