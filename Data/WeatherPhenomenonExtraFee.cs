namespace DeliveryFeeApi.Data
{
    public class WeatherPhenomenonExtraFee
    {
        public int Id { get; set; }
        public string? WeatherPhenomenon {  get; set; }
        public VehicleEnum VehicleType { get; set; }
        public decimal Price { get; set; }

    }
}
