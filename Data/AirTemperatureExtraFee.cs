using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Data
{
    [ExcludeFromCodeCoverage]
    public class AirTemperatureExtraFee
    {
        public int Id { get; set; }
        public decimal LowerTemperature { get; set; }
        public decimal UpperTemperature { get; set;}
        public VehicleEnum VehicleType { get; set; }
        public decimal Price { get; set; }
    }
}
