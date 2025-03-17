using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Data
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<StationWeather> StationWeathers { get; set; }
        public DbSet<AirTemperatureExtraFee> AirTemperatureExtraFees { get; set; }
        public DbSet<RegionalBaseFee> RegionalBaseFees { get; set; }
        public DbSet<WeatherPhenomenonExtraFee> WeatherPhenomenonExtraFees { get; set; }
        public DbSet<WindSpeedExtraFee> WindSpeedExtraFees { get; set; }
    }
}
