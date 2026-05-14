namespace Car_Rental_System.Services
{
    internal enum RentalOperationError
    {
        None,
        RentalAlreadyClosed,
        InvalidRentalDate,
        InvalidReturnDate,
        CustomerHasActiveRental,
        CarNotAvailable,
        ScheduleConflict,
        InvalidPaymentAmount
    }
}
