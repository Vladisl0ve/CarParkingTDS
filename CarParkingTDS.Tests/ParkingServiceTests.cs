using CarParkingTDS.Exceptions;
using CarParkingTDS.Extensions.Enums;
using CarParkingTDS.Models;
using CarParkingTDS.Models.DbContexts;
using CarParkingTDS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace CarParkingTDS.Tests
{
    public class ParkingServiceTests
    {
        private static ParkingContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ParkingContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ParkingContext(options);
        }

        [Fact]
        public async Task GetParkingStatus_ReturnCorrectAvailableSpacesAndOccupiedSpacesCounts()
        {
            // Arrange
            var context = CreateInMemoryContext();
            context.ParkingSpaces.AddRange(
                new ParkingSpace { SpaceNumber = 1, IsOccupied = false },
                new ParkingSpace { SpaceNumber = 2, IsOccupied = true, TimeIn = new DateTime(2025, 10, 29, 10, 00, 00), VehicleRegistrationNumber = "RegNumber_1", VehicleType = VehicleType.SmallCar },
                new ParkingSpace { SpaceNumber = 3, IsOccupied = false },
                new ParkingSpace { SpaceNumber = 4, IsOccupied = true, TimeIn = new DateTime(2025, 10, 29, 09, 00, 00), VehicleRegistrationNumber = "RegNumber_2", VehicleType = VehicleType.LargeCar }
            );
            await context.SaveChangesAsync();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(context)
                .BuildServiceProvider();

            var fakeTime = new DateTimeOffset(2025, 10, 29, 12, 0, 0, TimeSpan.Zero);
            var fakeTimeProvider = new FakeTimeProvider(fakeTime);

            var parkingService = new ParkingService(serviceProvider, fakeTimeProvider);

            // Act
            var (availableSpaces, occupiedSpaces) = await parkingService.GetParkingStatus();

            // Assert
            Assert.Equal(2, availableSpaces);
            Assert.Equal(2, occupiedSpaces);
        }

        [Fact]
        public async Task ParkVehicle_ShouldThrow_NoAvaliableSpaces()
        {
            // Arrange
            var context = CreateInMemoryContext();
            context.ParkingSpaces.AddRange(
                new ParkingSpace { SpaceNumber = 2, IsOccupied = true, TimeIn = new DateTime(2025, 10, 29, 10, 00, 00), VehicleRegistrationNumber = "RegNumber_1", VehicleType = VehicleType.SmallCar },
                new ParkingSpace { SpaceNumber = 4, IsOccupied = true, TimeIn = new DateTime(2025, 10, 29, 09, 00, 00), VehicleRegistrationNumber = "RegNumber_2", VehicleType = VehicleType.LargeCar }
            );
            await context.SaveChangesAsync();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(context)
                .BuildServiceProvider();

            var fakeTime = new DateTimeOffset(2025, 10, 29, 12, 0, 0, TimeSpan.Zero);
            var fakeTimeProvider = new FakeTimeProvider(fakeTime);

            var parkingService = new ParkingService(serviceProvider, fakeTimeProvider);

            //Act
            var exception = await Assert.ThrowsAsync<NoAvailableParkingSpacesException>(async () =>
            {
                await parkingService.ParkVehicle("RegNumber_X", VehicleType.LargeCar);
            });

            //Assert
            Assert.Equal("No available parking spaces.", exception.Message);
        }

        [Fact]
        public async Task ExitVehicle_ReturnCorrectCharge()
        {
            // Arrange
            var context = CreateInMemoryContext();
            context.ParkingSpaces.AddRange(
                new ParkingSpace { SpaceNumber = 2, IsOccupied = true, TimeIn = new DateTime(2025, 10, 29, 10, 00, 00), VehicleRegistrationNumber = "RegNumber_1", VehicleType = VehicleType.SmallCar }
            );
            await context.SaveChangesAsync();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(context)
                .BuildServiceProvider();

            var fakeTime = new DateTimeOffset(2025, 10, 29, 10, 16, 0, TimeSpan.Zero);
            var fakeTimeProvider = new FakeTimeProvider(fakeTime);

            var parkingService = new ParkingService(serviceProvider, fakeTimeProvider);

            //Act
            var (vehicleReg, vehicleCharge, timeIn, timeOut) = await parkingService.ExitVehicle("RegNumber_1");

            //Assert
            Assert.Equal(4.6, vehicleCharge); //16 mins == £4.6 == £0.1/minute * 16 + 16/3 == 0.1 * 16 + 5
        }
    }
}
