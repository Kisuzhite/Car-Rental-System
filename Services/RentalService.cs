using System.Data;
using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Services
{
    internal sealed class RentalService
    {
        public async Task<RentalOperationResult> CreateQuickRentalAsync(
            string customerName,
            int carId,
            DateTime rentalDate,
            DateTime expectedReturnDate,
            decimal amountPaid = 0,
            PaymentMethod paymentMethod = PaymentMethod.Cash)
        {
            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);

            var customerRepository = new CustomerRepository(db);
            int customerId = await customerRepository.GetOrCreateByNameAsync(customerName);
            RentalOperationResult result = await CreateRentalAsync(
                db,
                customerId,
                carId,
                rentalDate,
                expectedReturnDate,
                amountPaid,
                paymentMethod);

            if (result.Success)
            {
                await transaction.CommitAsync();
            }

            return result;
        }

        public async Task<RentalOperationResult> CreateRentalAsync(
            int customerId,
            int carId,
            DateTime rentalDate,
            DateTime expectedReturnDate,
            decimal amountPaid = 0,
            PaymentMethod paymentMethod = PaymentMethod.Cash)
        {
            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);

            RentalOperationResult result = await CreateRentalAsync(
                db,
                customerId,
                carId,
                rentalDate,
                expectedReturnDate,
                amountPaid,
                paymentMethod);

            if (result.Success)
            {
                await transaction.CommitAsync();
            }

            return result;
        }

        public async Task<RentalOperationResult> ProcessReturnAsync(
            int rentalId,
            DateTime actualReturnDate,
            decimal additionalPayment = 0,
            PaymentMethod paymentMethod = PaymentMethod.Cash)
        {
            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
            var rentalRepository = new RentalRepository(db);

            Rental? rental = await rentalRepository.GetActiveByIdWithCarAsync(rentalId);

            if (rental == null)
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.RentalAlreadyClosed,
                    "Rental Already Closed",
                    "This rental is no longer active.");
            }

            DateOnly actualReturn = RentalCalculator.ToDateOnly(actualReturnDate);

            if (actualReturn < rental.RentalDate)
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.InvalidReturnDate,
                    "Invalid Return Date",
                    "Actual return date cannot be earlier than the rental date.");
            }

            int lateDays = RentalCalculator.GetLateDays(rental.ExpectedReturnDate, actualReturnDate);
            decimal finalAmount = RentalCalculator.CalculateFinalAmount(
                rental.ExpectedReturnDate,
                actualReturnDate,
                rental.DailyRate,
                rental.TotalAmount);
            decimal newAmountPaid = rental.AmountPaid + additionalPayment;

            if (!PaymentHelper.IsValidPaymentAmount(newAmountPaid, finalAmount))
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.InvalidPaymentAmount,
                    "Invalid Payment",
                    "Payment cannot be negative or greater than the final amount.");
            }

            rental.ActualReturnDate = actualReturn;
            rental.TotalAmount = finalAmount;
            rental.AmountPaid = newAmountPaid;
            rental.PaymentStatus = PaymentHelper.GetPaymentStatus(finalAmount, newAmountPaid);
            if (additionalPayment > 0)
            {
                rental.PaymentMethod = paymentMethod;
            }
            rental.Status = RentalStatus.Returned;
            rental.Car.Status = CarStatus.Available;

            await rentalRepository.SaveChangesAsync();
            await transaction.CommitAsync();

            return RentalOperationResult.Ok(
                "Return Complete",
                "Car returned successfully.",
                rental.RentalId,
                rental.DailyRate,
                finalAmount,
                newAmountPaid,
                rental.Balance,
                lateDays);
        }

        private static async Task<RentalOperationResult> CreateRentalAsync(
            AppDbContext db,
            int customerId,
            int carId,
            DateTime rentalDate,
            DateTime expectedReturnDate,
            decimal amountPaid,
            PaymentMethod paymentMethod)
        {
            var rentalRepository = new RentalRepository(db);
            var carRepository = new CarRepository(db);
            var reservationRepository = new ReservationRepository(db);

            bool hasActiveRental = await rentalRepository.CustomerHasActiveRentalAsync(customerId);

            if (hasActiveRental)
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.CustomerHasActiveRental,
                    "Customer Has Active Rental",
                    "This customer already has an active rental.\nProcess the return first before renting another car.");
            }

            DateOnly rentalStart = RentalCalculator.ToDateOnly(rentalDate);
            DateOnly rentalEnd = RentalCalculator.ToDateOnly(expectedReturnDate);

            if (await reservationRepository.HasCarScheduleConflictAsync(carId, rentalStart, rentalEnd))
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.ScheduleConflict,
                    "Schedule Conflict",
                    "This car already has an active rental or reservation for those dates. Choose another car or adjust the dates.");
            }

            Car? car = await carRepository.GetAvailableByIdWithTypeAsync(carId);

            if (car == null)
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.CarNotAvailable,
                    "Car Not Available",
                    "The selected car is no longer available. Please choose another car.");
            }

            decimal dailyRate = car.CarType?.DailyRate ?? car.DailyRate;
            decimal totalAmount = RentalCalculator.CalculateBaseAmount(
                rentalDate,
                expectedReturnDate,
                dailyRate);

            if (!PaymentHelper.IsValidPaymentAmount(amountPaid, totalAmount))
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.InvalidPaymentAmount,
                    "Invalid Payment",
                    "Payment cannot be negative or greater than the total amount.");
            }

            car.Status = CarStatus.Rented;

            var rental = new Rental
            {
                CustomerId = customerId,
                CarId = carId,
                RentalDate = rentalStart,
                ExpectedReturnDate = rentalEnd,
                DailyRate = dailyRate,
                TotalAmount = totalAmount,
                AmountPaid = amountPaid,
                PaymentStatus = PaymentHelper.GetPaymentStatus(totalAmount, amountPaid),
                PaymentMethod = paymentMethod,
                Status = RentalStatus.Active
            };

            rentalRepository.Add(rental);
            await rentalRepository.SaveChangesAsync();

            return RentalOperationResult.Ok(
                "Saved",
                "Rental transaction created successfully.",
                rental.RentalId,
                dailyRate,
                totalAmount,
                amountPaid,
                rental.Balance);
        }

    }
}
