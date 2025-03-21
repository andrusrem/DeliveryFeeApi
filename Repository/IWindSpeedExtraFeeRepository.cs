using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Repository
{
    public interface IWindSpeedExtraFeeRepository
    {
        Task<List<WindSpeedExtraFee>> List();
        Task<WindSpeedExtraFee?> FindById(int id);
        Task<WindSpeedExtraFee> Update(WindSpeedExtraFee extraFee, decimal? price, bool? forbitten);
        Task<WindSpeedExtraFee> Save(WindSpeedExtraFee extraFee);
        Task DeleteFee(WindSpeedExtraFee fee);

    }
}
