using System.Globalization;

namespace Car_Rental_System.Helpers
{
    internal static class RentalCalculator
    {
        public static string CurrencyFormat => $"{AppConstants.CurrencyPrefix}#,##0.00";

        public static DateOnly ToDateOnly(DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime.Date);
        }

        public static int GetBillableDays(DateTime rentalDate, DateTime expectedReturnDate)
        {
            return Math.Max(1, (expectedReturnDate.Date - rentalDate.Date).Days);
        }

        public static decimal CalculateBaseAmount(DateTime rentalDate, DateTime expectedReturnDate, decimal dailyRate)
        {
            return GetBillableDays(rentalDate, expectedReturnDate) * dailyRate;
        }

        public static int GetLateDays(DateOnly expectedReturnDate, DateTime actualReturnDate)
        {
            DateOnly actualDate = ToDateOnly(actualReturnDate);
            return Math.Max(0, actualDate.DayNumber - expectedReturnDate.DayNumber);
        }

        public static decimal CalculateFinalAmount(
            DateOnly expectedReturnDate,
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

        public static bool TryParseCurrency(string value, out decimal amount)
        {
            string normalized = value
                .Replace(AppConstants.CurrencyPrefix, "", StringComparison.Ordinal)
                .Trim();

            return decimal.TryParse(
                    normalized,
                    NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                    CultureInfo.CurrentCulture,
                    out amount) ||
                decimal.TryParse(
                    normalized,
                    NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                    CultureInfo.InvariantCulture,
                    out amount);
        }
    }
}
