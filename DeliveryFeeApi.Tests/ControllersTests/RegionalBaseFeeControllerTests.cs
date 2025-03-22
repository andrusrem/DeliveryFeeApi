using DeliveryFeeApi.Controllers;
using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static System.Collections.Specialized.BitVector32;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ControllersTests
{
    [ExcludeFromCodeCoverage]
    public class RegionalBaseFeeControllerTests
    {
        private readonly Mock<IRegionalBaseFeeService> _mockService;
        private readonly RegionalBaseFeeController _controller;

        public RegionalBaseFeeControllerTests()
        {
            _mockService = new Mock<IRegionalBaseFeeService>();
            _controller = new RegionalBaseFeeController( _mockService.Object );
        }

        [Fact]
        public void GetAll_return_Ok_result_and_list_of_objects()
        {
            //Arrange
            _mockService.Setup(x => x.FindAll()).Returns(new List<RegionalBaseFee>());

            //Act
            var result = _controller.GetAll();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_BadRequest_if_stationEnum_is_null()
        {
            //Arrange
            var vehicle = "Car";
            var station = "Porvo";
            decimal price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns((StationEnum?)null);

            //Act
            var result = _controller.UpdateFee(vehicle, station, price);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_BadRequest_if_vehicleEnum_is_null()
        {
            //Arrange
            var vehicle = "Legs";
            var station = "Tallinn";
            decimal price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns((VehicleEnum?)null);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(StationEnum.Tallinn);

            //Act
            var result = _controller.UpdateFee(vehicle, station, price);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_NotFound_if_fee_not_found_in_database()
        {
            //Arrange
            var vehicle = "Car";
            var station = "Tallinn";
            decimal price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(StationEnum.Tallinn);
            _mockService.Setup(x => x.FindByVehicleTypeAndStationName(StationEnum.Tallinn, VehicleEnum.Car)).Returns((RegionalBaseFee?)null);

            //Act
            var result = _controller.UpdateFee(vehicle, station, price);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateFee_return_Ok_if_fee_found_in_database_and_updated()
        {
            //Arrange
            var vehicle = "Car";
            var station = "Tallinn";
            decimal price = 1;
            var fee = new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = 4, };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(StationEnum.Tallinn);
            _mockService.Setup(x => x.FindByVehicleTypeAndStationName(StationEnum.Tallinn, VehicleEnum.Car)).Returns(fee);
            _mockService.Setup(x => x.UpdateFee(fee, price)).Verifiable();

            //Act
            var result = _controller.UpdateFee(vehicle, station, price);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void CreateFee_return_BadRequest_if_stationEnum_is_null()
        {
            //Arrange
            var vehicle = "Car";
            var station = "Porvo";
            decimal price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns((StationEnum?)null);

            //Act
            var result = _controller.CreateFee(vehicle, station, price);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFee_return_BadRequest_if_vehicleEnum_is_null()
        {
            //Arrange
            var vehicle = "Legs";
            var station = "Tallinn";
            decimal price = 1;
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns((VehicleEnum?)null);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(StationEnum.Tallinn);

            //Act
            var result = _controller.CreateFee(vehicle, station, price);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFee_return_BadRequest_if_fee_found_in_database()
        {
            //Arrange
            var vehicle = "Car";
            var station = "Tallinn";
            decimal price = 1;
            var fee = new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = 4, };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(StationEnum.Tallinn);
            _mockService.Setup(x => x.FindByVehicleTypeAndStationName(StationEnum.Tallinn, VehicleEnum.Car)).Returns(fee);

            //Act
            var result = _controller.CreateFee(vehicle, station, price);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateFee_return_CreateAtAction_if_fee_not_found_in_database_and_created()
        {
            //Arrange
            var vehicle = "Car";
            var station = "Tallinn";
            decimal price = 1;
            var fee = new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = 4, };
            _mockService.Setup(x => x.ConvertVehicleTypeToEnum(vehicle)).Returns(VehicleEnum.Car);
            _mockService.Setup(x => x.ConvertStationNameToEnum(station)).Returns(StationEnum.Tallinn);
            _mockService.Setup(x => x.FindByVehicleTypeAndStationName(StationEnum.Tallinn, VehicleEnum.Car)).Returns((RegionalBaseFee?)null);
            _mockService.Setup(x => x.CreateFee(VehicleEnum.Car, StationEnum.Tallinn, price)).ReturnsAsync(fee);
            //Act
            var result = _controller.CreateFee(vehicle, station, price);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async void DeleteFee_return_BadRequest_if_function_return_null()
        {
            //Arrange
            var id = 1;
            _mockService.Setup(x => x.DeleteFee(id)).ReturnsAsync((RegionalBaseFee?)null);

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
            var fee = new RegionalBaseFee { Id = 1, VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = 4, };
            _mockService.Setup(x => x.DeleteFee(id)).ReturnsAsync(fee);

            //Act
            var result = await _controller.DeleteFee(id);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
