using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IWeatherPhenomenonExtraFeeService
    {
        List<WeatherPhenomenonExtraFee> FindAll();
        WeatherPhenomenonExtraFee? FindByVehicleTypeAndPhenomenon(string phenomenon, VehicleEnum vehicle);
        Task<WeatherPhenomenonExtraFee?> UpdateFee(WeatherPhenomenonExtraFee windSpeedExtraFee, decimal? price, bool? forbitten);
        Task<WeatherPhenomenonExtraFee?> CreateFee(VehicleEnum vehicle, string phenomenon, decimal? price, bool? forbitten);
        Task<WeatherPhenomenonExtraFee?> DeleteFee(int id);
        VehicleEnum? ConvertVehicleTypeToEnum(string vehicle);
    }
}
