using CarParkingTDS.Extensions.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarParkingTDS.Models.DTOs
{
    /// <summary>
    /// POST	/parking request body
    /// </summary>
    public class ParkRequest
    {
        [Required]
        public required string VehicleReg { get; set; }

        [Required]
        public VehicleType VehicleType { get; set; }
    }
}
