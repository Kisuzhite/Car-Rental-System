using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Repositories;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class RentalsPresenter
    {
        private readonly IRentalsView view;

        public RentalsPresenter(IRentalsView view)
        {
            this.view = view;
        }

        public async Task LoadLookupsAsync()
        {
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);
            await LoadLookupsAsync(today, today.AddDays(1));
        }

        public async Task LoadLookupsAsync(DateOnly pickupDate, DateOnly expectedReturnDate)
        {
            using var db = new AppDbContext();
            var customerRepository = new CustomerRepository(db);
            var carRepository = new CarRepository(db);

            List<CustomerLookupItem> customers = (await customerRepository.GetActiveForLookupAsync())
                .Select(c => new CustomerLookupItem
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    ContactNumber = c.ContactNumber,
                    LicenseNumber = c.LicenseNumber,
                    Address = c.Address
                })
                .ToList();

            view.BindCustomers(customers);
            await LoadAvailableCarsAsync(pickupDate, expectedReturnDate);
        }

        public async Task LoadAvailableCarsAsync(DateOnly pickupDate, DateOnly expectedReturnDate)
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);

            List<RentalCarLookupItem> cars = (await carRepository.GetAvailableForScheduleAsync(
                    pickupDate,
                    expectedReturnDate))
                .Select(c => new RentalCarLookupItem
                {
                    CarId = c.CarId,
                    CarName = c.Brand + " " + c.Model +
                        (c.CarType != null ? " (" + c.CarType.TypeName + ")" : ""),
                    DailyRate = c.CarType != null ? c.CarType.DailyRate : c.DailyRate
                })
                .ToList();

            view.BindAvailableCars(cars);
        }

        public async Task LoadRentalsAsync()
        {
            using var db = new AppDbContext();
            var rentalRepository = new RentalRepository(db);
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            List<RentalGridRow> rentals = (await rentalRepository.GetActiveRentalsAsync(view.SearchText))
                .Select(r => new RentalGridRow
                {
                    RentalId = r.RentalId,
                    Customer = r.Customer.FullName,
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                        (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                    RentalDate = r.RentalDate,
                    ReturnDate = r.ExpectedReturnDate,
                    TotalAmount = r.TotalAmount,
                    AmountPaid = r.AmountPaid,
                    Balance = r.Balance,
                    PaymentStatus = r.PaymentStatus,
                    PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                    LateDays = RentalStatusHelper.GetLateDays(r, today),
                    Status = RentalStatusHelper.GetDisplayStatus(r, today)
                })
                .ToList();

            view.BindRentals(rentals);
        }

        public Task<BookingOperationResult> CreateBookingAsync(BookingFormInput input)
        {
            var bookingService = new BookingService();
            return bookingService.CreateBookingAsync(
                input.CustomerName,
                input.ContactNumber,
                input.LicenseNumber,
                input.Address,
                input.CarId,
                input.PickupDate,
                input.ExpectedReturnDate,
                input.AmountPaid,
                input.PaymentMethod,
                input.PaymentReferenceNumber);
        }
    }
}
