namespace Car_Rental_System
{
    internal static class RentalCalculator
    {
        public static int GetBillableDays(DateTime rentalDate, DateTime expectedReturnDate)
        {
            return Math.Max(1, (expectedReturnDate.Date - rentalDate.Date).Days);
        }

        public static decimal CalculateBaseAmount(DateTime rentalDate, DateTime expectedReturnDate, decimal dailyRate)
        {
            return GetBillableDays(rentalDate, expectedReturnDate) * dailyRate;
        }

        public static int GetLateDays(DateTime expectedReturnDate, DateTime actualReturnDate)
        {
            return Math.Max(0, (actualReturnDate.Date - expectedReturnDate.Date).Days);
        }

        public static decimal CalculateFinalAmount(
            DateTime expectedReturnDate,
            DateTime actualReturnDate,
            decimal dailyRate,
            decimal baseAmount)
        {
            return baseAmount + GetLateDays(expectedReturnDate, actualReturnDate) * dailyRate;
        }

        public static string FormatCurrency(decimal amount)
        {
            return $"{AppConstants.CurrencyPrefix}{amount:N2}";
        }
    }
}
