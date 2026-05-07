using System.Data;
using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Services
{
    internal sealed class RentalService
    {
        public RentalOperationResult CreateQuickRental(
            string customerName,
            int carId,
            DateTime rentalDate,
            DateTime expectedReturnDate)
        {
            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);

            int customerId = GetOrCreateCustomerId(db, customerName);
            RentalOperationResult result = CreateRental(db, customerId, carId, rentalDate, expectedReturnDate);

            if (result.Success)
            {
                transaction.Commit();
            }

            return result;
        }

        public RentalOperationResult CreateRental(
            int customerId,
            int carId,
            DateTime rentalDate,
            DateTime expectedReturnDate)
        {
            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);

            RentalOperationResult result = CreateRental(db, customerId, carId, rentalDate, expectedReturnDate);

            if (result.Success)
            {
                transaction.Commit();
            }

            return result;
        }

        public RentalOperationResult ProcessReturn(int rentalId, DateTime actualReturnDate)
        {
            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);

            var rental = db.Rentals
                .Include(r => r.Car)
                .FirstOrDefault(r =>
                    r.RentalId == rentalId &&
                    r.Status == RentalStatus.Active);

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

            rental.ActualReturnDate = actualReturn;
            rental.TotalAmount = finalAmount;
            rental.Status = RentalStatus.Returned;
            rental.Car.Status = CarStatus.Available;

            db.SaveChanges();
            transaction.Commit();

            return RentalOperationResult.Ok(
                "Return Complete",
                "Car returned successfully.",
                rental.RentalId,
                rental.DailyRate,
                finalAmount,
                lateDays);
        }

        private static RentalOperationResult CreateRental(
            AppDbContext db,
            int customerId,
            int carId,
            DateTime rentalDate,
            DateTime expectedReturnDate)
        {
            bool hasActiveRental = db.Rentals.Any(r =>
                r.CustomerId == customerId &&
                r.Status == RentalStatus.Active);

            if (hasActiveRental)
            {
                return RentalOperationResult.Fail(
                    RentalOperationError.CustomerHasActiveRental,
                    "Customer Has Active Rental",
                    "This customer already has an active rental.\nProcess the return first before renting another car.");
            }

            var car = db.Cars
                .Include(c => c.CarType)
                .FirstOrDefault(c =>
                    c.CarId == carId &&
                    c.Status == CarStatus.Available);

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

            car.Status = CarStatus.Rented;

            var rental = new Rental
            {
                CustomerId = customerId,
                CarId = carId,
                RentalDate = RentalCalculator.ToDateOnly(rentalDate),
                ExpectedReturnDate = RentalCalculator.ToDateOnly(expectedReturnDate),
                DailyRate = dailyRate,
                TotalAmount = totalAmount,
                Status = RentalStatus.Active
            };

            db.Rentals.Add(rental);
            db.SaveChanges();

            return RentalOperationResult.Ok(
                "Saved",
                "Rental transaction created successfully.",
                rental.RentalId,
                dailyRate,
                totalAmount);
        }

        private static int GetOrCreateCustomerId(AppDbContext db, string customerName)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);

            var existing = db.Customers
                .Where(c => !c.IsArchived && c.FullName == customerName)
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefault();

            if (existing != null)
                return existing.CustomerId;

            var newCustomer = new Customer
            {
                FullName = customerName,
                ContactNumber = AppConstants.NotProvided,
                LicenseNumber = AppConstants.NotProvided,
                Address = AppConstants.NotProvided
            };

            db.Customers.Add(newCustomer);
            db.SaveChanges();
            return newCustomer.CustomerId;
        }
    }
}
