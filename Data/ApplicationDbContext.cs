using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<StationWeather> StationWeathers { get; set; }
    }
}
