using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Repository
{
    [ExcludeFromCodeCoverage]
    public class AirTemperatureExtraFeeRepository(ApplicationDbContext context, ILogger<AirTemperatureExtraFeeRepository> logger) : IAirTemperatureExtraFeeRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<AirTemperatureExtraFeeRepository> _logger = logger;

        public async Task<List<AirTemperatureExtraFee>> List()
        {
            return await _context.AirTemperatureExtraFees.ToListAsync();
        }

        public async Task<AirTemperatureExtraFee?> FindById(int id)
        {
            try
            {
                var extra_fee = await _context.AirTemperatureExtraFees.FindAsync(id);
                return extra_fee;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while looking AirTemperatureFee by it id: {Message}", ex.Message);
                throw;
            }
            
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
                _logger.LogInformation("Created new AirTemperatureExtraFee");
                return new_extra_fee;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating AirTemperatureFee: {Message}", ex.Message);
                throw;
            }

        }
    }
}
