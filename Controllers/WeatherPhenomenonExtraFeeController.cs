using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherPhenomenonExtraFeeController : ControllerBase
    {
        private readonly IWeatherPhenomenonExtraFeeService _weatherPhenomenonExtraFeeService;

        public WeatherPhenomenonExtraFeeController(IWeatherPhenomenonExtraFeeService weatherPhenomenonExtraFeeService)
        {
            _weatherPhenomenonExtraFeeService = weatherPhenomenonExtraFeeService;
        }

        [HttpGet("all")]
        public ActionResult<List<WeatherPhenomenonExtraFee>> GetAll()
        {
            var allFees = _weatherPhenomenonExtraFeeService.FindAll();
            return Ok(allFees);
        }

        [HttpPut("update")]
        public ActionResult<WeatherPhenomenonExtraFee?> UpdateFee(string vehicle, string phenomenon, decimal? price, bool? forbitten)
        {
            var vehicleEnum = _weatherPhenomenonExtraFeeService.ConvertVehicleTypeToEnum(vehicle);

            if (vehicleEnum == null)
            {
                return BadRequest("Wrong vehicle type, please choose right one (Car, Scooter or Bike).");
            }

            var phenomenonFee = _weatherPhenomenonExtraFeeService.FindByVehicleTypeAndPhenomenon(phenomenon, (VehicleEnum)vehicleEnum);

            if (phenomenonFee == null)
            {
                return NotFound("There is no such WeatherPhenomenonExtraFee with this phenomenon and vehicle type.");
            }

            var updatedFee = _weatherPhenomenonExtraFeeService.UpdateFee(phenomenonFee, price, forbitten);
            return Ok(updatedFee.Result);
        }

        [HttpPost("create")]
        public ActionResult CreateFee(string vehicle, string phenomenon, decimal? price, bool? forbitten)
        {
            var vehicleEnum = _weatherPhenomenonExtraFeeService.ConvertVehicleTypeToEnum(vehicle);

            if (vehicleEnum == null)
            {
                return BadRequest("Wrong vehicle type. Please choose a valid one: Car, Scooter, or Bike.");
            }

            var phenomenonFee = _weatherPhenomenonExtraFeeService.FindByVehicleTypeAndPhenomenon(phenomenon, (VehicleEnum)vehicleEnum);

            if (phenomenonFee != null)
            {
                return BadRequest("Duplicate value detected. A fee with these parameters already exists. If you want to update it, please use another endpoint.");
            }

            var createdFee = _weatherPhenomenonExtraFeeService.CreateFee((VehicleEnum)vehicleEnum, phenomenon, price, forbitten);
            return CreatedAtAction(nameof(CreateFee), createdFee.Result);
        }

        [HttpDelete("delete")]
        public ActionResult DeleteFee(int id)
        {
            var delete = _weatherPhenomenonExtraFeeService.DeleteFee(id);

            if (delete != null)
            {
                return NoContent();
            }

            return BadRequest("No such fee. Maybe was given wrong Id.");
        }
    }
}
