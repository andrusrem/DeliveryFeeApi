using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Repository
{
    [ExcludeFromCodeCoverage]
    public class RegionalBaseFeeRepository(ApplicationDbContext context, ILogger<RegionalBaseFeeRepository> logger) : IRegionalBaseFeeRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<RegionalBaseFeeRepository> _logger = logger;

        public async Task<List<RegionalBaseFee>> List()
        {
            return await _context.RegionalBaseFees.ToListAsync();
        }

        public async Task<RegionalBaseFee> FindById(int id)
        {
            var baseFee = await _context.RegionalBaseFees.FindAsync(id);
            if (baseFee == null)
            {
                return null;
            }
            return baseFee;
        }

        public async Task<RegionalBaseFee> Save(RegionalBaseFee baseFee)
        {
            try
            {
                var newBaseFee = new RegionalBaseFee
                {
                    VehicleType = baseFee.VehicleType,
                    StationName = baseFee.StationName,
                    Price = baseFee.Price,
                };
                await _context.RegionalBaseFees.AddAsync(baseFee);
                _context.SaveChanges();
                _logger.LogInformation("Created new RegionalBaseFee");
                return baseFee;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
