using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindSpeedExtraFeeController : ControllerBase
    {
        private readonly IWindSpeedExtraFeeService _windSpeedExtraFeeService;

        public WindSpeedExtraFeeController(IWindSpeedExtraFeeService windSpeedExtraFeeService)
        {
            _windSpeedExtraFeeService = windSpeedExtraFeeService;
        }

        [HttpGet("all")]
        public ActionResult<List<WindSpeedExtraFee>> GetAll()
        {
            var allFees = _windSpeedExtraFeeService.FindAll();
            return Ok(allFees);
        }

        [HttpPut("update")]
        public ActionResult<WindSpeedExtraFee?> UpdateFee(string vehicle, decimal lower, decimal? upper, decimal? price, bool? forbitten)
        {
            var vehicleEnum = _windSpeedExtraFeeService.ConvertVehicleTypeToEnum(vehicle);

            if (vehicleEnum == null)
            {
                return BadRequest("Wrong vehicle type, please choose right one (Car, Scooter or Bike).");
            }

            var windSpeedFee = _windSpeedExtraFeeService.FindByVehicleTypeLowerAndUpperSpeed(lower, upper, (VehicleEnum)vehicleEnum);

            if (windSpeedFee == null)
            {
                return NotFound("There is no such WindSpeedExtraFee with this lower and upper temperatures.");
            }

            var updatedFee = _windSpeedExtraFeeService.UpdateFee(windSpeedFee, price, forbitten);
            return Ok(updatedFee.Result);
        }

        [HttpPost("create")]
        public ActionResult CreateFee(string vehicle, decimal lower, decimal? upper, decimal? price, bool? forbitten)
        {
            var vehicleEnum = _windSpeedExtraFeeService.ConvertVehicleTypeToEnum(vehicle);

            if (vehicleEnum == null)
            {
                return BadRequest("Wrong vehicle type. Please choose a valid one: Car, Scooter, or Bike.");
            }

            var windSpeedFee = _windSpeedExtraFeeService.FindByVehicleTypeLowerAndUpperSpeed(lower, upper, (VehicleEnum)vehicleEnum);

            if (windSpeedFee != null)
            {
                return BadRequest("Duplicate value detected. A fee with these parameters already exists. If you want to update it, please use another endpoint.");
            }

            var createdFee =_windSpeedExtraFeeService.CreateFee((VehicleEnum)vehicleEnum, lower, upper, price, forbitten);
            return CreatedAtAction(nameof(CreateFee), createdFee.Result);
        }

        [HttpDelete("delete")]
        public ActionResult DeleteFee(int id)
        {
            var delete = _windSpeedExtraFeeService.DeleteFee(id);

            if (delete != null)
            {
                return NoContent();
            }

            return BadRequest("No such fee. Maybe was given wrong Id.");
        }
    }
}
