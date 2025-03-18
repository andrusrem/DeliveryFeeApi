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
            var stationNameEnum = _service.ConvertStationNameToEnum(station);
            var vehicleTypeEnum = _service.ConvertVehicleTypeToEnum(vehicle);
            if (stationNameEnum == null)
            {
                return BadRequest("Invalid station: we not operate in that area or you send station name in wrong format.");
            }
            else if (vehicleTypeEnum == null)
            {
                return BadRequest("Invalid vehicle: you send vehicle type wrongly.");
            }

            var response = new ResponseBody();
            var weatherData = _service.GetStationWeather((StationEnum)stationNameEnum);

            // Fee values
            var baseFee = _service.GetBaseFee((VehicleEnum)vehicleTypeEnum, (StationEnum)stationNameEnum);
            var airFee = _service.GetAirTemperatureFee((VehicleEnum)vehicleTypeEnum, (decimal)weatherData.AirTemp);
            var windSpeedFee = _service.GetWindSpeedFee((VehicleEnum)vehicleTypeEnum, (decimal)weatherData.WindSpeed);
            var phenomenonFee = _service.GetWeatherPhenomenonFee((VehicleEnum)vehicleTypeEnum, weatherData.WeatherPhenomenon);
            if(windSpeedFee == null || phenomenonFee == null)
            {
                response.Forbitten = true;
                return Ok(response);
            }

            var totalFee = baseFee + airFee + windSpeedFee + phenomenonFee;
            response.Total = totalFee;
            
            
            return Ok(response);
        }
    }
}
