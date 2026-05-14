using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class ReservationsPresenter
    {
        private readonly IReservationsView view;

        public ReservationsPresenter(IReservationsView view)
        {
            this.view = view;
        }

        public async Task LoadLookupsAsync()
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);

            List<ReservationCarLookupItem> cars = (await carRepository.GetAvailableCarsAsync())
                .Select(c => new ReservationCarLookupItem
                {
                    CarId = c.CarId,
                    CarName = c.PlateNumber + " - " + c.Brand + " " + c.Model +
                        (c.CarType != null ? " (" + c.CarType.TypeName + ")" : ""),
                    DailyRate = c.CarType != null ? c.CarType.DailyRate : c.DailyRate
                })
                .ToList();

            view.BindAvailableCars(cars);
        }

        public async Task LoadReservationsAsync()
        {
            using var db = new AppDbContext();
            var reservationRepository = new ReservationRepository(db);

            List<ReservationGridRow> reservations = (await reservationRepository.SearchAsync(view.SearchText))
                .Select(r => new ReservationGridRow
                {
                    ReservationId = r.ReservationId,
                    CarId = r.CarId,
                    CustomerName = r.CustomerName,
                    ContactNumber = r.ContactNumber,
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                        (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                    ReservationDate = r.ReservationDate,
                    PickupDate = r.PickupDate,
                    ExpectedReturnDate = r.ExpectedReturnDate,
                    DepositAmount = r.DepositAmount,
                    PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                    PaymentReferenceNumber = r.PaymentReferenceNumber,
                    Status = r.Status
                })
                .ToList();

            view.BindReservations(reservations);
        }

        public async Task<PresenterResult> AddReservationAsync(ReservationFormInput input)
        {
            using var db = new AppDbContext();
            var reservationRepository = new ReservationRepository(db);
            var carRepository = new CarRepository(db);

            if (await reservationRepository.HasCarScheduleConflictAsync(input.CarId, input.PickupDate, input.ExpectedReturnDate))
            {
                return ScheduleConflictResult();
            }

            Car? car = await carRepository.GetAvailableByIdWithTypeAsync(input.CarId);
            if (car == null)
            {
                return PresenterResult.Info(
                    "Car Not Available",
                    "The selected car is no longer available. Please choose another car.");
            }

            PresenterResult? depositValidation = ValidateReservationDeposit(input, car);
            if (depositValidation != null)
            {
                return depositValidation;
            }

            reservationRepository.Add(new Reservation
            {
                CarId = input.CarId,
                CustomerName = input.CustomerName,
                ContactNumber = input.ContactNumber,
                ReservationDate = RentalCalculator.ToDateOnly(DateTime.Today),
                PickupDate = input.PickupDate,
                ExpectedReturnDate = input.ExpectedReturnDate,
                DepositAmount = input.DepositAmount,
                PaymentMethod = input.PaymentMethod,
                PaymentReferenceNumber = input.PaymentReferenceNumber,
                Status = ReservationStatus.Confirmed
            });

            await reservationRepository.SaveChangesAsync();
            return PresenterResult.Ok("Saved", "Reservation saved successfully.");
        }

        public async Task<PresenterResult> UpdateReservationAsync(int reservationId, ReservationFormInput input)
        {
            using var db = new AppDbContext();
            var reservationRepository = new ReservationRepository(db);
            var carRepository = new CarRepository(db);
            Reservation? reservation = await reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null)
            {
                return PresenterResult.Error("Error", "Reservation not found.");
            }

            if (reservation.Status == ReservationStatus.Converted)
            {
                return PresenterResult.Info("Already Converted", "This reservation has already been converted to a rental.");
            }

            if (reservation.Status == ReservationStatus.Cancelled)
            {
                return PresenterResult.Info("Already Cancelled", "Cancelled reservations cannot be edited. Create a new reservation instead.");
            }

            if (await reservationRepository.HasCarScheduleConflictAsync(input.CarId, input.PickupDate, input.ExpectedReturnDate, reservationId))
            {
                return ScheduleConflictResult();
            }

            Car? car = await carRepository.GetAvailableByIdWithTypeAsync(input.CarId);
            if (car == null)
            {
                return PresenterResult.Info(
                    "Car Not Available",
                    "The selected car is no longer available. Please choose another car.");
            }

            PresenterResult? depositValidation = ValidateReservationDeposit(input, car);
            if (depositValidation != null)
            {
                return depositValidation;
            }

            reservation.CarId = input.CarId;
            reservation.CustomerName = input.CustomerName;
            reservation.ContactNumber = input.ContactNumber;
            reservation.PickupDate = input.PickupDate;
            reservation.ExpectedReturnDate = input.ExpectedReturnDate;
            reservation.DepositAmount = input.DepositAmount;
            reservation.PaymentMethod = input.PaymentMethod;
            reservation.PaymentReferenceNumber = input.PaymentReferenceNumber;

            await reservationRepository.SaveChangesAsync();
            return PresenterResult.Ok("Updated", "Reservation updated successfully.");
        }

        public async Task<PresenterResult> CancelReservationAsync(int reservationId)
        {
            using var db = new AppDbContext();
            var reservationRepository = new ReservationRepository(db);
            Reservation? reservation = await reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null)
            {
                return PresenterResult.Error("Error", "Reservation not found.");
            }

            if (reservation.Status == ReservationStatus.Converted)
            {
                return PresenterResult.Info("Already Converted", "Converted reservations cannot be cancelled.");
            }

            reservation.Status = ReservationStatus.Cancelled;
            await reservationRepository.SaveChangesAsync();
            return PresenterResult.Ok("Cancelled", "Reservation cancelled.");
        }

        public Task<ReservationOperationResult> ConvertToRentalAsync(int reservationId)
        {
            var reservationService = new ReservationService();
            return reservationService.ConvertToRentalAsync(reservationId);
        }

        private static PresenterResult ScheduleConflictResult()
        {
            return PresenterResult.Info(
                "Schedule Conflict",
                "This car already has an active rental or reservation for those dates.");
        }

        private static PresenterResult? ValidateReservationDeposit(ReservationFormInput input, Car car)
        {
            decimal dailyRate = car.CarType?.DailyRate ?? car.DailyRate;
            decimal estimatedTotal = RentalCalculator.CalculateBaseAmount(
                input.PickupDate.ToDateTime(TimeOnly.MinValue),
                input.ExpectedReturnDate.ToDateTime(TimeOnly.MinValue),
                dailyRate);

            if (PaymentHelper.IsValidInitialPaymentAmount(input.DepositAmount, estimatedTotal))
            {
                return null;
            }

            decimal minimumDeposit = PaymentHelper.GetMinimumDepositAmount(estimatedTotal);
            return PresenterResult.Warning(
                "Invalid Deposit",
                $"Deposit must be at least the minimum deposit of {RentalCalculator.FormatCurrency(minimumDeposit)} and cannot be greater than the estimated total.");
        }
    }
}
