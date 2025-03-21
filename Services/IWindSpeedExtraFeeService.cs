using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IWindSpeedExtraFeeService
    {
        List<WindSpeedExtraFee> FindAll();
        WindSpeedExtraFee? FindByVehicleTypeLowerAndUpperSpeed(decimal lowerSpeed, decimal? upperSpeed, VehicleEnum vehicle);
        Task<WindSpeedExtraFee?> UpdateFee(WindSpeedExtraFee windSpeedExtraFee, decimal? price, bool? forbitten);
        Task<WindSpeedExtraFee?> CreateFee(VehicleEnum vehicle, decimal lower, decimal? upper, decimal? price, bool? forbitten);
        Task<WindSpeedExtraFee?> DeleteFee(int id);
        VehicleEnum? ConvertVehicleTypeToEnum(string vehicle);
    }
}
