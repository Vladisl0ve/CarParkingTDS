using CarParkingTDS.Exceptions;
using CarParkingTDS.Models.DTOs;
using CarParkingTDS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarParkingTDS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController(ParkingService parkingService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ParkingStatusResponse>> GetParkingStatus()
        {
            try
            {
                var (availableSpaces, occupiedSpaces) = await parkingService.GetParkingStatus();
                ParkingStatusResponse response = new()
                {
                    AvailableSpaces = availableSpaces,
                    OccupiedSpaces = occupiedSpaces
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ParkResponse>> ParkVehicle(ParkRequest parkRequest)
        {
            if (!Enum.IsDefined(parkRequest.VehicleType))
                ModelState.AddModelError("VehicleType", "Invalid vehicle type.");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var isThisVehicleAlreadyParked = await parkingService.IsVehicleParked(parkRequest.VehicleReg);
                if (isThisVehicleAlreadyParked)
                    return Conflict($"Vehicle with registration number {parkRequest.VehicleReg} is already parked");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


            try
            {
                var (vehicleReg, spaceNumber, timeIn) = await parkingService.ParkVehicle(parkRequest.VehicleReg, parkRequest.VehicleType);
                var response = new ParkResponse()
                {
                    VehicleReg = vehicleReg,
                    SpaceNumber = spaceNumber,
                    TimeIn = timeIn
                };

                return Ok(response);
            }
            catch (NoAvailableParkingSpacesException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("exit")]
        public async Task<ActionResult<ExitParkingResponse>> ExitVehicle(ExitParkingRequest exitParkingRequest)
        {
            try
            {
                var (vehicleReg, vehicleCharge, timeIn, timeOut) = await parkingService.ExitVehicle(exitParkingRequest.VehicleReg);
                var response = new ExitParkingResponse()
                {
                    VehicleReg = vehicleReg,
                    VehicleCharge = vehicleCharge,
                    TimeIn = timeIn,
                    TimeOut = timeOut
                };
                return Ok(response);
            }
            catch (VehicleNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
