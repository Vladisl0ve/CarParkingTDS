using Microsoft.EntityFrameworkCore;

namespace CarParkingTDS.Models.DbContexts
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
    }
}
