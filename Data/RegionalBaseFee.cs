namespace DeliveryFeeApi.Data
{
    public class RegionalBaseFee
    {
        public int Id { get; set; }
        public VehicleEnum VehicleType { get; set; }
        public StationEnum StationName { get; set; }
        public decimal Price { get; set; }
    }
}
