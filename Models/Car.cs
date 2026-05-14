using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_System.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string PlateNumber { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public string Color { get; set; } = "";
        public decimal DailyRate { get; set; }
        public int? CarTypeId { get; set; }
        public CarStatus Status { get; set; } = CarStatus.Available;
        public string? ImagePath { get; set; }

        public CarType? CarType { get; set; }

        [NotMapped]
        // Requires CarType to be loaded with Include before this value can show a car type name.
        public string CarTypeName => CarType?.TypeName ?? "";

        public ICollection<Rental> Rentals { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
