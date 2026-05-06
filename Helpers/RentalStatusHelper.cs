using Car_Rental_System.Models;

namespace Car_Rental_System.Helpers
{
    internal static class RentalStatusHelper
    {
        public static string GetDisplayStatus(Rental rental, DateOnly today)
        {
            if (rental.Status == AppConstants.ActiveRentalStatus &&
                rental.ExpectedReturnDate < today)
            {
                return AppConstants.OverdueRentalStatus;
            }

            return rental.Status;
        }

        public static int GetLateDays(Rental rental, DateOnly today)
        {
            DateOnly? actualReturnDate = rental.ActualReturnDate;
            DateOnly comparisonDate = actualReturnDate ?? today;

            if (rental.Status == AppConstants.ActiveRentalStatus ||
                rental.Status == AppConstants.ReturnedRentalStatus)
            {
                return Math.Max(0, comparisonDate.DayNumber - rental.ExpectedReturnDate.DayNumber);
            }

            return 0;
        }

        public static decimal GetLatePenalty(Rental rental, DateOnly today)
        {
            return GetLateDays(rental, today) * rental.DailyRate;
        }
    }
}
