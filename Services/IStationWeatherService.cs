using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IStationWeatherService
    {
        Task<List<StationWeather>> GetWeatherData();
        List<StationWeather> DeserializeWeatherData(string xml);
    }
}
