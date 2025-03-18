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
            var extraFee = await _context.WeatherPhenomenonExtraFees.FindAsync(id);
            if (extraFee == null)
            {
                return null;
            }
            return extraFee;
        }

        public async Task<WeatherPhenomenonExtraFee> Save(WeatherPhenomenonExtraFee extraFee)
        {
            try
            {
                var newExtraFee = new WeatherPhenomenonExtraFee
                {
                    WeatherPhenomenon = extraFee.WeatherPhenomenon,
                    VehicleType = extraFee.VehicleType,
                    Price = extraFee.Price,
                };
                await _context.WeatherPhenomenonExtraFees.AddAsync(newExtraFee);
                _context.SaveChanges();
                _logger.LogInformation("Created new WeatherPhenomenonExtraFee");
                return newExtraFee;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
