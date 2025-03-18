using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IDeliveryPriceService
    {
        decimal GetAirTemperatureFee(VehicleEnum vehicle, decimal airTemp);
        decimal? GetWeatherPhenomenonFee(VehicleEnum vehicle, string weatherPhenomenon);
        decimal? GetWindSpeedFee(VehicleEnum vehicle, decimal windSpeed);
        decimal GetBaseFee(VehicleEnum vehicle, StationEnum station);
        StationWeather GetStationWeather(StationEnum? station);
        string? ConvertStationEnumToName(StationEnum? station);
        StationEnum? ConvertStationNameToEnum(string station);
        VehicleEnum? ConvertVehicleTypeToEnum(string vehicle);

    }
}
