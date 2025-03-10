using DeliveryFeeApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFeeApi.Repository
{
    public class StationWeatherRepository : IStationWeatherRepository
    {
        private readonly ApplicationDbContext _context;

        public StationWeatherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StationWeather>> List()
        {
            return await _context.StationWeathers.ToListAsync();
        }

        public async Task<StationWeather?> FindById(int id)
        {
            var station = await _context.StationWeathers.FindAsync(id);
            if(station == null)
            {
                return null;
            }
            return station;
        }

        public async Task Save(int id, StationWeather station)
        {
            var new_station = _context.StationWeathers.FirstOrDefault(x => x.Id == id);
            if(new_station == null)
            {
                await _context.AddAsync(station);
            }
            else
            {
                new_station.StationName = station.StationName;
                new_station.VmoCode = station.VmoCode;
                new_station.AirTemp = station.AirTemp;
                new_station.WindSpeed = station.WindSpeed;
                new_station.WeatherPhenomenon = station.WeatherPhenomenon;
                new_station.Timestamp = station.Timestamp;
                _context.Update(station);
            }

            await _context.SaveChangesAsync();
        }

    }
}
