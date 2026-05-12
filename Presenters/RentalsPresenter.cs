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
            using var db = new AppDbContext();
            var customerRepository = new CustomerRepository(db);
            var carRepository = new CarRepository(db);

            List<CustomerLookupItem> customers = (await customerRepository.GetActiveForLookupAsync())
                .Select(c => new CustomerLookupItem
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName
                })
                .ToList();

            List<RentalCarLookupItem> cars = (await carRepository.GetAvailableCarsAsync())
                .Select(c => new RentalCarLookupItem
                {
                    CarId = c.CarId,
                    CarName = c.PlateNumber + " - " + c.Brand + " " + c.Model +
                        (c.CarType != null ? " (" + c.CarType.TypeName + ")" : ""),
                    DailyRate = c.CarType != null ? c.CarType.DailyRate : c.DailyRate
                })
                .ToList();

            view.BindCustomers(customers);
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

        public Task<RentalOperationResult> CreateRentalAsync(RentalFormInput input)
        {
            var rentalService = new RentalService();
            return rentalService.CreateRentalAsync(
                input.CustomerId,
                input.CarId,
                input.RentalDate,
                input.ExpectedReturnDate,
                input.AmountPaid,
                input.PaymentMethod);
        }
    }
}
