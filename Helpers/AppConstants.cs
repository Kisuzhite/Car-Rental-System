namespace Car_Rental_System.Helpers
{
    internal static class AppConstants
    {
        public const string OverdueRentalStatus = "Overdue";

        public const int MinimumCarYear = 1900;
        public const int MaximumRentalDays = 365;
        public const int MoneyPrecision = 10;
        public const int MoneyScale = 2;
        public const decimal MinimumDailyRate = 100;
        public const decimal MaximumDailyRate = 100000;

        public const string CurrencyPrefix = "P ";
        public const string NotProvided = "Not provided";
        public const string OwnerRole = "Owner";

        public static readonly string DefaultCarImagePath =
            Path.Combine("Assets", "Cars", "default-car.png");
    }
}
