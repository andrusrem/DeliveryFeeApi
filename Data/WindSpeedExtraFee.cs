namespace DeliveryFeeApi.Data
{
    public class WindSpeedExtraFee
    {
        public int Id { get; set; }
        public decimal LowerSpeed { get; set; }
        public decimal UpperSpeed { get; set;}
        public VehicleEnum VehicleType { get; set; }
        public StationEnum StationName { get; set; }
        public decimal Price { get; set; }
    }
}
