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
            DateOnly pickupDate = RentalCalculator.ToDateOnly(DateTime.Today.AddDays(1));
            await LoadLookupsAsync(pickupDate, pickupDate.AddDays(1));
        }

        public async Task LoadLookupsAsync(
            DateOnly pickupDate,
            DateOnly expectedReturnDate,
            int? excludedReservationId = null)
        {
            await LoadAvailableCarsAsync(pickupDate, expectedReturnDate, excludedReservationId);
        }

        public async Task LoadAvailableCarsAsync(
            DateOnly pickupDate,
            DateOnly expectedReturnDate,
            int? excludedReservationId = null)
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);

            List<ReservationCarLookupItem> cars = (await carRepository.GetAvailableForScheduleAsync(
                    pickupDate,
                    expectedReturnDate,
                    excludedReservationId: excludedReservationId))
                .Select(c => new ReservationCarLookupItem
                {
                    CarId = c.CarId,
                    CarName = c.Brand + " " + c.Model +
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
                    CustomerName = r.Customer?.FullName ?? r.CustomerName,
                    ContactNumber = GetReservationContactNumber(r),
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                        (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                    DailyRate = r.Car.CarType != null ? r.Car.CarType.DailyRate : r.Car.DailyRate,
                    ReservationDate = r.ReservationDate,
                    PickupDate = r.PickupDate,
                    ExpectedReturnDate = r.ExpectedReturnDate,
                    DepositAmount = r.DepositAmount,
                    PaymentMethodValue = r.PaymentMethod,
                    PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                    PaymentReferenceNumber = r.PaymentReferenceNumber,
                    Status = r.Status
                })
                .ToList();

            view.BindReservations(reservations);
        }

        public async Task<PresenterResult> UpdateReservationAsync(int reservationId, ReservationFormInput input)
        {
            using var db = new AppDbContext();
            var reservationRepository = new ReservationRepository(db);
            var carRepository = new CarRepository(db);
            var customerRepository = new CustomerRepository(db);
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

            Customer? reservationCustomer = reservation.CustomerId.HasValue
                ? await customerRepository.GetByIdAsync(reservation.CustomerId.Value)
                : null;
            string lockedCustomerName = reservationCustomer is { IsArchived: false }
                ? reservationCustomer.FullName
                : ValidationHelper.CollapseSpaces(reservation.CustomerName);

            if (string.IsNullOrWhiteSpace(lockedCustomerName))
            {
                lockedCustomerName = input.CustomerName;
            }

            Customer? matchingCustomer = reservationCustomer is { IsArchived: false }
                ? reservationCustomer
                : await customerRepository.GetActiveByNameAsync(lockedCustomerName);
            int? customerIdForConflictCheck = matchingCustomer?.CustomerId ?? reservation.CustomerId;

            if (await reservationRepository.HasCustomerScheduleConflictAsync(
                    customerIdForConflictCheck,
                    lockedCustomerName,
                    input.PickupDate,
                    input.ExpectedReturnDate,
                    reservationId))
            {
                return CustomerScheduleConflictResult();
            }

            Car? car = await carRepository.GetBookableByIdWithTypeAsync(input.CarId);
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

            Customer customer = await customerRepository.GetOrCreateCustomerByNameAndContactAsync(
                lockedCustomerName,
                input.ContactNumber);

            reservation.CustomerId = customer.CustomerId;
            reservation.CarId = input.CarId;
            reservation.CustomerName = customer.FullName;
            reservation.ContactNumber = customer.ContactNumber;
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

        private static PresenterResult CustomerScheduleConflictResult()
        {
            return PresenterResult.Info(
                "Customer Schedule Conflict",
                "This customer already has an active rental or confirmed reservation that overlaps those dates. Return, cancel, or adjust the existing booking first.");
        }

        private static string GetReservationContactNumber(Reservation reservation)
        {
            string? customerContact = reservation.Customer?.ContactNumber;
            if (!string.IsNullOrWhiteSpace(customerContact) &&
                !string.Equals(customerContact, AppConstants.NotProvided, StringComparison.OrdinalIgnoreCase))
            {
                return customerContact;
            }

            return reservation.ContactNumber;
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
