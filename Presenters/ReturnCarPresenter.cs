using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Repositories;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class ReturnCarPresenter
    {
        private readonly IReturnCarView view;

        public ReturnCarPresenter(IReturnCarView view)
        {
            this.view = view;
        }

        public async Task LoadActiveRentalsAsync()
        {
            using var db = new AppDbContext();
            var rentalRepository = new RentalRepository(db);
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            List<ActiveRentalGridRow> rentals = (await rentalRepository.GetActiveRentalsForReturnAsync(view.SearchText))
                .Select(r => new ActiveRentalGridRow
                {
                    RentalId = r.RentalId,
                    CarId = r.CarId,
                    Customer = r.Customer.FullName,
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                        (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                    RentalDate = r.RentalDate,
                    ExpectedReturn = r.ExpectedReturnDate,
                    DailyRate = r.DailyRate,
                    BaseAmount = r.TotalAmount,
                    AmountPaid = r.AmountPaid,
                    Balance = r.Balance,
                    PaymentStatus = r.PaymentStatus,
                    PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                    LateDays = RentalStatusHelper.GetLateDays(r, today),
                    Penalty = RentalStatusHelper.GetLatePenalty(r, today),
                    Status = RentalStatusHelper.GetDisplayStatus(r, today)
                })
                .ToList();

            view.BindActiveRentals(rentals);
        }

        public Task<RentalOperationResult> ProcessReturnAsync(ReturnFormInput input)
        {
            var rentalService = new RentalService();
            return rentalService.ProcessReturnAsync(
                input.RentalId,
                input.ActualReturnDate,
                input.AdditionalPayment,
                input.PaymentMethod);
        }
    }
}
