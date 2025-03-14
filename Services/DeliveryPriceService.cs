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

        public DeliveryPriceService(
            IAirTemperatureExtraFeeRepository airTemperatureExtraFeeRepository,
            IWeatherPhenomenonExtraFeeRepository weatherPhenomenonExtraFeeRepository,
            IRegionalBaseFeeRepository regionalBaseFeeRepository,
            IWindSpeedExtraFeeRepository windSpeedExtraFeeRepository,
            IStationWeatherRepository stationWeatherRepository)
        {
            _airTemperatureExtraFeeRepository = airTemperatureExtraFeeRepository;
            _weatherPhenomenonExtraFeeRepository = weatherPhenomenonExtraFeeRepository;
            _regionalBaseFeeRepository = regionalBaseFeeRepository;
            _windSpeedExtraFeeRepository = windSpeedExtraFeeRepository;
            _stationWeatherRepository = stationWeatherRepository;
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

        public StationWeather? GetStationWeather(StationEnum? station)
        {
            var proper_name = ConvertStationEnumToName(station);
            var station_weather = _stationWeatherRepository
                .List().Result
                .Where(x => x.StationName == proper_name)
                .FirstOrDefault();

            if (station_weather != null)
            {
                return station_weather;
            }
            return null;

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
            if (station == "Tallinn")
            {
                return StationEnum.Tallinn;
            }
            else if (station == "Tartu")
            {
                return StationEnum.Tartu;
            }
            else if (station == "Pärnu")
            {
                return StationEnum.Pärnu;
            }
            return null;
        }
        public VehicleEnum? ConvertVehicleTypeToEnum(string? vehicle)
        {
            if (vehicle == "Bike")
            {
                return VehicleEnum.Bike;
            }
            else if (vehicle == "Car")
            {
                return VehicleEnum.Car;
            }
            else if (vehicle == "Scooter")
            {
                return VehicleEnum.Scooter;
            }
            return null;
        }
    }
}
