using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<StationWeather> StationWeathers { get; set; }
        public DbSet<AirTemperatureExtraFee> AirTemperatureExtraFees { get; set; }
        public DbSet<RegionalBaseFee> RegionalBaseFees { get; set; }
        public DbSet<WeatherPhenomenonExtraFee> WeatherPhenomenonExtraFees { get; set; }
        public DbSet<WindSpeedExtraFee> WindSpeedExtraFees { get; set; }
    }
}
