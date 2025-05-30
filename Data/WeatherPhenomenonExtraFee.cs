﻿using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Data
{
    [ExcludeFromCodeCoverage]
    public class WeatherPhenomenonExtraFee
    {
        public int Id { get; set; }
        public string? WeatherPhenomenon {  get; set; }
        public VehicleEnum VehicleType { get; set; }
        public decimal? Price { get; set; }
        public bool? Forbitten { get; set; } = false;

    }
}
