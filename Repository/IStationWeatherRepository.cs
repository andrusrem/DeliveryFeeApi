using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Repository
{
    public interface IStationWeatherRepository
    {
        Task<List<StationWeather>> List();
        Task<StationWeather?> FindById(int id);
        Task<StationWeather> Save(StationWeather station);

    }
}
