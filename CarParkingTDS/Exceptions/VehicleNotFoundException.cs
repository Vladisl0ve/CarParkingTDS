namespace CarParkingTDS.Exceptions
{
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string vehicleReg) : base($"No vehicle found with registration number {vehicleReg}") { }
    }
}
