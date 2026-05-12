namespace Car_Rental_System.Services
{
    internal sealed class RentalOperationResult
    {
        private RentalOperationResult(
            bool success,
            string title,
            string message,
            RentalOperationError error = RentalOperationError.None,
            int? rentalId = null,
            decimal dailyRate = 0,
            decimal totalAmount = 0,
            decimal amountPaid = 0,
            decimal balance = 0,
            int lateDays = 0)
        {
            Success = success;
            Title = title;
            Message = message;
            Error = error;
            RentalId = rentalId;
            DailyRate = dailyRate;
            TotalAmount = totalAmount;
            AmountPaid = amountPaid;
            Balance = balance;
            LateDays = lateDays;
        }

        public bool Success { get; }
        public string Title { get; }
        public string Message { get; }
        public RentalOperationError Error { get; }
        public bool CarWasUnavailable => Error == RentalOperationError.CarNotAvailable;
        public bool RentalWasAlreadyClosed => Error == RentalOperationError.RentalAlreadyClosed;
        public bool HasScheduleConflict => Error == RentalOperationError.ScheduleConflict;
        public int? RentalId { get; }
        public decimal DailyRate { get; }
        public decimal TotalAmount { get; }
        public decimal AmountPaid { get; }
        public decimal Balance { get; }
        public int LateDays { get; }

        public static RentalOperationResult Ok(
            string title,
            string message,
            int? rentalId = null,
            decimal dailyRate = 0,
            decimal totalAmount = 0,
            decimal amountPaid = 0,
            decimal balance = 0,
            int lateDays = 0)
        {
            return new RentalOperationResult(
                true,
                title,
                message,
                rentalId: rentalId,
                dailyRate: dailyRate,
                totalAmount: totalAmount,
                amountPaid: amountPaid,
                balance: balance,
                lateDays: lateDays);
        }

        public static RentalOperationResult Fail(
            RentalOperationError error,
            string title,
            string message)
        {
            return new RentalOperationResult(false, title, message, error);
        }
    }
}
