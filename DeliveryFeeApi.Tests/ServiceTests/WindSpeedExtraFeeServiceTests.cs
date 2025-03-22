using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ServiceTests
{
    public class WindSpeedExtraFeeServiceTests
    {
        private readonly Mock<IWindSpeedExtraFeeRepository> _mockWindSpeedExtraFeeRepository;
        private readonly Mock<ILogger<WindSpeedExtraFeeService>> _mockLogger;
        private readonly WindSpeedExtraFeeService _service;

        public WindSpeedExtraFeeServiceTests()
        {
            _mockLogger = new Mock<ILogger<WindSpeedExtraFeeService>>();
            _mockWindSpeedExtraFeeRepository = new Mock<IWindSpeedExtraFeeRepository>();
            _service = new WindSpeedExtraFeeService(_mockWindSpeedExtraFeeRepository.Object, _mockLogger.Object );
        }

        [Fact]
        public void FindAll_return_empty_list_of_objects()
        {
            //Arrange
            _mockWindSpeedExtraFeeRepository.Setup(x => x.List()).ReturnsAsync(new List<WindSpeedExtraFee>());

            //Act
            var result = _service.FindAll();

            //Assert
            Assert.Empty( result );
        }

        [Fact]
        public void FindByVehicleTypeLowerAndUpperSpeed_return_null_if_fee_not_found()
        {
            //Arrange
            decimal lowerSpeed = -10;
            decimal? upperSpeed = 10;
            var vehicle = VehicleEnum.Car;
            var windSpeedFees = new List<WindSpeedExtraFee> {
                new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, },
                new WindSpeedExtraFee { LowerSpeed = 20, UpperSpeed = 1000, VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };
            _mockWindSpeedExtraFeeRepository.Setup(x => x.List()).ReturnsAsync(windSpeedFees
                .Where(x => x.LowerSpeed == lowerSpeed)
                .Where(x => x.UpperSpeed == upperSpeed)
                .Where(x => x.VehicleType == vehicle).ToList());
               

            //Act
            var result = _service.FindByVehicleTypeLowerAndUpperSpeed(lowerSpeed, upperSpeed, vehicle);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void FindByVehicleTypeLowerAndUpperSpeed_return_fee_if_fee()
        {
            //Arrange
            decimal lowerSpeed = 10;
            decimal? upperSpeed = 20;
            var vehicle = VehicleEnum.Bike;
            var windSpeedFees = new List<WindSpeedExtraFee> {
                new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, },
                new WindSpeedExtraFee { LowerSpeed = 20, UpperSpeed = 1000, VehicleType = VehicleEnum.Bike, Forbitten = true, },
            };
            _mockWindSpeedExtraFeeRepository.Setup(x => x.List()).ReturnsAsync(windSpeedFees
                .Where(x => x.LowerSpeed == lowerSpeed)
                .Where(x => x.UpperSpeed == upperSpeed)
                .Where(x => x.VehicleType == vehicle)
                .ToList());


            //Act
            var result = _service.FindByVehicleTypeLowerAndUpperSpeed(lowerSpeed, upperSpeed, vehicle);

            //Assert
            Assert.Equal(windSpeedFees[0], result);
        }

        [Fact]
        public async Task UpdateFee_return_updated_fee()
        {
            //Arrange
            decimal? price = 1;
            var fee = new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 0.5M, };

            var updatedFee = new WindSpeedExtraFee { LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Bike, Price = 1, };

            _mockWindSpeedExtraFeeRepository.Setup(x => x.Update(fee, price, null)).ReturnsAsync(updatedFee);

            //Act
            var result = await _service.UpdateFee(fee, price, null);

            //Assert
            Assert.Equal(updatedFee, result);

        }

        [Fact]
        public async Task CreateFee_return_created_fee()
        {
            //Arrange
            decimal lowerSpeed = 20;
            decimal? upperSpeed = 30;
            decimal? price = 1;
            var vehicle = VehicleEnum.Bike;
            var fee = new WindSpeedExtraFee { LowerSpeed =  lowerSpeed, UpperSpeed = upperSpeed, VehicleType = vehicle, Price = price, Forbitten = false };

            _mockWindSpeedExtraFeeRepository.Setup(x => x.Save(fee)).Verifiable();

            //Act
            var result = await _service.CreateFee(vehicle, lowerSpeed, upperSpeed, price, null);

            //Assert
            _mockWindSpeedExtraFeeRepository.Verify(r => r.Save(It.Is<WindSpeedExtraFee>(x => x.Price == 1)), Times.Once);
        }

        [Fact]
        public async Task DeleteFee_return_null_if_fee_not_found()
        {
            //Arrange
            var id = 10;
            _mockWindSpeedExtraFeeRepository.Setup(x => x.FindById(id)).ReturnsAsync((WindSpeedExtraFee?)null);

            //Act
            var result = await _service.DeleteFee(id);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public async Task DeleteFee_return_verify_that_delete_runs_once()
        {
            //Arrange
            var id = 10;
            var fee = new WindSpeedExtraFee {Id = id, LowerSpeed = 10, UpperSpeed = 20, VehicleType = VehicleEnum.Car, Price = 1, Forbitten = false };
            _mockWindSpeedExtraFeeRepository.Setup(x => x.FindById(id)).ReturnsAsync(fee);
            _mockWindSpeedExtraFeeRepository.Setup(x => x.DeleteFee(fee)).Verifiable();

            //Act
            var result = await _service.DeleteFee(id);

            //Assert
            _mockWindSpeedExtraFeeRepository.Verify(r => r.DeleteFee(fee), Times.Once);
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
