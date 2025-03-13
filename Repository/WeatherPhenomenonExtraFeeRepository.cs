using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeApi.Repository
{
    public class WeatherPhenomenonExtraFeeRepository : IWeatherPhenomenonExtraFeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WeatherPhenomenonExtraFeeRepository> _logger;

        public WeatherPhenomenonExtraFeeRepository(ApplicationDbContext context, ILogger<WeatherPhenomenonExtraFeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<WeatherPhenomenonExtraFee>> List()
        {
            return await _context.WeatherPhenomenonExtraFees.ToListAsync();
        }

        public async Task<WeatherPhenomenonExtraFee?> FindById(int id)
        {
            var extra_fee = await _context.WeatherPhenomenonExtraFees.FindAsync(id);
            if (extra_fee == null)
            {
                return null;
            }
            return extra_fee;
        }

        public async Task<WeatherPhenomenonExtraFee> Save(WeatherPhenomenonExtraFee extra_fee)
        {
            try
            {
                var new_extra_fee = new WeatherPhenomenonExtraFee
                {
                    WeatherPhenomenon = extra_fee.WeatherPhenomenon,
                    VehicleType = extra_fee.VehicleType,
                    Price = extra_fee.Price,
                };
                await _context.WeatherPhenomenonExtraFees.AddAsync(new_extra_fee);
                _context.SaveChanges();
                _logger.LogInformation("Created new WeatherPhenomenonExtraFee");
                return new_extra_fee;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
