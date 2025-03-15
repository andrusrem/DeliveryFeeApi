using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeliveryFeeApi.Services
{
    public class DeliveryPriceService : IDeliveryPriceService
    {
        private readonly IAirTemperatureExtraFeeRepository _airTemperatureExtraFeeRepository;
        private readonly IWeatherPhenomenonExtraFeeRepository _weatherPhenomenonExtraFeeRepository;
        private readonly IRegionalBaseFeeRepository _regionalBaseFeeRepository;
        private readonly IWindSpeedExtraFeeRepository _windSpeedExtraFeeRepository;
        private readonly IStationWeatherRepository _stationWeatherRepository;
        private readonly ILogger<DeliveryPriceService> _logger;

        public DeliveryPriceService(
            IAirTemperatureExtraFeeRepository airTemperatureExtraFeeRepository,
            IWeatherPhenomenonExtraFeeRepository weatherPhenomenonExtraFeeRepository,
            IRegionalBaseFeeRepository regionalBaseFeeRepository,
            IWindSpeedExtraFeeRepository windSpeedExtraFeeRepository,
            IStationWeatherRepository stationWeatherRepository,
            ILogger<DeliveryPriceService> logger)
        {
            _airTemperatureExtraFeeRepository = airTemperatureExtraFeeRepository;
            _weatherPhenomenonExtraFeeRepository = weatherPhenomenonExtraFeeRepository;
            _regionalBaseFeeRepository = regionalBaseFeeRepository;
            _windSpeedExtraFeeRepository = windSpeedExtraFeeRepository;
            _stationWeatherRepository = stationWeatherRepository;
            _logger = logger;
        }

        public decimal GetAirTemperatureFee(VehicleEnum? vehicle, decimal? air_temp)
        {
            var air_fee = _airTemperatureExtraFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.LowerTemperature < air_temp && x.UpperTemperature > air_temp)
                .FirstOrDefault();

            if (air_fee != null)
            {
                return air_fee.Price;
            }
            return 0;
        }

        public decimal? GetWeatherPhenomenonFee(VehicleEnum? vehicle, string? weather_phenomenon) 
        {
            var phenomenon_fee = _weatherPhenomenonExtraFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.WeatherPhenomenon == weather_phenomenon)
                .FirstOrDefault();
            if(phenomenon_fee != null)
            {
                if(phenomenon_fee.Price != null) 
                {
                    return phenomenon_fee.Price;
                }
                else if (phenomenon_fee.Forbitten == true)
                {
                    // When this function returns null, it means that operations on this vehicle are forbidden
                    return null;
                }
            }
            return 0;
        }

        public decimal? GetWindSpeedFee(VehicleEnum? vehicle, decimal? wind_speed) 
        {
            var wind_fee = _windSpeedExtraFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.LowerSpeed < wind_speed)
                .FirstOrDefault();

            if (wind_fee != null)
            {
                if (wind_fee.Price != null)
                {
                    return wind_fee.Price;
                }
                else if(wind_fee.Forbitten == true)
                {
                    // When this function returns null, it means that operations on this vehicle are forbidden
                    return null;
                }
            }
            return 0;
        }

        public decimal GetBaseFee(VehicleEnum? vehicle, StationEnum? station)
        {
            var base_fee = _regionalBaseFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.StationName == station)
                .FirstOrDefault();
            if (base_fee != null)
            {
                return base_fee.Price;
            }
            return 0;
        }

        public StationWeather GetStationWeather(StationEnum station)
        {
            try
            {
                var proper_name = ConvertStationEnumToName(station);
                var station_weather = _stationWeatherRepository
                    .List().Result
                    .Where(x => x.StationName == proper_name)
                    .FirstOrDefault();

                return station_weather;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving station weather: {ex.Message}");
                throw new ApplicationException("Failed to retrieve station weather.", ex);
            }
            


        }
        public string? ConvertStationEnumToName(StationEnum? station)
        {
            if(station == StationEnum.Tallinn)
            {
                return "Tallinn-Harku";
            }
            else if(station == StationEnum.Tartu)
            {
                return "Tartu-Tõravere";
            }
            else if(station == StationEnum.Pärnu)
            {
                return "Pärnu";
            }
            return "";
        }
        public StationEnum? ConvertStationNameToEnum(string? station)
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
        public VehicleEnum? ConvertVehicleTypeToEnum(string? vehicle)
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
