using System;
using System.Collections.Generic;
using System.Text;

namespace Car_Rental_System.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string LicenseNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public bool IsArchived { get; set; }

        public ICollection<Rental> Rentals { get; set; } = [];
    }
}
