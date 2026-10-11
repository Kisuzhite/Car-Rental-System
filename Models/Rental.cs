using System.ComponentModel.DataAnnotations.Schema;
using Car_Rental_System.Helpers;

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
        public decimal AmountPaid { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public RentalStatus Status { get; set; } = RentalStatus.Active;

        public Customer Customer { get; set; } = null!;
        public Car Car { get; set; } = null!;
        public ICollection<RentalPayment> Payments { get; set; } = new List<RentalPayment>();

        [NotMapped]
        public decimal Balance => PaymentHelper.GetBalance(TotalAmount, AmountPaid);
    }
}
