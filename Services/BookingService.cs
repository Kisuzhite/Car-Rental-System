using System.Data;
using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Services
{
    internal sealed class BookingService
    {
        public async Task<BookingOperationResult> CreateBookingAsync(
            string customerName,
            string contactNumber,
            string licenseNumber,
            string address,
            int carId,
            DateTime pickupDate,
            DateTime expectedReturnDate,
            decimal amountPaid,
            PaymentMethod paymentMethod,
            string paymentReferenceNumber = "")
        {
            using var db = new AppDbContext();
            await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            BookingOperationResult result = await CreateBookingAsync(
                db,
                customerName,
                contactNumber,
                licenseNumber,
                address,
                carId,
                pickupDate,
                expectedReturnDate,
                amountPaid,
                paymentMethod,
                paymentReferenceNumber);

            if (result.Success)
            {
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
            }

            return result;
        }

        private static async Task<BookingOperationResult> CreateBookingAsync(
            AppDbContext db,
            string customerName,
            string contactNumber,
            string licenseNumber,
            string address,
            int carId,
            DateTime pickupDate,
            DateTime expectedReturnDate,
            decimal amountPaid,
            PaymentMethod paymentMethod,
            string paymentReferenceNumber)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);
            contactNumber = string.IsNullOrWhiteSpace(contactNumber)
                ? AppConstants.NotProvided
                : ValidationHelper.CollapseSpaces(contactNumber);
            licenseNumber = string.IsNullOrWhiteSpace(licenseNumber)
                ? AppConstants.NotProvided
                : ValidationHelper.NormalizeLicenseNumber(licenseNumber);
            address = string.IsNullOrWhiteSpace(address)
                ? AppConstants.NotProvided
                : ValidationHelper.CollapseSpaces(address);
            paymentReferenceNumber = string.IsNullOrWhiteSpace(paymentReferenceNumber)
                ? AppConstants.NotProvided
                : ValidationHelper.CollapseSpaces(paymentReferenceNumber);

            if (!ValidationHelper.IsValidPersonName(customerName))
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidCustomer,
                    "Invalid Customer",
                    "Customer name must be 2-100 characters and contain only letters, spaces, apostrophes, periods, or hyphens.");
            }

            if (!ValidationHelper.IsValidContactNumber(contactNumber))
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidCustomer,
                    "Invalid Contact Number",
                    "Contact number must contain 7-15 digits and may only include spaces, +, -, parentheses, or periods.");
            }

            if (!ValidationHelper.IsValidLicenseNumber(licenseNumber))
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidCustomer,
                    "Invalid License Number",
                    "License number must be 3-50 letters/numbers and may only include spaces or hyphens.");
            }

            if (address.Length < 5 || address.Length > 200)
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidCustomer,
                    "Invalid Billing Address",
                    "Billing address must be between 5 and 200 characters.");
            }

            DateOnly pickup = RentalCalculator.ToDateOnly(pickupDate);
            DateOnly expectedReturn = RentalCalculator.ToDateOnly(expectedReturnDate);
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            if (pickup < today)
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidDates,
                    "Invalid Pickup Date",
                    "Pickup date cannot be in the past.");
            }

            if (!ValidationHelper.TryValidateRentalDates(pickupDate, expectedReturnDate, out string dateError))
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidDates,
                    "Invalid Dates",
                    dateError);
            }

            var carRepository = new CarRepository(db);
            var customerRepository = new CustomerRepository(db);
            var rentalRepository = new RentalRepository(db);
            var reservationRepository = new ReservationRepository(db);

            Car? car = await carRepository.GetBookableByIdWithTypeAsync(carId);
            if (car == null)
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.CarNotAvailable,
                    "Car Not Available",
                    "The selected car is no longer available. Please choose another car.");
            }

            if (await reservationRepository.HasCarScheduleConflictAsync(carId, pickup, expectedReturn))
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.ScheduleConflict,
                    "Schedule Conflict",
                    "This car already has an active rental or reservation for those dates.");
            }

            decimal dailyRate = car.CarType?.DailyRate ?? car.DailyRate;
            decimal totalAmount = RentalCalculator.CalculateBaseAmount(pickupDate, expectedReturnDate, dailyRate);

            if (!PaymentHelper.IsValidInitialPaymentAmount(amountPaid, totalAmount))
            {
                decimal minimumDeposit = PaymentHelper.GetMinimumDepositAmount(totalAmount);
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidPaymentAmount,
                    "Invalid Payment",
                    $"Amount paid must be at least the minimum deposit of {RentalCalculator.FormatCurrency(minimumDeposit)} and cannot be greater than the total amount.");
            }

            Customer? existingCustomer = await customerRepository.GetActiveByNameAsync(customerName);
            if (await customerRepository.LicenseNumberExistsAsync(licenseNumber, existingCustomer?.CustomerId))
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.InvalidCustomer,
                    "Duplicate License Number",
                    "Another customer already uses this license number.");
            }

            Customer customer = await customerRepository.GetOrCreateCustomerByBookingDetailsAsync(
                customerName,
                contactNumber,
                licenseNumber,
                address);
            int customerId = customer.CustomerId;
            customerName = customer.FullName;
            contactNumber = customer.ContactNumber;

            if (await reservationRepository.HasCustomerScheduleConflictAsync(
                    customerId,
                    customerName,
                    pickup,
                    expectedReturn))
            {
                return BookingOperationResult.Fail(
                    BookingOperationError.ScheduleConflict,
                    "Customer Schedule Conflict",
                    "This customer already has an active rental or confirmed reservation that overlaps those dates. Return, cancel, or adjust the existing booking first.");
            }

            if (pickup == today)
            {
                if (await rentalRepository.CustomerHasActiveRentalAsync(customerId))
                {
                    return BookingOperationResult.Fail(
                        BookingOperationError.CustomerHasActiveRental,
                        "Customer Has Active Rental",
                        "This customer already has an active rental. Process the return first before starting another rental.");
                }

                if (car.Status != CarStatus.Available)
                {
                    return BookingOperationResult.Fail(
                        BookingOperationError.CarNotAvailable,
                        "Car Not Available",
                        "This car is not available right now. Process any active return or choose another car.");
                }

                car.Status = CarStatus.Rented;
                var rental = new Rental
                {
                    CustomerId = customerId,
                    CarId = carId,
                    RentalDate = pickup,
                    ExpectedReturnDate = expectedReturn,
                    DailyRate = dailyRate,
                    TotalAmount = totalAmount,
                    AmountPaid = amountPaid,
                    PaymentStatus = PaymentHelper.GetPaymentStatus(totalAmount, amountPaid),
                    PaymentMethod = paymentMethod,
                    Status = RentalStatus.Active
                };

                if (amountPaid > 0)
                {
                    rental.Payments.Add(new RentalPayment
                    {
                        PaymentDate = today,
                        Amount = amountPaid,
                        PaymentMethod = paymentMethod,
                        PaymentReferenceNumber = paymentReferenceNumber,
                        PaymentType = RentalPaymentType.Initial
                    });
                }

                rentalRepository.Add(rental);
                await rentalRepository.SaveChangesAsync();

                return BookingOperationResult.Ok(
                    "Rental Started",
                    "Booking saved as an active rental.",
                    BookingOperationKind.Rental,
                    rentalId: rental.RentalId,
                    totalAmount: totalAmount,
                    amountPaid: amountPaid,
                    balance: rental.Balance);
            }

            var reservation = new Reservation
            {
                CustomerId = customerId,
                CarId = carId,
                CustomerName = customerName,
                ContactNumber = contactNumber,
                ReservationDate = today,
                PickupDate = pickup,
                ExpectedReturnDate = expectedReturn,
                DepositAmount = amountPaid,
                PaymentMethod = paymentMethod,
                PaymentReferenceNumber = paymentReferenceNumber,
                Status = ReservationStatus.Confirmed
            };

            reservationRepository.Add(reservation);
            await reservationRepository.SaveChangesAsync();

            return BookingOperationResult.Ok(
                "Reservation Saved",
                "Booking saved as a future reservation.",
                BookingOperationKind.Reservation,
                reservationId: reservation.ReservationId,
                totalAmount: totalAmount,
                amountPaid: amountPaid,
                balance: PaymentHelper.GetBalance(totalAmount, amountPaid));
        }
    }
}
