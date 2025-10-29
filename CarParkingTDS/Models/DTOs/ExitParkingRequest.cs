using System.ComponentModel.DataAnnotations;

namespace CarParkingTDS.Models.DTOs
{
    /// <summary>
    /// POST	/parking/exit	request body
    /// </summary>
    public class ExitParkingRequest
    {
        [Required]
        public required string VehicleReg { get; set; }
    }
}
