using CarParkingTDS.Extensions.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarParkingTDS.Models
{
    [Table("TDS_PARKING_SPACES")]
    public class ParkingSpace
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("SPACE_NUMBER")]
        public int SpaceNumber { get; set; }

        [Column("IS_OCCUPIED")]
        public bool IsOccupied { get; set; }

        [Column("VEHICLE_TYPE")]
        public VehicleType? VehicleType { get; set; }

        [Column("VEHICLE_REG")]
        public string? VehicleRegistrationNumber { get; set; }

        [Column("TIME_IN")]
        public DateTime? TimeIn { get; set; }
    }
}
