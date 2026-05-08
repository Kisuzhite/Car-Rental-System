namespace Car_Rental_System.Services
{
    internal enum RentalOperationError
    {
        None,
        RentalAlreadyClosed,
        InvalidReturnDate,
        CustomerHasActiveRental,
        CarNotAvailable,
        InvalidPaymentAmount
    }
}
