using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Repository
{
    [ExcludeFromCodeCoverage]
    public class WeatherPhenomenonExtraFeeRepository(ApplicationDbContext context, ILogger<WeatherPhenomenonExtraFeeRepository> logger) : IWeatherPhenomenonExtraFeeRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<WeatherPhenomenonExtraFeeRepository> _logger = logger;

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
