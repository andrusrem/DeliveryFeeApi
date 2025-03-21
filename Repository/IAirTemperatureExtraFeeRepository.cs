using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Repository
{
    public interface IAirTemperatureExtraFeeRepository
    {
        Task<List<AirTemperatureExtraFee>> List();
        Task<AirTemperatureExtraFee?> FindById(int id);
        Task<AirTemperatureExtraFee> Update(AirTemperatureExtraFee extraFee, decimal price);
        Task<AirTemperatureExtraFee> Save(AirTemperatureExtraFee extraFee);
        Task DeleteFee(AirTemperatureExtraFee fee);
    }
}
