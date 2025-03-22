using DeliveryFeeApi.Data;
using DeliveryFeeApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace DeliveryFeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionalBaseFeeController : ControllerBase
    {
        private readonly IRegionalBaseFeeService _regionalBaseFeeService;

        public RegionalBaseFeeController(IRegionalBaseFeeService regionalBaseFeeService)
        {
            _regionalBaseFeeService = regionalBaseFeeService;
        }

        [HttpGet("all")]
        public ActionResult<List<RegionalBaseFee>> GetAll()
        {
            var allFees = _regionalBaseFeeService.FindAll();
            return Ok(allFees);
        }

        [HttpPut("update")]
        public ActionResult<RegionalBaseFee?> UpdateFee(string vehicle, string station, decimal price)
        {
            var vehicleEnum = _regionalBaseFeeService.ConvertVehicleTypeToEnum(vehicle);
            var stationEnum = _regionalBaseFeeService.ConvertStationNameToEnum(station);

            if (vehicleEnum == null || stationEnum == null)
            {
                return BadRequest("Wrong vehicle type or station name, please choose right one station(Tallinn, Tartu or Pärnu), vehicle(Car, Scooter or Bike).");
            }

            var baseFee = _regionalBaseFeeService.FindByVehicleTypeAndStationName((StationEnum) stationEnum, (VehicleEnum)vehicleEnum);

            if (baseFee == null)
            {
                return NotFound("There is no such RegionalBaseFee with this station or.");
            }

            var updatedFee = _regionalBaseFeeService.UpdateFee(baseFee, price);
            return Ok(updatedFee.Result);
        }
        [HttpPost("create")]
        public ActionResult CreateFee(string vehicle, string station, decimal price)
        {
            var vehicleEnum = _regionalBaseFeeService.ConvertVehicleTypeToEnum(vehicle);
            var stationEnum = _regionalBaseFeeService.ConvertStationNameToEnum(station);

            if (vehicleEnum == null || stationEnum == null)
            {
                return BadRequest("Wrong vehicle type or station name, please choose right one station(Tallinn, Tartu or Pärnu), vehicle(Car, Scooter or Bike).");
            }

            var baseFee = _regionalBaseFeeService.FindByVehicleTypeAndStationName((StationEnum)stationEnum, (VehicleEnum)vehicleEnum);

            if (baseFee != null)
            {
                return BadRequest("Duplicate value detected. A fee with these parameters already exists. If you want to update it, please use another endpoint.");
            }

            var createdFee = _regionalBaseFeeService.CreateFee((VehicleEnum)vehicleEnum, (StationEnum)stationEnum, price);
            return CreatedAtAction(nameof(CreateFee), createdFee.Result);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteFee(int id)
        {
            var delete = await _regionalBaseFeeService.DeleteFee(id);

            if (delete != null)
            {
                return NoContent();
            }

            return NotFound("No such fee. Maybe was given wrong Id.");
        }
    }
}
