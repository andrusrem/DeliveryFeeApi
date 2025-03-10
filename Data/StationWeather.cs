using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace DeliveryFeeApi.Data
{
    [XmlRoot(ElementName = "station")]
    public class StationWeather
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [XmlElement(ElementName = "name")]
        public string StationName { get; set; }
        [Required]
        [XmlElement(ElementName = "wmocode")]
        
        public int VmoCode {  get; set; }
        [XmlElement(ElementName = "airtemperature")]
        public decimal AirTemp { get; set; }
        [XmlElement(ElementName = "windspeed")]
        public decimal WindSpeed { get; set; }
        [XmlElement(ElementName = "phenomenon")]
        public string WeatherPhenomenon {  get; set; }
        [XmlElement(ElementName = "timestamp")]
        public long Timestamp { get; set; }

        public StationWeather(string stationName, string weatherPhenomenon)
        {
            StationName = stationName;
            WeatherPhenomenon = weatherPhenomenon;
        }

    }
}
