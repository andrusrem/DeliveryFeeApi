using Microsoft.CodeAnalysis.Elfie.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace DeliveryFeeApi.Data
{
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = "station")]
    public class StationWeather
    {
        [Key]
        public int Id { get; set; }
        [XmlElement(ElementName = "name")]
        public string? StationName { get; set; }
        [XmlElement(ElementName = "wmocode")]
        public int? VmoCode {  get; set; }
        [XmlElement(ElementName = "airtemperature")]
        public decimal? AirTemp { get; set; }
        [XmlElement(ElementName = "windspeed")]
        public decimal? WindSpeed { get; set; }
        [XmlElement(ElementName = "phenomenon")]
        public string? WeatherPhenomenon {  get; set; }
        [XmlElement(ElementName = "timestamp")]
        public long Timestamp { get; set; } = DateTime.Now.ToLong();

        

    }
}
