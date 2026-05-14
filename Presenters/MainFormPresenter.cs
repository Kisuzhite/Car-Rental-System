using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Presenters
{
    internal sealed class MainFormPresenter
    {
        private readonly IMainFormView view;

        public MainFormPresenter(IMainFormView view)
        {
            this.view = view;
        }

        public static async Task<bool> DatabaseIsReadyAsync()
        {
            using var db = new AppDbContext();

            if (!await db.Database.CanConnectAsync())
            {
                throw new InvalidOperationException("The database connection could not be opened.");
            }

            List<string> pendingMigrations = (await db.Database.GetPendingMigrationsAsync()).ToList();
            return pendingMigrations.Count == 0;
        }

        public async Task LoadDashboardDataAsync()
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);
            var rentalRepository = new RentalRepository(db);
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            int totalCars = await carRepository.CountAsync();
            int availableCars = await carRepository.CountByStatusAsync(CarStatus.Available);
            int overdueRentals = await rentalRepository.CountOverdueAsync(today);

            DateOnly firstOfMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateOnly firstOfNextMonth = firstOfMonth.AddMonths(1);
            decimal income = await rentalRepository.GetMonthlyCollectedAsync(firstOfMonth, firstOfNextMonth);

            List<Car> availableCarsForDashboard = await carRepository.GetAvailableCarsAsync(view.DashboardSearchText);

            List<DashboardCarGridRow> carRows = availableCarsForDashboard
                .Select(c => new DashboardCarGridRow
                {
                    CarId = c.CarId,
                    PlateNo = c.PlateNumber,
                    Type = c.CarType != null ? c.CarType.TypeName : AppConstants.NotProvided,
                    Brand = c.Brand,
                    Model = c.Model,
                    RatePerDay = c.CarType != null ? c.CarType.DailyRate : c.DailyRate,
                    Status = c.Status
                })
                .ToList();

            List<RecentRentalGridRow> recentRentals = (await rentalRepository.GetRecentRentalsAsync(10))
                .Select(r => new RecentRentalGridRow
                {
                    RentalId = r.RentalId,
                    Customer = r.Customer.FullName,
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                        (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                    ReturnDate = r.ExpectedReturnDate,
                    Amount = r.TotalAmount,
                    AmountPaid = r.AmountPaid,
                    Balance = r.Balance,
                    PaymentStatus = r.PaymentStatus,
                    Status = RentalStatusHelper.GetDisplayStatus(r, today)
                })
                .ToList();

            List<CarLookupItem> lookup = availableCarsForDashboard
                .Select(c => new CarLookupItem
                {
                    CarId = c.CarId,
                    CarName = c.PlateNumber + " - " + c.Brand + " " + c.Model +
                        (c.CarType != null ? " (" + c.CarType.TypeName + ")" : ""),
                    DailyRate = c.CarType != null ? c.CarType.DailyRate : c.DailyRate
                })
                .ToList();

            var dashboardData = new DashboardData(
                totalCars,
                availableCars,
                overdueRentals,
                income,
                carRows,
                recentRentals,
                lookup);

            view.BindDashboard(dashboardData);
            view.RefreshAvailableCarSelection(lookup);
        }

        public Task<RentalOperationResult> CreateQuickRentalAsync(QuickRentalFormInput input)
        {
            var rentalService = new RentalService();
            return rentalService.CreateQuickRentalAsync(
                input.CustomerName,
                input.CarId,
                input.RentalDate,
                input.ExpectedReturnDate,
                input.AmountPaid,
                input.PaymentMethod);
        }
    }
}
