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
            var extra_fee = await _context.WindSpeedExtraFees.FindAsync(id);
            if (extra_fee == null)
            {
                return null;
            }
            return extra_fee;
        }

        public async Task<WindSpeedExtraFee> Save(WindSpeedExtraFee extra_fee)
        {
            try
            {
                var new_extra_fee = new WindSpeedExtraFee
                {
                    LowerSpeed = extra_fee.LowerSpeed,
                    UpperSpeed = extra_fee.UpperSpeed,
                    VehicleType = extra_fee.VehicleType,
                    Price = extra_fee.Price,
                };
                await _context.WindSpeedExtraFees.AddAsync(new_extra_fee);
                _context.SaveChanges();
                _logger.LogInformation("Created new WindSpeedExtraFee");
                return new_extra_fee;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
