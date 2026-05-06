namespace Car_Rental_System.Services
{
    internal sealed class RentalOperationResult
    {
        private RentalOperationResult(
            bool success,
            string title,
            string message,
            int? rentalId = null,
            decimal dailyRate = 0,
            decimal totalAmount = 0,
            int lateDays = 0)
        {
            Success = success;
            Title = title;
            Message = message;
            RentalId = rentalId;
            DailyRate = dailyRate;
            TotalAmount = totalAmount;
            LateDays = lateDays;
        }

        public bool Success { get; }
        public string Title { get; }
        public string Message { get; }
        public int? RentalId { get; }
        public decimal DailyRate { get; }
        public decimal TotalAmount { get; }
        public int LateDays { get; }

        public static RentalOperationResult Ok(
            string title,
            string message,
            int? rentalId = null,
            decimal dailyRate = 0,
            decimal totalAmount = 0,
            int lateDays = 0)
        {
            return new RentalOperationResult(true, title, message, rentalId, dailyRate, totalAmount, lateDays);
        }

        public static RentalOperationResult Fail(string title, string message)
        {
            return new RentalOperationResult(false, title, message);
        }
    }
}
