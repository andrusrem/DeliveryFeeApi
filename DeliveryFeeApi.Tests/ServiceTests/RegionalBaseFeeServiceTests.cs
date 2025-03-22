using System.Diagnostics.CodeAnalysis;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using DeliveryFeeApi.Repository;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ServiceTests
{
    [ExcludeFromCodeCoverage]
    public class RegionalBaseFeeServiceTests
    {
        private readonly Mock<ILogger<RegionalBaseFeeService>> _mockLogger;
        private readonly RegionalBaseFeeService _service;
        private readonly Mock<IRegionalBaseFeeRepository> _mockRepo;

        public RegionalBaseFeeServiceTests()
        {
            _mockLogger = new Mock<ILogger<RegionalBaseFeeService>>();
            _mockRepo = new Mock<IRegionalBaseFeeRepository>();
            _service = new RegionalBaseFeeService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public void FindAll_return_empty_list_of_objects()
        {
            //Arrange
            _mockRepo.Setup(x => x.List()).ReturnsAsync(new List<RegionalBaseFee>());

            //Act
            var result = _service.FindAll();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FindByVehicleTypeAndStationName_return_null_if_fee_not_found()
        {
            //Arrange
            var station = StationEnum.Tallinn;
            var vehicle = VehicleEnum.Car;
            var baseFees = new List<RegionalBaseFee>
            {
                new RegionalBaseFee { VehicleType = VehicleEnum.Scooter, StationName = StationEnum.Tallinn, Price = 3.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Bike, StationName = StationEnum.Tallinn, Price = 3, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tartu, Price = 3.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Scooter, StationName = StationEnum.Tartu, Price = 3, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Bike, StationName = StationEnum.Tartu, Price = 2.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Pärnu, Price = 3, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Scooter, StationName = StationEnum.Pärnu, Price = 2.5M, },
                new RegionalBaseFee { VehicleType = VehicleEnum.Bike, StationName = StationEnum.Pärnu, Price = 2, },
            };

            _mockRepo.Setup(x => x.List())
                .ReturnsAsync(baseFees
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.StationName == station)
                .ToList());


            //Act
            var result = _service.FindByVehicleTypeAndStationName(station, vehicle);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void FindByVehicleTypeAndStationName_return_fee_if_fee()
        {
            //Arrange
            var station = StationEnum.Tallinn;
            var vehicle = VehicleEnum.Car;
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

            _mockRepo.Setup(x => x.List())
                .ReturnsAsync(baseFees
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.StationName == station)
                .ToList());

            //Act
            var result = _service.FindByVehicleTypeAndStationName(station, vehicle);

            //Assert
            Assert.Equal(baseFees[0], result);
        }

        [Fact]
        public async Task UpdateFee_return_updated_fee()
        {
            //Arrange
            decimal price = 5;
            var fee = new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = 4, };

            var updatedFee = new RegionalBaseFee { VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = price, };

            _mockRepo.Setup(x => x.Update(fee, price)).ReturnsAsync(updatedFee);

            //Act
            var result = await _service.UpdateFee(fee, price);

            //Assert
            Assert.Equal(updatedFee, result);

        }

        [Fact]
        public async Task CreateFee_return_created_fee()
        {
            //Arrange
            var station = StationEnum.Tallinn;
            var vehicle = VehicleEnum.Car;
            decimal price = 4;
            var fee = new RegionalBaseFee { VehicleType = vehicle, StationName = station, Price = price, };

            _mockRepo.Setup(x => x.Save(fee)).Verifiable();

            //Act
            var result = await _service.CreateFee(vehicle, station, price);

            //Assert
            _mockRepo.Verify(r => r.Save(It.Is<RegionalBaseFee>(x => x.Price == price)), Times.Once);
        }

        [Fact]
        public async Task DeleteFee_return_null_if_fee_not_found()
        {
            //Arrange
            var id = 10;
            _mockRepo.Setup(x => x.FindById(id)).ReturnsAsync((RegionalBaseFee?)null);

            //Act
            var result = await _service.DeleteFee(id);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public async Task DeleteFee_return_verify_that_delete_runs_once()
        {
            //Arrange
            var id = 2;
            var fee = new RegionalBaseFee { Id = id, VehicleType = VehicleEnum.Car, StationName = StationEnum.Tallinn, Price = 4, };
            _mockRepo.Setup(x => x.FindById(id)).ReturnsAsync(fee);
            _mockRepo.Setup(x => x.DeleteFee(fee)).Verifiable();

            //Act
            var result = await _service.DeleteFee(id);

            //Assert
            _mockRepo.Verify(r => r.DeleteFee(fee), Times.Once);
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
    }
}
