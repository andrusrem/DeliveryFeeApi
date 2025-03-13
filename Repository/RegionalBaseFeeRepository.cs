using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeApi.Repository
{
    public class RegionalBaseFeeRepository : IRegionalBaseFeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegionalBaseFeeRepository> _logger;

        public RegionalBaseFeeRepository(ApplicationDbContext context, ILogger<RegionalBaseFeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<RegionalBaseFee>> List()
        {
            return await _context.RegionalBaseFees.ToListAsync();
        }

        public async Task<RegionalBaseFee> FindById(int id)
        {
            var base_fee = await _context.RegionalBaseFees.FindAsync(id);
            if (base_fee == null)
            {
                return null;
            }
            return base_fee;
        }

        public async Task<RegionalBaseFee> Save(RegionalBaseFee base_fee)
        {
            try
            {
                var new_base_fee = new RegionalBaseFee
                {
                    VehicleType = base_fee.VehicleType,
                    StationName = base_fee.StationName,
                    Price = base_fee.Price,
                };
                await _context.RegionalBaseFees.AddAsync(base_fee);
                _context.SaveChanges();
                _logger.LogInformation("Created new RegionalBaseFee");
                return base_fee;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
