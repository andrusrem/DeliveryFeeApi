using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ServiceTests
{
    public class AirTemperatureExtraFeeServiceTests
    {
        private readonly Mock<IAirTemperatureExtraFeeRepository> _mockAirTemperatureExtraFeeRepository;
        private readonly Mock<ILogger<AirTemperatureExtraFeeService>> _mockLogger;
        private readonly AirTemperatureExtraFeeService _service;

        public AirTemperatureExtraFeeServiceTests()
        {
            _mockLogger = new Mock<ILogger<AirTemperatureExtraFeeService>>();
            _mockAirTemperatureExtraFeeRepository = new Mock<IAirTemperatureExtraFeeRepository>();
            _service = new AirTemperatureExtraFeeService(_mockAirTemperatureExtraFeeRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void FindAll_return_empty_list_of_objects()
        {
            //Arrange
            _mockAirTemperatureExtraFeeRepository.Setup(x => x.List()).ReturnsAsync(new List<AirTemperatureExtraFee>());

            //Act
            var result = _service.FindAll();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FindByVehicleTypeLowerAndUpperPoints_return_null_if_fee_not_found()
        {
            //Arrange
            var vehicle = VehicleEnum.Scooter;
            decimal lowerTemperature = 2;
            decimal upperTemperature = 10;
            var airTempFees = new List<AirTemperatureExtraFee>
            {
               new AirTemperatureExtraFee{Id = 0, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Scooter, Price = 1},
               new AirTemperatureExtraFee{Id = 1, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Bike, Price = 1},
               new AirTemperatureExtraFee{Id = 2, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 0.5m},
               new AirTemperatureExtraFee{Id = 3, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Bike, Price = 0.5m},
            };
            _mockAirTemperatureExtraFeeRepository.Setup(x => x.List()).ReturnsAsync(airTempFees
                .Where(x => x.LowerTemperature == lowerTemperature)
                .Where(x => x.UpperTemperature == upperTemperature)
                .Where(x => x.VehicleType == vehicle).ToList());


            //Act
            var result = _service.FindByVehicleTypeLowerAndUpperPoints(lowerTemperature, upperTemperature, vehicle);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void FindByVehicleTypeLowerAndUpperSpeed_return_fee_if_fee()
        {
            //Arrange
            var vehicle = VehicleEnum.Scooter;
            decimal lowerTemperature = -10;
            decimal upperTemperature = 0;
            var airTempFees = new List<AirTemperatureExtraFee>
            {
               new AirTemperatureExtraFee{Id = 0, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Scooter, Price = 1},
               new AirTemperatureExtraFee{Id = 1, LowerTemperature =  -273.15m, UpperTemperature = -10, VehicleType = VehicleEnum.Bike, Price = 1},
               new AirTemperatureExtraFee{Id = 2, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 0.5m},
               new AirTemperatureExtraFee{Id = 3, LowerTemperature =  -10, UpperTemperature = 0, VehicleType = VehicleEnum.Bike, Price = 0.5m},
            };
            _mockAirTemperatureExtraFeeRepository.Setup(x => x.List()).ReturnsAsync(airTempFees
                .Where(x => x.LowerTemperature == lowerTemperature)
                .Where(x => x.UpperTemperature == upperTemperature)
                .Where(x => x.VehicleType == vehicle).ToList());

            //Act
            var result = _service.FindByVehicleTypeLowerAndUpperPoints(lowerTemperature, upperTemperature, vehicle);

            //Assert
            Assert.Equal(airTempFees[2], result);
        }

        [Fact]
        public async Task UpdateFee_return_updated_fee()
        {
            //Arrange
            decimal price = 1;
            var fee = new AirTemperatureExtraFee { Id = 2, LowerTemperature = -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 0.5m };

            var updatedFee = new AirTemperatureExtraFee { Id = 2, LowerTemperature = -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 1 };

            _mockAirTemperatureExtraFeeRepository.Setup(x => x.Update(fee, price)).ReturnsAsync(updatedFee);

            //Act
            var result = await _service.UpdateFee(fee, price);

            //Assert
            Assert.Equal(updatedFee, result);

        }

        [Fact]
        public async Task CreateFee_return_created_fee()
        {
            //Arrange
            decimal lowerTemperature = -10;
            decimal upperTemperature = 0;
            decimal price = 1;
            var vehicle = VehicleEnum.Bike;
            var fee = new AirTemperatureExtraFee { LowerTemperature = lowerTemperature, UpperTemperature = upperTemperature, VehicleType = vehicle, Price = price };


            _mockAirTemperatureExtraFeeRepository.Setup(x => x.Save(fee)).Verifiable();

            //Act
            var result = await _service.CreateFee(vehicle, lowerTemperature, upperTemperature, price);

            //Assert
            _mockAirTemperatureExtraFeeRepository.Verify(r => r.Save(It.Is<AirTemperatureExtraFee>(x => x.Price == 1)), Times.Once);
        }

        [Fact]
        public async Task DeleteFee_return_null_if_fee_not_found()
        {
            //Arrange
            var id = 10;
            _mockAirTemperatureExtraFeeRepository.Setup(x => x.FindById(id)).ReturnsAsync((AirTemperatureExtraFee?)null);

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
            var fee = new AirTemperatureExtraFee { Id = 2, LowerTemperature = -10, UpperTemperature = 0, VehicleType = VehicleEnum.Scooter, Price = 0.5m };
            _mockAirTemperatureExtraFeeRepository.Setup(x => x.FindById(id)).ReturnsAsync(fee);
            _mockAirTemperatureExtraFeeRepository.Setup(x => x.DeleteFee(fee)).Verifiable();

            //Act
            var result = await _service.DeleteFee(id);

            //Assert
            _mockAirTemperatureExtraFeeRepository.Verify(r => r.DeleteFee(fee), Times.Once);
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
