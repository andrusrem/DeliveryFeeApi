using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Repository
{
    public interface IWeatherPhenomenonExtraFeeRepository
    {
        Task<List<WeatherPhenomenonExtraFee>> List();
        Task<WeatherPhenomenonExtraFee?> FindById(int id);
        Task<WeatherPhenomenonExtraFee> Save(WeatherPhenomenonExtraFee extraFee);
    }
}
