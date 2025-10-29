namespace CarParkingTDS.Models.DTOs
{
    /// <summary>
    /// GET	/parking return body
    /// </summary>
    public class ParkingStatusResponse
    {
        public int AvailableSpaces { get; set; }
        public int OccupiedSpaces { get; set; }
    }
}
