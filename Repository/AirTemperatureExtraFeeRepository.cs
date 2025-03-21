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
                var extraFee = await _context.AirTemperatureExtraFees.FindAsync(id);
                return extraFee;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while looking AirTemperatureFee by it id: {Message}", ex.Message);
                throw;
            }
            
        }

        public async Task<AirTemperatureExtraFee> Update(AirTemperatureExtraFee extraFee, decimal price)
        {
             extraFee.Price = price;
             _context.AirTemperatureExtraFees.Update(extraFee);
             await _context.SaveChangesAsync();
             _logger.LogInformation($"AirTemperatureExtraFee with lower {extraFee.LowerTemperature} and upper{extraFee.UpperTemperature} temperature updated.");
             return extraFee;
        }

        public async Task<AirTemperatureExtraFee> Save(AirTemperatureExtraFee extraFee)
        {
            try
            {
                var newExtraFee = new AirTemperatureExtraFee
                {
                    LowerTemperature = extraFee.LowerTemperature,
                    UpperTemperature = extraFee.UpperTemperature,
                    VehicleType = extraFee.VehicleType,
                    Price = extraFee.Price,
                };
                await _context.AirTemperatureExtraFees.AddAsync(newExtraFee);
                _context.SaveChanges();
                _logger.LogInformation("Created new AirTemperatureExtraFee");
                return newExtraFee;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating AirTemperatureFee: {Message}", ex.Message);
                throw;
            }
        }
        public async Task DeleteFee(AirTemperatureExtraFee fee)
        {
            _context.AirTemperatureExtraFees.Remove(fee);
            await _context.SaveChangesAsync();
        }
    }
}
