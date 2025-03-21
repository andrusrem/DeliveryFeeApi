using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IAirTemperatureExtraFeeService
    {
        List<AirTemperatureExtraFee> FindAll();
        AirTemperatureExtraFee? FindByVehicleTypeLowerAndUpperPoints(decimal lowerPoint, decimal upperPoint, VehicleEnum vehicle);
        Task<AirTemperatureExtraFee?> UpdateFee(AirTemperatureExtraFee airTemperatureExtraFee, decimal price);
        Task<AirTemperatureExtraFee?> CreateFee(VehicleEnum vehicle, decimal lower, decimal upper, decimal price);
        Task<AirTemperatureExtraFee?> DeleteFee(int id);
        VehicleEnum? ConvertVehicleTypeToEnum(string vehicle);
    }
}
