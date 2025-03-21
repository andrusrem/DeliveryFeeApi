using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;

namespace DeliveryFeeApi.Services
{
    public class WeatherPhenomenonExtraFeeService(
        IWeatherPhenomenonExtraFeeRepository weatherPhenomenonExtraFeeRepository,
        ILogger<WeatherPhenomenonExtraFeeService> logger) : IWeatherPhenomenonExtraFeeService
    {
        private readonly IWeatherPhenomenonExtraFeeRepository _weatherPhenomenonExtraFeeRepository = weatherPhenomenonExtraFeeRepository;
        private readonly ILogger<WeatherPhenomenonExtraFeeService> _logger = logger;

        public List<WeatherPhenomenonExtraFee> FindAll()
        {
            var allFees = _weatherPhenomenonExtraFeeRepository.List().Result;
            return allFees;
        }
        public WeatherPhenomenonExtraFee? FindByVehicleTypeAndPhenomenon(string phenomenon, VehicleEnum vehicle)
        {
            var phenomenonFee = _weatherPhenomenonExtraFeeRepository.List().Result
                .Where(x => x.WeatherPhenomenon == phenomenon)
                .Where(x => x.VehicleType == vehicle)
                .FirstOrDefault();

            if (phenomenonFee == null)
            {
                var error = "Could not find WeatherPhenomenonExtraFee with this parameters.";
                _logger.LogError(error);
                return null;
            }
            _logger.LogInformation("WeatherPhenomenonExtraFee is founded.");
            return phenomenonFee;
        }

        public async Task<WeatherPhenomenonExtraFee?> UpdateFee(WeatherPhenomenonExtraFee windSpeedExtraFee, decimal? price, bool? forbitten)
        {
            var updatedFee = await _weatherPhenomenonExtraFeeRepository.Update(windSpeedExtraFee, price, forbitten);
            _logger.LogInformation("WeatherPhenomenonExtraFee is updated.");
            return updatedFee;
        }

        public async Task<WeatherPhenomenonExtraFee?> CreateFee(VehicleEnum vehicle, string phenomenon, decimal? price, bool? forbitten)
        {
            var fee = new WeatherPhenomenonExtraFee { WeatherPhenomenon = phenomenon, VehicleType = vehicle, Price = price, Forbitten = forbitten };
            var createdFee = await _weatherPhenomenonExtraFeeRepository.Save(fee);
            _logger.LogInformation("WeatherPhenomenonExtraFee is created.");
            return createdFee;
        }

        public async Task<WeatherPhenomenonExtraFee?> DeleteFee(int id)
        {
            var fee = await _weatherPhenomenonExtraFeeRepository.FindById(id);
            if (fee != null)
            {
                await _weatherPhenomenonExtraFeeRepository.DeleteFee(fee);
                _logger.LogInformation("WeatherPhenomenonExtraFee was deleted.");
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
