using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Data
{
    [ExcludeFromCodeCoverage]
    public class WindSpeedExtraFee
    {
        public int Id { get; set; }
        public decimal LowerSpeed { get; set; }
        public decimal? UpperSpeed { get; set;}
        public VehicleEnum VehicleType { get; set; }
        public decimal? Price { get; set; }
        public bool? Forbitten { get; set; } = false;
    }
}
