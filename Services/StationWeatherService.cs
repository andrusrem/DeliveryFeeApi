using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using NuGet.Protocol;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DeliveryFeeApi.Services
{
    public class StationWeatherService : IStationWeatherService
    {
        private readonly IStationWeatherRepository _stationWeatherRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StationWeatherService> _logger;
        public StationWeatherService(IStationWeatherRepository stationWeatherRepository, IHttpClientFactory httpClientFactory, ILogger<StationWeatherService> logger)
        {
            _stationWeatherRepository = stationWeatherRepository;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<StationWeather>> GetWeatherData()
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            string url = "https://www.ilmateenistus.ee/ilma_andmed/xml/observations.php";
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                var weatherData = ParseWeatherData(responseData);
                _logger.LogInformation($"Information: {weatherData.Count}");
                return ParseWeatherData(responseData);
            }
        }

        public List<StationWeather> ParseWeatherData(string xml)
        {
            var doc = XDocument.Parse(xml);
            var stations = doc.Descendants("station").Select(s => new StationWeather
            {
                StationName = (string?)s.Element("name")?.Value,
                VmoCode = int.TryParse((string?)s.Element("wmocode")?.Value, out int vmo) ? vmo : (int?)null,
                AirTemp = decimal.TryParse((string?)s.Element("airtemperature")?.Value, out decimal temp) ? temp : (decimal?)null,
                WindSpeed = decimal.TryParse((string?)s.Element("windspeed")?.Value, out decimal speed) ? speed : (decimal?)null,
                WeatherPhenomenon = (string?)s.Element("phenomenon")?.Value,
            }).ToList();
            return stations;
        }
        
        public async Task LoadToDatabase()
        {
            List<StationWeather> stations = await GetWeatherData();

            foreach ( StationWeather station in stations )
            {
                if (station.StationName == "Tallinn-Harku" || station.StationName == "Tartu-Tõravere" || station.StationName == "Pärnu")
                {
                    _logger.LogInformation($"Information: Ready to load {station.ToJson()}");
                    await _stationWeatherRepository.Save(station);
                }

            }

        }

    }
}
