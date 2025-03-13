using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Repository
{
    public interface IWindSpeedExtraFeeRepository
    {
        Task<List<WindSpeedExtraFee>> List();
        Task<WindSpeedExtraFee?> FindById(int id);
        Task<WindSpeedExtraFee> Save(WindSpeedExtraFee extra_fee);

    }
}
