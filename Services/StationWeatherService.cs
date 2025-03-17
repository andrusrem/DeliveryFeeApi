using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using NuGet.Protocol;
using System.Xml.Linq;

namespace DeliveryFeeApi.Services
{
    public class StationWeatherService(IStationWeatherRepository stationWeatherRepository, IHttpClientFactory httpClientFactory, ILogger<StationWeatherService> logger) : IStationWeatherService
    {
        private readonly IStationWeatherRepository _stationWeatherRepository = stationWeatherRepository;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ILogger<StationWeatherService> _logger = logger;

        public async Task<List<StationWeather>> GetWeatherData()
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            string url = "https://www.ilmateenistus.ee/ilma_andmed/xml/observations.php";
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                var weatherData = ParseWeatherData(responseData);

                _logger.LogInformation($"Information: {weatherData.Count}");
                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching weather data");
                throw;
            }
        }

        public List<StationWeather> ParseWeatherData(string xml)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing weather XML data");
                return new List<StationWeather>();
            }
            
        }
        
        public async Task LoadToDatabase(List<StationWeather> stations)
        {

            if (stations.Count == 0)
            {
                _logger.LogWarning("No weather data available to load into the database.");
                return;
            }

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
