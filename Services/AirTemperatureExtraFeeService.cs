using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace DeliveryFeeApi.Services
{
    public class AirTemperatureExtraFeeService(
       IAirTemperatureExtraFeeRepository airTemperatureExtraFeeRepository,
       ILogger<AirTemperatureExtraFeeService> logger
        ) : IAirTemperatureExtraFeeService
    {
        private readonly IAirTemperatureExtraFeeRepository _airTemperatureExtraFeeRepository = airTemperatureExtraFeeRepository;
        private readonly ILogger<AirTemperatureExtraFeeService> _logger = logger;
            
        public List<AirTemperatureExtraFee> FindAll()
        {
            var allFees = _airTemperatureExtraFeeRepository.List().Result;
            return allFees;
        }
        public AirTemperatureExtraFee? FindByVehicleTypeLowerAndUpperPoints(decimal lowerPoint, decimal upperPoint, VehicleEnum vehicle)
        {
            var airFee = _airTemperatureExtraFeeRepository.List().Result
                .Where(x => x.LowerTemperature == lowerPoint)
                .Where(x => x.UpperTemperature == upperPoint)
                .Where(x => x.VehicleType == vehicle)
                .FirstOrDefault();

            if(airFee == null)
            {
                var error = "Could not find AirTemperatureFee with this parameters.";
                _logger.LogError(error);
                return null;
            }
            _logger.LogInformation("AirTemperatureFee is founded.");
            return airFee;
        }

        public async Task<AirTemperatureExtraFee?> UpdateFee(AirTemperatureExtraFee airTemperatureExtraFee, decimal price)
        {
            var updatedFee = await _airTemperatureExtraFeeRepository.Update(airTemperatureExtraFee, price);
            _logger.LogInformation("AirTemperatureFee is updated.");
            return updatedFee;
        }

        public async Task<AirTemperatureExtraFee?> CreateFee(VehicleEnum vehicle, decimal lower, decimal upper, decimal price)
        { 
            var fee = new AirTemperatureExtraFee { LowerTemperature = lower, UpperTemperature = upper, VehicleType = vehicle, Price = price };
            var createdFee = await _airTemperatureExtraFeeRepository.Save(fee);
            _logger.LogInformation("AirTemperatureFee is created.");
            return createdFee;
        }

        public async Task<AirTemperatureExtraFee?> DeleteFee(int id)
        {
            var fee = await _airTemperatureExtraFeeRepository.FindById(id);
            if (fee != null)
            {
                await _airTemperatureExtraFeeRepository.DeleteFee(fee);
                _logger.LogInformation("AirTemperatureFee was deleted.");
            }
            return null;
        }

        public VehicleEnum? ConvertVehicleTypeToEnum(string vehicle)
        {
            if (vehicle.ToLower() == "bike")
            {
                return VehicleEnum.Bike;
            }
            else if (vehicle.ToLower() == "car")
            {
                return VehicleEnum.Car;
            }
            else if (vehicle.ToLower() == "scooter")
            {
                return VehicleEnum.Scooter;
            }
            return null;
        }

    }
}
