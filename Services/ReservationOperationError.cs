namespace Car_Rental_System.Services
{
    internal enum ReservationOperationError
    {
        None,
        ReservationNotFound,
        ReservationCannotConvert,
        PickupDateNotReached,
        CustomerHasActiveRental,
        CarNotAvailable,
        InvalidPaymentAmount,
        ScheduleConflict
    }
}
