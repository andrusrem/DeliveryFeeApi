using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Repository
{
    public interface IRegionalBaseFeeRepository
    {
        Task<List<RegionalBaseFee>> List();
        Task<RegionalBaseFee> FindById(int id);
        Task<RegionalBaseFee> Save(RegionalBaseFee baseFee);
    }
}
