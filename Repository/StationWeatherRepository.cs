using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryFeeApi.Repository
{
    [ExcludeFromCodeCoverage]
    public class StationWeatherRepository(ApplicationDbContext context, ILogger<StationWeatherRepository> logger) : IStationWeatherRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<StationWeatherRepository> _logger = logger;

        public async Task<List<StationWeather>> List()
        {
            return await _context.StationWeathers.ToListAsync();
        }

        public async Task<StationWeather?> FindById(int id)
        {
            var station = await _context.StationWeathers.FindAsync(id);
            if (station == null)
            {
                return null;
            }
            return station;
        }

        public async Task<StationWeather> Save(StationWeather station)
        {
            try
            {
                var new_station = new StationWeather
                {
                    StationName = station.StationName,
                    VmoCode = station.VmoCode,
                    AirTemp = station.AirTemp,
                    WindSpeed = station.WindSpeed,
                    WeatherPhenomenon = station.WeatherPhenomenon,
                    Timestamp = station.Timestamp
                };
                await _context.StationWeathers.AddAsync(new_station);
                _context.SaveChanges();
                _logger.LogInformation("Created new StationWeather");
                return new_station;
                
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
