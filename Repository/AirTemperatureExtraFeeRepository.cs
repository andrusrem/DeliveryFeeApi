using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeApi.Repository
{
    public class AirTemperatureExtraFeeRepository : IAirTemperatureExtraFeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AirTemperatureExtraFeeRepository> _logger;

        public AirTemperatureExtraFeeRepository(ApplicationDbContext context, ILogger<AirTemperatureExtraFeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<AirTemperatureExtraFee>> List()
        {
            return await _context.AirTemperatureExtraFees.ToListAsync();
        }

        public async Task<AirTemperatureExtraFee?> FindById(int id)
        {
            var extra_fee = await _context.AirTemperatureExtraFees.FindAsync(id);
            if (extra_fee == null)
            {
                return null;
            }
            return extra_fee;
        }

        public async Task<AirTemperatureExtraFee> Save(AirTemperatureExtraFee extra_fee)
        {
            try
            {
                var new_extra_fee = new AirTemperatureExtraFee
                {
                    LowerTemperature = extra_fee.LowerTemperature,
                    UpperTemperature = extra_fee.UpperTemperature,
                    VehicleType = extra_fee.VehicleType,
                    Price = extra_fee.Price,
                };
                await _context.AirTemperatureExtraFees.AddAsync(new_extra_fee);
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
