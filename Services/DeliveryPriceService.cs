using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace DeliveryFeeApi.Services
{
    public class DeliveryPriceService(
        IAirTemperatureExtraFeeRepository airTemperatureExtraFeeRepository,
        IWeatherPhenomenonExtraFeeRepository weatherPhenomenonExtraFeeRepository,
        IRegionalBaseFeeRepository regionalBaseFeeRepository,
        IWindSpeedExtraFeeRepository windSpeedExtraFeeRepository,
        IStationWeatherRepository stationWeatherRepository,
        ILogger<DeliveryPriceService> logger) : IDeliveryPriceService
    {
        private readonly IAirTemperatureExtraFeeRepository _airTemperatureExtraFeeRepository = airTemperatureExtraFeeRepository;
        private readonly IWeatherPhenomenonExtraFeeRepository _weatherPhenomenonExtraFeeRepository = weatherPhenomenonExtraFeeRepository;
        private readonly IRegionalBaseFeeRepository _regionalBaseFeeRepository = regionalBaseFeeRepository;
        private readonly IWindSpeedExtraFeeRepository _windSpeedExtraFeeRepository = windSpeedExtraFeeRepository;
        private readonly IStationWeatherRepository _stationWeatherRepository = stationWeatherRepository;
        private readonly ILogger<DeliveryPriceService> _logger = logger;

        public decimal GetAirTemperatureFee(VehicleEnum vehicle, decimal airTemp)
        {
            var airFee = _airTemperatureExtraFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.LowerTemperature < airTemp)
                .Where(x => x.UpperTemperature > airTemp)
                .FirstOrDefault();

            if (airFee != null)
            {
                return airFee.Price;
            }
            return 0;
        }

        public decimal? GetWeatherPhenomenonFee(VehicleEnum vehicle, string weatherPhenomenon) 
        {
            var phenomenonFee = _weatherPhenomenonExtraFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.WeatherPhenomenon == weatherPhenomenon)
                .FirstOrDefault();
            if(phenomenonFee != null)
            {
                if(phenomenonFee.Forbitten == false) 
                {
                    return phenomenonFee.Price;
                }
                // When this function returns null, it means that operations on this vehicle are forbidden
                return null;
                
            }
            return 0;
        }

        public decimal? GetWindSpeedFee(VehicleEnum vehicle, decimal windSpeed) 
        {
            var windFee = _windSpeedExtraFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.LowerSpeed < windSpeed)
                .Where(x => x.UpperSpeed > windSpeed)
                .FirstOrDefault();

            if (windFee != null)
            {
                if (windFee.Forbitten == false)
                {
                    return windFee.Price;
                }
                // When this function returns null, it means that operations on this vehicle are forbidden
                return null;
            }
            return 0;
        }

        public decimal GetBaseFee(VehicleEnum vehicle, StationEnum station)
        {
            var baseFee = _regionalBaseFeeRepository
                .List().Result
                .Where(x => x.VehicleType == vehicle)
                .Where(x => x.StationName == station)
                .FirstOrDefault();
            if (baseFee != null)
            {
                return baseFee.Price;
            }
            return 0;
        }

        public StationWeather GetStationWeather(StationEnum? station)
        {
            try
            {
                var properName = ConvertStationEnumToName(station);
                var stationWeather = _stationWeatherRepository
                    .List().Result
                    .Where(x => x.StationName == properName)
                    .Where(x => x.Timestamp < DateTime.Now.ToLong())
                    .OrderByDescending(x => x.Timestamp)
                    .FirstOrDefault();

                return stationWeather;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving station weather: Failed to retrieve station weather.");
                throw;
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
