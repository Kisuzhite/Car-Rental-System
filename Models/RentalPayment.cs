namespace Car_Rental_System.Models
{
    public class RentalPayment
    {
        public int RentalPaymentId { get; set; }
        public int RentalId { get; set; }
        public DateOnly PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public string PaymentReferenceNumber { get; set; } = "";
        public RentalPaymentType PaymentType { get; set; } = RentalPaymentType.Initial;

        public Rental Rental { get; set; } = null!;
    }
}
