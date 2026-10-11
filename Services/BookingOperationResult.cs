namespace Car_Rental_System.Services
{
    internal enum BookingOperationKind
    {
        None,
        Rental,
        Reservation
    }

    internal enum BookingOperationError
    {
        None,
        InvalidDates,
        InvalidCustomer,
        InvalidPaymentAmount,
        CustomerHasActiveRental,
        CarNotAvailable,
        ScheduleConflict
    }

    internal sealed class BookingOperationResult
    {
        private BookingOperationResult(
            bool success,
            string title,
            string message,
            BookingOperationKind kind = BookingOperationKind.None,
            BookingOperationError error = BookingOperationError.None,
            int? reservationId = null,
            int? rentalId = null,
            decimal totalAmount = 0,
            decimal amountPaid = 0,
            decimal balance = 0)
        {
            Success = success;
            Title = title;
            Message = message;
            Kind = kind;
            Error = error;
            ReservationId = reservationId;
            RentalId = rentalId;
            TotalAmount = totalAmount;
            AmountPaid = amountPaid;
            Balance = balance;
        }

        public bool Success { get; }
        public string Title { get; }
        public string Message { get; }
        public BookingOperationKind Kind { get; }
        public BookingOperationError Error { get; }
        public bool CreatedRental => Kind == BookingOperationKind.Rental;
        public bool CreatedReservation => Kind == BookingOperationKind.Reservation;
        public bool CarWasUnavailable => Error == BookingOperationError.CarNotAvailable;
        public bool HasScheduleConflict => Error == BookingOperationError.ScheduleConflict;
        public int? ReservationId { get; }
        public int? RentalId { get; }
        public decimal TotalAmount { get; }
        public decimal AmountPaid { get; }
        public decimal Balance { get; }

        public static BookingOperationResult Ok(
            string title,
            string message,
            BookingOperationKind kind,
            int? reservationId = null,
            int? rentalId = null,
            decimal totalAmount = 0,
            decimal amountPaid = 0,
            decimal balance = 0)
        {
            return new BookingOperationResult(
                true,
                title,
                message,
                kind,
                reservationId: reservationId,
                rentalId: rentalId,
                totalAmount: totalAmount,
                amountPaid: amountPaid,
                balance: balance);
        }

        public static BookingOperationResult Fail(
            BookingOperationError error,
            string title,
            string message)
        {
            return new BookingOperationResult(false, title, message, error: error);
        }
    }
}
