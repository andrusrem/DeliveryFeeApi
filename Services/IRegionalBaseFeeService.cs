using DeliveryFeeApi.Data;

namespace DeliveryFeeApi.Services
{
    public interface IRegionalBaseFeeService
    {
        List<RegionalBaseFee> FindAll();
        RegionalBaseFee? FindByVehicleTypeAndStationName(StationEnum station, VehicleEnum vehicle);
        Task<RegionalBaseFee?> UpdateFee(RegionalBaseFee regionalBaseFee, decimal price);
        Task<RegionalBaseFee?> CreateFee(VehicleEnum vehicle, StationEnum station, decimal price);
        Task<RegionalBaseFee?> DeleteFee(int id);
        VehicleEnum? ConvertVehicleTypeToEnum(string vehicle);
        StationEnum? ConvertStationNameToEnum(string station);
    }
}
