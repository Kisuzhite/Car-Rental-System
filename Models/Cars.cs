using System;
using System.Collections.Generic;
using System.Text;

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
        public string Status { get; set; } = "";
        public string? ImagePath { get; set; }

        public ICollection<Rental> Rentals { get; set; } = [];
    }
}
