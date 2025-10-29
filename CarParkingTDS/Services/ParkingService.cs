using CarParkingTDS.Exceptions;
using CarParkingTDS.Extensions.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarParkingTDS.Services
{
    public class ParkingService(IServiceProvider serviceProvider, TimeProvider timeProvider) : DBServiceBase(serviceProvider)
    {
        public async Task<(int availableSpaces, int occupiedSpaces)> GetParkingStatus()
        {
            var occupied = await ParkingContext.ParkingSpaces.CountAsync(ps => ps.IsOccupied);
            var available = await ParkingContext.ParkingSpaces.CountAsync(ps => !ps.IsOccupied);

            return (available, occupied);
        }

        public async Task<bool> IsVehicleParked(string vehicleReg)
        {
            return await ParkingContext.ParkingSpaces.AnyAsync(p => p.VehicleRegistrationNumber == vehicleReg);
        }

        public async Task<(string vehicleReg, int spaceNumber, DateTime timeIn)> ParkVehicle(string vehicleReg, VehicleType vehicleType)
        {
            var placeToPark = await ParkingContext.ParkingSpaces
                            .FirstOrDefaultAsync(ps => !ps.IsOccupied) ?? throw new NoAvailableParkingSpacesException();

            placeToPark.IsOccupied = true;
            placeToPark.VehicleType = vehicleType;
            placeToPark.VehicleRegistrationNumber = vehicleReg;
            placeToPark.TimeIn = timeProvider.GetUtcNow().UtcDateTime;
            await ParkingContext.SaveChangesAsync();

            return (vehicleReg, placeToPark.SpaceNumber, placeToPark.TimeIn.Value);
        }

        public async Task<(string vehicleReg, double vehicleCharge, DateTime timeIn, DateTime timeOut)> ExitVehicle(string vehicleReg)
        {
            var parkedSpace = await ParkingContext.ParkingSpaces
                .FirstOrDefaultAsync(ps => ps.VehicleRegistrationNumber == vehicleReg) ?? throw new VehicleNotFoundException(vehicleReg);

            var timeIn = parkedSpace.TimeIn ?? throw new NullReferenceException("TimeIn must not be null");
            var timeOut = timeProvider.GetUtcNow().UtcDateTime;

            var totalCharge = CalculateParkingCost(timeIn, timeOut, parkedSpace.VehicleType);

            parkedSpace.IsOccupied = false;
            parkedSpace.VehicleType = null;
            parkedSpace.VehicleRegistrationNumber = null;
            parkedSpace.TimeIn = null;

            await ParkingContext.SaveChangesAsync();

            return (vehicleReg, totalCharge, timeIn, timeOut);
        }

        private static double CalculateParkingCost(DateTime timeIn, DateTime timeOut, VehicleType? vehicleType)
        {
            var minutesParked = (timeOut - timeIn).TotalMinutes;

            double costPerMinute = vehicleType switch
            {
                VehicleType.SmallCar => 0.10,
                VehicleType.MediumCar => 0.20,
                VehicleType.LargeCar => 0.40,
                _ => throw new Exception($"Unsupported vehicle type: {vehicleType}")
            };

            // Every 5 minutes an additional charge of £1 will be added
            double additionalCharge = Math.Floor(minutesParked / 5) * 1;

            return Math.Round((minutesParked * costPerMinute) + additionalCharge, 2);
        }
    }
}
