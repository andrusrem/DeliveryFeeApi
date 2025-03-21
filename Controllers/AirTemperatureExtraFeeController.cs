using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirTemperatureExtraFeeController : ControllerBase
    {
        private readonly IAirTemperatureExtraFeeService _airTemperatureExtraFeeService;

        public AirTemperatureExtraFeeController(IAirTemperatureExtraFeeService airTemperatureExtraFeeService)
        {
            _airTemperatureExtraFeeService = airTemperatureExtraFeeService;
        }

        [HttpGet("all")]
        public ActionResult<List<AirTemperatureExtraFee>> GetAll()
        {
            var allFees = _airTemperatureExtraFeeService.FindAll();
            return Ok(allFees);
        }

        [HttpPut("update")]
        public ActionResult<AirTemperatureExtraFee?> UpdateFee(string vehicle, decimal lower, decimal upper, decimal price)
        {
            var vehicleEnum = _airTemperatureExtraFeeService.ConvertVehicleTypeToEnum(vehicle);
            
            if (vehicleEnum == null)
            {
                return BadRequest("Wrong vehicle type, please choose right one (Car, Scooter or Bike).");
            }

            var airFee = _airTemperatureExtraFeeService.FindByVehicleTypeLowerAndUpperPoints(lower, upper, (VehicleEnum)vehicleEnum);

            if (airFee == null)
            {
                return NotFound("There is no such AirTemperatureFee with this lower and upper temperatures.");
            }

            var updatedFee = _airTemperatureExtraFeeService.UpdateFee(airFee, price);
            return Ok(updatedFee.Result);
        }
        [HttpPost("create")]
        public ActionResult CreateFee(string vehicle, decimal lower, decimal upper, decimal price)
        {
            var vehicleEnum = _airTemperatureExtraFeeService.ConvertVehicleTypeToEnum(vehicle);

            if (vehicleEnum == null)
            {
                return BadRequest("Wrong vehicle type. Please choose a valid one: Car, Scooter, or Bike.");
            }

            var airFee = _airTemperatureExtraFeeService.FindByVehicleTypeLowerAndUpperPoints(lower, upper, (VehicleEnum)vehicleEnum);

            if (airFee != null)
            {
                return BadRequest("Duplicate value detected. A fee with these parameters already exists. If you want to update it, please use another endpoint.");
            }

            var createdFee = _airTemperatureExtraFeeService.CreateFee((VehicleEnum)vehicleEnum, lower, upper, price);
            return CreatedAtAction(nameof(CreateFee), createdFee.Result);
        }

        [HttpDelete("delete")]
        public ActionResult DeleteFee(int id)
        {
            var delete = _airTemperatureExtraFeeService.DeleteFee(id);

            if (delete != null)
            {
                return NoContent();
            }

            return BadRequest("No such fee. Maybe was given wrong Id.");
        }
    }
}
