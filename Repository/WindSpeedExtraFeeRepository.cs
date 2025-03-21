using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Repository
{
    [ExcludeFromCodeCoverage]
    public class WindSpeedExtraFeeRepository(ApplicationDbContext context, ILogger<WindSpeedExtraFeeRepository> logger) : IWindSpeedExtraFeeRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<WindSpeedExtraFeeRepository> _logger = logger;

        public async Task<List<WindSpeedExtraFee>> List()
        {
            return await _context.WindSpeedExtraFees.ToListAsync();
        }

        public async Task<WindSpeedExtraFee?> FindById(int id)
        {
            var extraFee = await _context.WindSpeedExtraFees.FindAsync(id);
            if (extraFee == null)
            {
                return null;
            }
            return extraFee;
        }
        public async Task<WindSpeedExtraFee> Update(WindSpeedExtraFee extraFee, decimal? price, bool? forbitten)
        {
            if(price != null) 
            {
                extraFee.Price = price;
            }
            if(forbitten != null)
            {
                extraFee.Forbitten = forbitten;
            }
            
            _context.WindSpeedExtraFees.Update(extraFee);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"WindSpeedExtraFee with lower {extraFee.LowerSpeed} m/s and upper {extraFee.UpperSpeed} m/s speed updated.");
            return extraFee;
        }

        public async Task<WindSpeedExtraFee> Save(WindSpeedExtraFee extraFee)
        {
            try
            {
                var newExtraFee = new WindSpeedExtraFee
                {
                    LowerSpeed = extraFee.LowerSpeed,
                    UpperSpeed = extraFee.UpperSpeed,
                    VehicleType = extraFee.VehicleType,
                    Price = extraFee.Price,
                };
                await _context.WindSpeedExtraFees.AddAsync(newExtraFee);
                _context.SaveChanges();
                _logger.LogInformation("Created new WindSpeedExtraFee");
                return newExtraFee;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task DeleteFee(WindSpeedExtraFee fee)
        {
            _context.WindSpeedExtraFees.Remove(fee);
            await _context.SaveChangesAsync();
        }
    }
}
