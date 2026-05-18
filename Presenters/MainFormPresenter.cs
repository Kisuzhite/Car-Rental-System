using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            var reservationRepository = new ReservationRepository(db);
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            int rentedCars = await rentalRepository.CountActiveAsync();
            int activeBookings = rentedCars + await reservationRepository.CountOpenAsync();
            int overdueRentals = await rentalRepository.CountOverdueAsync(today);

            DateOnly firstOfMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);
            decimal totalRevenue = await rentalRepository.GetTotalCollectedAsync() +
                await reservationRepository.GetOpenDepositCollectedAsync();

            List<Car> availableCarsForDashboard = await carRepository.GetAvailableForScheduleAsync(
                today,
                today.AddDays(1));
            int availableCars = availableCarsForDashboard.Count;

            List<DashboardScheduleDay> weeklySchedule = new();
            for (int i = 0; i < 7; i++)
            {
                DateOnly scheduleDate = today.AddDays(i);
                weeklySchedule.Add(new DashboardScheduleDay(
                    scheduleDate,
                    await reservationRepository.CountOpenPickupsForDateAsync(scheduleDate),
                    await rentalRepository.CountReturnsDueAsync(scheduleDate)));
            }

            List<DashboardRevenuePoint> monthlyRevenue = new();
            DateOnly firstChartMonth = firstOfMonth.AddMonths(-5);
            for (int i = 0; i < 6; i++)
            {
                DateOnly monthStart = firstChartMonth.AddMonths(i);
                DateOnly monthEnd = monthStart.AddMonths(1);
                decimal amount = await rentalRepository.GetCollectedForStartsInRangeAsync(monthStart, monthEnd) +
                    await reservationRepository.GetOpenDepositCollectedInRangeAsync(monthStart, monthEnd);
                string monthLabel = new DateTime(monthStart.Year, monthStart.Month, 1)
                    .ToString("MMM", CultureInfo.CurrentCulture);

                monthlyRevenue.Add(new DashboardRevenuePoint(monthLabel, amount));
            }

            List<(decimal TotalAmount, decimal AmountPaid)> paymentAmounts = await rentalRepository.GetActivePaymentAmountsAsync();
            paymentAmounts.AddRange(await reservationRepository.GetOpenPaymentAmountsAsync());
            List<DashboardPaymentStatusPoint> paymentStatus = BuildPaymentStatusPoints(paymentAmounts);

            var dashboardData = new DashboardData(
                totalRevenue,
                activeBookings,
                rentedCars,
                availableCars,
                overdueRentals,
                weeklySchedule,
                monthlyRevenue,
                paymentStatus);

            view.BindDashboard(dashboardData);
        }

        private static List<DashboardPaymentStatusPoint> BuildPaymentStatusPoints(
            List<(decimal TotalAmount, decimal AmountPaid)> paymentAmounts)
        {
            int fullyPaid = 0;
            int balanceRemaining = 0;

            foreach ((decimal totalAmount, decimal amountPaid) in paymentAmounts)
            {
                if (amountPaid >= totalAmount)
                {
                    fullyPaid++;
                }
                else
                {
                    balanceRemaining++;
                }
            }

            return
            [
                new DashboardPaymentStatusPoint("Fully Paid", fullyPaid),
                new DashboardPaymentStatusPoint("Balance Remaining", balanceRemaining)
            ];
        }
    }
}
