using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class ReportsPresenter
    {
        private readonly IReportsView view;

        public ReportsPresenter(IReportsView view)
        {
            this.view = view;
        }

        public async Task LoadReportsAsync()
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);
            var rentalRepository = new RentalRepository(db);
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            int totalCars = await carRepository.CountAsync();
            int availableCars = await carRepository.CountByStatusAsync(CarStatus.Available);
            int activeRentals = await rentalRepository.CountActiveAsync();
            int overdueRentals = await rentalRepository.CountOverdueAsync(today);
            decimal totalCollected = await rentalRepository.GetTotalCollectedAsync();
            List<ReportRentalHistoryRow> history = await LoadRentalHistoryAsync(rentalRepository, today);

            view.BindReports(new ReportsData(
                totalCars,
                availableCars,
                activeRentals,
                overdueRentals,
                totalCollected,
                history));
        }

        private async Task<List<ReportRentalHistoryRow>> LoadRentalHistoryAsync(
            RentalRepository rentalRepository,
            DateOnly today)
        {
            List<Rental> rentals = await rentalRepository.GetRentalHistoryAsync(view.SearchText, today);

            return rentals
                .Select(r => new ReportRentalHistoryRow
                {
                    RentalId = r.RentalId,
                    Customer = r.Customer.FullName,
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                        (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                    CarType = r.Car.CarType?.TypeName ?? AppConstants.NotProvided,
                    RentalDate = r.RentalDate,
                    ExpectedReturn = r.ExpectedReturnDate,
                    ActualReturn = r.ActualReturnDate,
                    DailyRate = r.DailyRate,
                    LateDays = RentalStatusHelper.GetLateDays(r, today),
                    Penalty = RentalStatusHelper.GetLatePenalty(r, today),
                    TotalAmount = r.TotalAmount,
                    AmountPaid = r.AmountPaid,
                    Balance = r.Balance,
                    PaymentStatus = r.PaymentStatus,
                    PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                    Status = RentalStatusHelper.GetDisplayStatus(r, today)
                })
                .ToList();
        }
    }
}
