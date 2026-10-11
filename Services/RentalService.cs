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
        public async Task<RentalOperationResult> ProcessReturnAsync(
            int rentalId,
            DateTime actualReturnDate,
            decimal additionalPayment = 0,
            PaymentMethod paymentMethod = PaymentMethod.Cash,
            string paymentReferenceNumber = "")
        {
            using var db = new AppDbContext();
            await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var rentalRepository = new RentalRepository(db);

            Rental? rental = await rentalRepository.GetActiveByIdWithCarAsync(rentalId);

            if (rental == null)
            {
                await transaction.RollbackAsync();
                return RentalOperationResult.Fail(
                    RentalOperationError.RentalAlreadyClosed,
                    "Rental Already Closed",
                    "This rental is no longer active.");
            }

            DateOnly actualReturn = RentalCalculator.ToDateOnly(actualReturnDate);

            if (actualReturn < rental.RentalDate)
            {
                await transaction.RollbackAsync();
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
            paymentReferenceNumber = string.IsNullOrWhiteSpace(paymentReferenceNumber)
                ? AppConstants.NotProvided
                : ValidationHelper.CollapseSpaces(paymentReferenceNumber);

            if (!PaymentHelper.IsValidPaymentAmount(newAmountPaid, finalAmount))
            {
                await transaction.RollbackAsync();
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
                rentalRepository.AddPayment(new RentalPayment
                {
                    RentalId = rental.RentalId,
                    PaymentDate = RentalCalculator.ToDateOnly(DateTime.Today),
                    Amount = additionalPayment,
                    PaymentMethod = paymentMethod,
                    PaymentReferenceNumber = paymentReferenceNumber,
                    PaymentType = RentalPaymentType.Return
                });
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

    }
}
