using System.Text.RegularExpressions;

namespace Car_Rental_System.Helpers
{
    internal static partial class ValidationHelper
    {
        public static string NormalizePlateNumber(string plateNumber)
        {
            return CollapseSpaces(plateNumber).ToUpperInvariant();
        }

        public static string NormalizeLicenseNumber(string licenseNumber)
        {
            return CollapseSpaces(licenseNumber).ToUpperInvariant();
        }

        public static bool IsValidPersonName(string name)
        {
            string normalized = CollapseSpaces(name);
            return normalized.Length >= 2 &&
                normalized.Length <= 100 &&
                PersonNameRegex().IsMatch(normalized);
        }

        public static bool IsValidContactNumber(string contactNumber)
        {
            string normalized = CollapseSpaces(contactNumber);
            int digitCount = normalized.Count(char.IsDigit);

            return digitCount is >= 7 and <= 15 &&
                ContactNumberRegex().IsMatch(normalized);
        }

        public static bool IsValidCarTypeName(string typeName)
        {
            string normalized = CollapseSpaces(typeName);

            return normalized.Length is >= 2 and <= 50 &&
                CarTypeNameRegex().IsMatch(normalized);
        }

        public static bool IsValidPlateNumber(string plateNumber)
        {
            string normalized = NormalizePlateNumber(plateNumber);

            return normalized.Length is >= 3 and <= 20 &&
                normalized.Any(char.IsLetter) &&
                normalized.Any(char.IsDigit) &&
                PlateNumberRegex().IsMatch(normalized);
        }

        public static bool IsValidLicenseNumber(string licenseNumber, bool allowNotProvided = false)
        {
            string normalized = NormalizeLicenseNumber(licenseNumber);

            if (allowNotProvided &&
                string.Equals(normalized, AppConstants.NotProvided, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return normalized.Length is >= 3 and <= 50 &&
                normalized.Any(char.IsLetterOrDigit) &&
                LicenseNumberRegex().IsMatch(normalized);
        }

        public static bool IsValidDailyRate(decimal dailyRate)
        {
            return dailyRate >= AppConstants.MinimumDailyRate &&
                dailyRate <= AppConstants.MaximumDailyRate;
        }

        public static bool TryValidateRentalDates(DateTime rentalDate, DateTime expectedReturnDate, out string message)
        {
            message = "";

            if (expectedReturnDate.Date < rentalDate.Date)
            {
                message = "Return date cannot be earlier than rental date.";
                return false;
            }

            int rentalDays = (expectedReturnDate.Date - rentalDate.Date).Days;

            if (rentalDays > AppConstants.MaximumRentalDays)
            {
                message = $"Rental period cannot be longer than {AppConstants.MaximumRentalDays} days.";
                return false;
            }

            return true;
        }

        public static string CollapseSpaces(string value)
        {
            return WhitespaceRegex().Replace(value.Trim(), " ");
        }

        [GeneratedRegex(@"^[\p{L}][\p{L} .'-]*$")]
        private static partial Regex PersonNameRegex();

        [GeneratedRegex(@"^[0-9+\-(). ]+$")]
        private static partial Regex ContactNumberRegex();

        [GeneratedRegex(@"^[A-Za-z0-9][A-Za-z0-9 /&-]*$")]
        private static partial Regex CarTypeNameRegex();

        [GeneratedRegex(@"^[A-Z0-9][A-Z0-9 -]*[A-Z0-9]$")]
        private static partial Regex PlateNumberRegex();

        [GeneratedRegex(@"^[A-Z0-9 -]+$")]
        private static partial Regex LicenseNumberRegex();

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();
    }
}
