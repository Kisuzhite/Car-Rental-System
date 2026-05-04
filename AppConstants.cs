namespace Car_Rental_System
{
    internal static class AppConstants
    {
        public const string AvailableStatus = "Available";
        public const string RentedStatus = "Rented";
        public const string MaintenanceStatus = "Maintenance";
        public const string ActiveRentalStatus = "Active";
        public const string ReturnedRentalStatus = "Returned";

        public const int MinimumCarYear = 1900;
        public const int MoneyPrecision = 10;
        public const int MoneyScale = 2;

        public const string CurrencyPrefix = "P ";
        public const string NotProvided = "Not provided";

        public static readonly string DefaultCarImagePath =
            Path.Combine("Assets", "Cars", "default-car.png");
    }
}
