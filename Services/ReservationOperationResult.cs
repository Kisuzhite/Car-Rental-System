namespace Car_Rental_System.Services
{
    internal sealed class ReservationOperationResult
    {
        private ReservationOperationResult(
            bool success,
            string title,
            string message,
            ReservationOperationError error = ReservationOperationError.None,
            int? reservationId = null,
            int? rentalId = null)
        {
            Success = success;
            Title = title;
            Message = message;
            Error = error;
            ReservationId = reservationId;
            RentalId = rentalId;
        }

        public bool Success { get; }
        public string Title { get; }
        public string Message { get; }
        public ReservationOperationError Error { get; }
        public bool HasScheduleConflict => Error == ReservationOperationError.ScheduleConflict;
        public bool CarWasUnavailable => Error == ReservationOperationError.CarNotAvailable;
        public int? ReservationId { get; }
        public int? RentalId { get; }

        public static ReservationOperationResult Ok(
            string title,
            string message,
            int? reservationId = null,
            int? rentalId = null)
        {
            return new ReservationOperationResult(
                true,
                title,
                message,
                reservationId: reservationId,
                rentalId: rentalId);
        }

        public static ReservationOperationResult Fail(
            ReservationOperationError error,
            string title,
            string message)
        {
            return new ReservationOperationResult(false, title, message, error);
        }
    }
}
