using System;
using System.Collections.Generic;
using System.Text;

namespace Car_Rental_System.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        public DateOnly RentalDate { get; set; }
        public DateOnly ExpectedReturnDate { get; set; }
        public DateOnly? ActualReturnDate { get; set; }
        public decimal DailyRate { get; set; }
        public decimal TotalAmount { get; set; }
        public RentalStatus Status { get; set; } = RentalStatus.Active;

        public Customer Customer { get; set; } = null!;
        public Car Car { get; set; } = null!;
    }
}
