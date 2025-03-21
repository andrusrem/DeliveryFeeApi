using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;

namespace DeliveryFeeApi.Services
{
    public class WindSpeedExtraFeeService(
        IWindSpeedExtraFeeRepository windSpeedExtraFeeRepository,
        ILogger<WindSpeedExtraFeeService> logger) : IWindSpeedExtraFeeService
    {
        private readonly IWindSpeedExtraFeeRepository _windSpeedExtraFeeRepository = windSpeedExtraFeeRepository;
        private readonly ILogger<WindSpeedExtraFeeService> _logger = logger;

        public List<WindSpeedExtraFee> FindAll()
        {
            var allFees = _windSpeedExtraFeeRepository.List().Result;
            return allFees;
        }
        public WindSpeedExtraFee? FindByVehicleTypeLowerAndUpperSpeed(decimal lowerSpeed, decimal? upperSpeed, VehicleEnum vehicle)
        {
            var windSpeedFee = _windSpeedExtraFeeRepository.List().Result
                .Where(x => x.LowerSpeed == lowerSpeed)
                .Where(x => x.UpperSpeed == upperSpeed)
                .Where(x => x.VehicleType == vehicle)
                .FirstOrDefault();

            if (windSpeedFee == null)
            {
                var error = "Could not find WindSpeedExtraFee with this parameters.";
                _logger.LogError(error);
                return null;
            }
            _logger.LogInformation("WindSpeedExtraFee is founded.");
            return windSpeedFee;
        }

        public async Task<WindSpeedExtraFee?> UpdateFee(WindSpeedExtraFee windSpeedExtraFee, decimal? price, bool? forbitten)
        {
            var updatedFee = await _windSpeedExtraFeeRepository.Update(windSpeedExtraFee, price, forbitten);
            _logger.LogInformation("WindSpeedExtraFee is updated.");
            return updatedFee;
        }

        public async Task<WindSpeedExtraFee?> CreateFee(VehicleEnum vehicle, decimal lower, decimal? upper, decimal? price, bool? forbitten)
        {
            var fee = new WindSpeedExtraFee { LowerSpeed = lower, UpperSpeed = upper, VehicleType = vehicle, Price = price , Forbitten = forbitten};
            var createdFee = await _windSpeedExtraFeeRepository.Save(fee);
            _logger.LogInformation("WindSpeedExtraFee is created.");
            return createdFee;
        }

        public async Task<WindSpeedExtraFee?> DeleteFee(int id)
        {
            var fee = await _windSpeedExtraFeeRepository.FindById(id);
            if (fee != null)
            {
                await _windSpeedExtraFeeRepository.DeleteFee(fee);
                _logger.LogInformation("WindSpeedExtraFee was deleted.");
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
