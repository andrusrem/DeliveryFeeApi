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

        public async Task<WeatherPhenomenonExtraFee> Update(WeatherPhenomenonExtraFee extraFee, decimal? price, bool? forbitten)
        {
            if (price != null)
            {
                extraFee.Price = price;
            }
            if (forbitten != null)
            {
                extraFee.Forbitten = forbitten;
            }

            _context.WeatherPhenomenonExtraFees.Update(extraFee);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"WeatherPhenomenonExtraFee is updated.");
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
                    Forbitten = extraFee.Forbitten,
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

        public async Task DeleteFee(WeatherPhenomenonExtraFee fee)
        {
            _context.WeatherPhenomenonExtraFees.Remove(fee);
            await _context.SaveChangesAsync();
        }
    }
}
