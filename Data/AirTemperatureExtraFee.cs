namespace DeliveryFeeApi.Data
{
    public class AirTemperatureExtraFee
    {
        public int Id { get; set; }
        public decimal LowerTemperature { get; set; }
        public decimal UpperTemperature { get; set;}
        public VehicleEnum VehicleType { get; set; }
        public decimal Price { get; set; }
    }
}
