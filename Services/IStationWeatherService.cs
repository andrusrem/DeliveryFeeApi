using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IStationWeatherService
    {
        Task<List<StationWeather>> GetWeatherData();
        List<StationWeather> ParseWeatherData(string xml);
        Task LoadToDatabase(List<StationWeather> stations);
    }
}
