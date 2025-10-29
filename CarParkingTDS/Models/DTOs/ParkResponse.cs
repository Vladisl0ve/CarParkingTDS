namespace CarParkingTDS.Models.DTOs
{
    /// <summary>
    /// POST	/parking return body
    /// </summary>
    public class ParkResponse
    {
        public required string VehicleReg { get; set; }
        public int SpaceNumber { get; set; }
        public DateTime TimeIn { get; set; }
    }
}
