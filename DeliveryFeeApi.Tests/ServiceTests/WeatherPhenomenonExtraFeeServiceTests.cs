using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.DeliveryFeeApi.Tests.ServiceTests
{
    [ExcludeFromCodeCoverage]
    public class WeatherPhenomenonExtraFeeServiceTests
    {
        private readonly Mock<IWeatherPhenomenonExtraFeeRepository> _mockRepo;
        private readonly Mock<ILogger<WeatherPhenomenonExtraFeeService>> _logger;
        private readonly WeatherPhenomenonExtraFeeService _service;

        public WeatherPhenomenonExtraFeeServiceTests()
        {
            _mockRepo = new Mock<IWeatherPhenomenonExtraFeeRepository>();
            _logger = new Mock<ILogger<WeatherPhenomenonExtraFeeService>>();
            _service = new WeatherPhenomenonExtraFeeService(_mockRepo.Object, _logger.Object );
        }

        [Fact]
        public void FindAll_return_empty_list_of_objects()
        {
            //Arrange
            _mockRepo.Setup(x => x.List()).ReturnsAsync(new List<WeatherPhenomenonExtraFee>());

            //Act
            var result = _service.FindAll();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FindByVehicleTypeAndPhenomenon_return_null_if_fee_not_found()
        {
            //Arrange
            var vehicle = VehicleEnum.Bike;
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

            _mockRepo.Setup(x => x.List())
                .ReturnsAsync(weatherPhenomenonFees
                                .Where(x => x.VehicleType == vehicle)
                                .Where(x => x.WeatherPhenomenon == phenomenon)
                                .ToList());

            //Act
            var result = _service.FindByVehicleTypeAndPhenomenon(phenomenon, vehicle);

            //Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void FindByVehicleTypeAndPhenomenon_return_fee_if_fee()
        {
            //Arrange
            var vehicle = VehicleEnum.Bike;
            string phenomenon = "Light rain";
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

            _mockRepo.Setup(x => x.List())
                .ReturnsAsync(weatherPhenomenonFees
                                .Where(x => x.VehicleType == vehicle)
                                .Where(x => x.WeatherPhenomenon == phenomenon)
                                .ToList());

            //Act
            var result = _service.FindByVehicleTypeAndPhenomenon(phenomenon, vehicle);

            //Assert
            Assert.Equal(weatherPhenomenonFees[1], result);
        }

        [Fact]
        public async Task UpdateFee_return_updated_fee()
        {
            //Arrange
            decimal? price = 1;
            bool? forbitten = false;
            var fee = new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Bike, Forbitten = true, };

            var updatedFee = new WeatherPhenomenonExtraFee { WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Bike, Price = price, Forbitten = forbitten, };

            _mockRepo.Setup(x => x.Update(fee, price, forbitten)).ReturnsAsync(updatedFee);

            //Act
            var result = await _service.UpdateFee(fee, price, forbitten);

            //Assert
            Assert.Equal(updatedFee, result);

        }

        [Fact]
        public async Task CreateFee_return_created_fee()
        {
            //Arrange
            string phenomenon = "Hail";
            var vehicle = VehicleEnum.Bike;
            bool? forbitten = true;
            var fee = new WeatherPhenomenonExtraFee { WeatherPhenomenon = phenomenon, VehicleType = vehicle, Forbitten = forbitten, };

            _mockRepo.Setup(x => x.Save(fee)).Verifiable();

            //Act
            var result = await _service.CreateFee(vehicle, phenomenon, null, forbitten);

            //Assert
            _mockRepo.Verify(r => r.Save(It.Is<WeatherPhenomenonExtraFee>(x => x.Forbitten == forbitten)), Times.Once);
        }

        [Fact]
        public async Task DeleteFee_return_null_if_fee_not_found()
        {
            //Arrange
            var id = 10;
            _mockRepo.Setup(x => x.FindById(id)).ReturnsAsync((WeatherPhenomenonExtraFee?)null);

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
            var fee = new WeatherPhenomenonExtraFee { Id = 2, WeatherPhenomenon = "Hail", VehicleType = VehicleEnum.Bike, Forbitten = true, };
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
    }
}
