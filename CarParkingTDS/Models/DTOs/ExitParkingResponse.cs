namespace CarParkingTDS.Models.DTOs
{
    /// <summary>
    /// POST	/parking/exit	return body
    /// </summary>
    public class ExitParkingResponse
    {
        public required string VehicleReg { get; set; }
        public double VehicleCharge { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
    }
}
