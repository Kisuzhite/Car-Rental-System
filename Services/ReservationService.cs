using System.Data;
using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Services
{
    internal sealed class ReservationService
    {
        public ReservationOperationResult ConvertToRental(int reservationId)
        {
            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);

            var reservation = db.Reservations
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                return ReservationOperationResult.Fail(
                    ReservationOperationError.ReservationNotFound,
                    "Reservation Not Found",
                    "This reservation no longer exists.");
            }

            if (reservation.Status is ReservationStatus.Cancelled or ReservationStatus.Converted)
            {
                return ReservationOperationResult.Fail(
                    ReservationOperationError.ReservationCannotConvert,
                    "Cannot Convert",
                    "Only pending or confirmed reservations can be converted to rentals.");
            }

            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);
            if (reservation.PickupDate > today)
            {
                return ReservationOperationResult.Fail(
                    ReservationOperationError.PickupDateNotReached,
                    "Pickup Date Not Reached",
                    "Convert this reservation on the pickup date when the customer takes the car.");
            }

            if (HasCarScheduleConflict(
                    db,
                    reservation.CarId,
                    reservation.PickupDate,
                    reservation.ExpectedReturnDate,
                    reservation.ReservationId))
            {
                return ReservationOperationResult.Fail(
                    ReservationOperationError.ScheduleConflict,
                    "Schedule Conflict",
                    "This car already has another active rental or reservation for those dates.");
            }

            int customerId = GetOrCreateCustomerId(db, reservation.CustomerName, reservation.ContactNumber);

            bool hasActiveRental = db.Rentals.Any(r =>
                r.CustomerId == customerId &&
                r.Status == RentalStatus.Active);

            if (hasActiveRental)
            {
                return ReservationOperationResult.Fail(
                    ReservationOperationError.CustomerHasActiveRental,
                    "Customer Has Active Rental",
                    "This customer already has an active rental. Process the return first before converting this reservation.");
            }

            if (reservation.Car.Status != CarStatus.Available)
            {
                return ReservationOperationResult.Fail(
                    ReservationOperationError.CarNotAvailable,
                    "Car Not Available",
                    "This car is not available right now. Process any active return or choose another car.");
            }

            decimal dailyRate = reservation.Car.CarType?.DailyRate ?? reservation.Car.DailyRate;
            DateTime pickupDate = reservation.PickupDate.ToDateTime(TimeOnly.MinValue);
            DateTime expectedReturnDate = reservation.ExpectedReturnDate.ToDateTime(TimeOnly.MinValue);
            decimal totalAmount = RentalCalculator.CalculateBaseAmount(pickupDate, expectedReturnDate, dailyRate);

            if (!PaymentHelper.IsValidPaymentAmount(reservation.DepositAmount, totalAmount))
            {
                return ReservationOperationResult.Fail(
                    ReservationOperationError.InvalidPaymentAmount,
                    "Invalid Deposit",
                    "The reservation deposit cannot be greater than the rental total.");
            }

            reservation.CustomerId = customerId;
            reservation.Status = ReservationStatus.Converted;
            reservation.Car.Status = CarStatus.Rented;

            var rental = new Rental
            {
                CustomerId = customerId,
                CarId = reservation.CarId,
                RentalDate = reservation.PickupDate,
                ExpectedReturnDate = reservation.ExpectedReturnDate,
                DailyRate = dailyRate,
                TotalAmount = totalAmount,
                AmountPaid = reservation.DepositAmount,
                PaymentStatus = PaymentHelper.GetPaymentStatus(totalAmount, reservation.DepositAmount),
                PaymentMethod = reservation.PaymentMethod,
                Status = RentalStatus.Active
            };

            db.Rentals.Add(rental);
            db.SaveChanges();
            transaction.Commit();

            return ReservationOperationResult.Ok(
                "Converted",
                "Reservation converted to an active rental.",
                reservation.ReservationId,
                rental.RentalId);
        }

        public static bool HasCarScheduleConflict(
            AppDbContext db,
            int carId,
            DateOnly pickupDate,
            DateOnly expectedReturnDate,
            int? excludedReservationId = null)
        {
            bool rentalConflict = db.Rentals.Any(r =>
                r.CarId == carId &&
                r.Status == RentalStatus.Active &&
                pickupDate <= r.ExpectedReturnDate &&
                r.RentalDate <= expectedReturnDate);

            if (rentalConflict)
            {
                return true;
            }

            return db.Reservations.Any(r =>
                r.CarId == carId &&
                (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed) &&
                (!excludedReservationId.HasValue || r.ReservationId != excludedReservationId.Value) &&
                pickupDate <= r.ExpectedReturnDate &&
                r.PickupDate <= expectedReturnDate);
        }

        public static bool DateRangesOverlap(
            DateOnly start,
            DateOnly end,
            DateOnly existingStart,
            DateOnly existingEnd)
        {
            return start <= existingEnd && existingStart <= end;
        }

        private static int GetOrCreateCustomerId(AppDbContext db, string customerName, string contactNumber)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);
            contactNumber = ValidationHelper.CollapseSpaces(contactNumber);

            var existing = db.Customers
                .Where(c => !c.IsArchived && c.FullName == customerName)
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefault();

            if (existing != null)
            {
                if (string.Equals(existing.ContactNumber, AppConstants.NotProvided, StringComparison.OrdinalIgnoreCase))
                {
                    existing.ContactNumber = contactNumber;
                    db.SaveChanges();
                }

                return existing.CustomerId;
            }

            var newCustomer = new Customer
            {
                FullName = customerName,
                ContactNumber = contactNumber,
                LicenseNumber = AppConstants.NotProvided,
                Address = AppConstants.NotProvided
            };

            db.Customers.Add(newCustomer);
            db.SaveChanges();
            return newCustomer.CustomerId;
        }
    }
}
