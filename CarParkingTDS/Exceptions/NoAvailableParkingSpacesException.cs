namespace CarParkingTDS.Exceptions
{
    public class NoAvailableParkingSpacesException : Exception
    {
        public NoAvailableParkingSpacesException() : base("No available parking spaces.") { }
    }
}
