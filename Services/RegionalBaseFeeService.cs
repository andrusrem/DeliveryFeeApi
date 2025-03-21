using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;

namespace DeliveryFeeApi.Services
{
    public class RegionalBaseFeeService(
        IRegionalBaseFeeRepository regionalBaseFeeRepository,
        ILogger<RegionalBaseFeeService> logger) : IRegionalBaseFeeService
    {
        private readonly IRegionalBaseFeeRepository _regionalBaseFeeRepository = regionalBaseFeeRepository;
        private readonly ILogger<RegionalBaseFeeService> _logger = logger;

        public List<RegionalBaseFee> FindAll()
        {
            var allFees = _regionalBaseFeeRepository.List().Result;
            return allFees;
        }
        public RegionalBaseFee? FindByVehicleTypeAndStationName(StationEnum station, VehicleEnum vehicle)
        {
            var baseFee = _regionalBaseFeeRepository.List().Result
                .Where(x => x.StationName == station)
                .Where(x => x.VehicleType == vehicle)
                .FirstOrDefault();

            if (baseFee == null)
            {
                var error = "Could not find RegionalBaseFee with this parameters.";
                _logger.LogError(error);
                return null;
            }
            _logger.LogInformation("RegionalBaseFee is founded.");
            return baseFee;
        }

        public async Task<RegionalBaseFee?> UpdateFee(RegionalBaseFee regionalBaseFee, decimal price)
        {
            var updatedFee = await _regionalBaseFeeRepository.Update(regionalBaseFee, price);
            _logger.LogInformation("RegionalBaseFee is updated.");
            return updatedFee;
        }

        public async Task<RegionalBaseFee?> CreateFee(VehicleEnum vehicle, StationEnum station, decimal price)
        {
            var fee = new RegionalBaseFee { VehicleType = vehicle, StationName = station, Price = price };
            var createdFee = await _regionalBaseFeeRepository.Save(fee);
            _logger.LogInformation("RegionalBaseFee is created.");
            return createdFee;
        }

        public async Task<RegionalBaseFee?> DeleteFee(int id)
        {
            var fee = await _regionalBaseFeeRepository.FindById(id);
            if (fee != null)
            {
                await _regionalBaseFeeRepository.DeleteFee(fee);
                _logger.LogInformation("RegionalBaseFee was deleted.");
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

        public StationEnum? ConvertStationNameToEnum(string station)
        {
            if (station.ToLower() == "tallinn")
            {
                return StationEnum.Tallinn;
            }
            else if (station.ToLower() == "tartu")
            {
                return StationEnum.Tartu;
            }
            else if (station.ToLower() == "pärnu")
            {
                return StationEnum.Pärnu;
            }
            return null;
        }
    }
}
