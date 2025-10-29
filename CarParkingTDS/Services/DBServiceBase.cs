using CarParkingTDS.Models.DbContexts;

namespace CarParkingTDS.Services
{
    public class DBServiceBase
    {
        private readonly ParkingContext _parkingContext;

        public ParkingContext ParkingContext => _parkingContext;

        public DBServiceBase(IServiceProvider serviceProvider)
        {
            _parkingContext = serviceProvider.GetRequiredService<ParkingContext>();
        }
    }
}
