namespace Car_Rental_System.Helpers
{
    internal static class RentalCalculator
    {
        public static DateOnly ToDateOnly(DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime.Date);
        }

        public static int GetBillableDays(DateTime rentalDate, DateTime expectedReturnDate)
        {
            return Math.Max(1, (expectedReturnDate.Date - rentalDate.Date).Days);
        }

        public static int GetBillableDays(DateOnly rentalDate, DateOnly expectedReturnDate)
        {
            return Math.Max(1, expectedReturnDate.DayNumber - rentalDate.DayNumber);
        }

        public static decimal CalculateBaseAmount(DateTime rentalDate, DateTime expectedReturnDate, decimal dailyRate)
        {
            return GetBillableDays(rentalDate, expectedReturnDate) * dailyRate;
        }

        public static decimal CalculateBaseAmount(DateOnly rentalDate, DateOnly expectedReturnDate, decimal dailyRate)
        {
            return GetBillableDays(rentalDate, expectedReturnDate) * dailyRate;
        }

        public static int GetLateDays(DateTime expectedReturnDate, DateTime actualReturnDate)
        {
            return Math.Max(0, (actualReturnDate.Date - expectedReturnDate.Date).Days);
        }

        public static int GetLateDays(DateOnly expectedReturnDate, DateOnly actualReturnDate)
        {
            return Math.Max(0, actualReturnDate.DayNumber - expectedReturnDate.DayNumber);
        }

        public static int GetLateDays(DateOnly expectedReturnDate, DateTime actualReturnDate)
        {
            return GetLateDays(expectedReturnDate, ToDateOnly(actualReturnDate));
        }

        public static decimal CalculateFinalAmount(
            DateTime expectedReturnDate,
            DateTime actualReturnDate,
            decimal dailyRate,
            decimal baseAmount)
        {
            return baseAmount + GetLateDays(expectedReturnDate, actualReturnDate) * dailyRate;
        }

        public static decimal CalculateFinalAmount(
            DateOnly expectedReturnDate,
            DateOnly actualReturnDate,
            decimal dailyRate,
            decimal baseAmount)
        {
            return baseAmount + GetLateDays(expectedReturnDate, actualReturnDate) * dailyRate;
        }

        public static decimal CalculateFinalAmount(
            DateOnly expectedReturnDate,
            DateTime actualReturnDate,
            decimal dailyRate,
            decimal baseAmount)
        {
            return CalculateFinalAmount(expectedReturnDate, ToDateOnly(actualReturnDate), dailyRate, baseAmount);
        }

        public static string FormatCurrency(decimal amount)
        {
            return $"{AppConstants.CurrencyPrefix}{amount:N2}";
        }
    }
}
