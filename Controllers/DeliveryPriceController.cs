using DeliveryFeeApi.Data;
using DeliveryFeeApi.Repository;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPriceController : ControllerBase
    {
        private readonly IDeliveryPriceService _service;

        public DeliveryPriceController(IDeliveryPriceService service)
        {
            _service = service;
        }

        [HttpGet("calculate")]
        public ActionResult<ResponseBody> GetDeliveryPrice(string station, string vehicle) 
        {
            var station_name_enum = _service.ConvertStationNameToEnum(station);
            var vehicle_type_enum = _service.ConvertVehicleTypeToEnum(vehicle);
            if (station_name_enum == null)
            {
                return BadRequest("Invalid station: we not operate in that area or you send station name in wrong format.");
            }
            else if (vehicle_type_enum == null)
            {
                return BadRequest("Invalid vehicle: you send vehicle type wrongly.");
            }

            var response = new ResponseBody();
            var weather_data = _service.GetStationWeather((StationEnum)station_name_enum);

            // Fee values
            var base_fee = _service.GetBaseFee(vehicle_type_enum, station_name_enum);
            var air_fee = _service.GetAirTemperatureFee(vehicle_type_enum, weather_data.AirTemp);
            var wind_speed_fee = _service.GetWindSpeedFee(vehicle_type_enum, weather_data.WindSpeed);
            var phenomenon_fee = _service.GetWeatherPhenomenonFee(vehicle_type_enum, weather_data.WeatherPhenomenon);
            if(wind_speed_fee == null || phenomenon_fee == null)
            {
                response.Forbitten = true;
                return Ok(response);
            }

            var total_fee = base_fee + air_fee + wind_speed_fee + phenomenon_fee;
            response.Total = total_fee;
            
            
            return Ok(response);
        }
    }
}
