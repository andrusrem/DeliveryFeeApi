using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IDeliveryPriceService
    {
        decimal GetAirTemperatureFee(VehicleEnum? vehicle, decimal? air_temp);
        decimal? GetWeatherPhenomenonFee(VehicleEnum? vehicle, string? weather_phenomenon);
        decimal? GetWindSpeedFee(VehicleEnum? vehicle, decimal? wind_speed);
        decimal GetBaseFee(VehicleEnum? vehicle, StationEnum? station);
        StationWeather GetStationWeather(StationEnum station);
        string? ConvertStationEnumToName(StationEnum? station);
        StationEnum? ConvertStationNameToEnum(string? station);
        VehicleEnum? ConvertVehicleTypeToEnum(string? vehicle);

    }
}
