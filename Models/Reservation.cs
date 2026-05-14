namespace Car_Rental_System.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int? CustomerId { get; set; }
        public int CarId { get; set; }
        public string CustomerName { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public DateOnly ReservationDate { get; set; }
        public DateOnly PickupDate { get; set; }
        public DateOnly ExpectedReturnDate { get; set; }
        public decimal DepositAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public string PaymentReferenceNumber { get; set; } = "";
        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;

        public Customer? Customer { get; set; }
        public Car Car { get; set; } = null!;
    }
}
