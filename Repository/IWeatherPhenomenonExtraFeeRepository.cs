using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Repository
{
    public interface IWeatherPhenomenonExtraFeeRepository
    {
        Task<List<WeatherPhenomenonExtraFee>> List();
        Task<WeatherPhenomenonExtraFee?> FindById(int id);
        Task<WeatherPhenomenonExtraFee> Update(WeatherPhenomenonExtraFee extraFee, decimal? price, bool? forbitten);
        Task<WeatherPhenomenonExtraFee> Save(WeatherPhenomenonExtraFee extraFee);
        Task DeleteFee(WeatherPhenomenonExtraFee fee);
    }
}
