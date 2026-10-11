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
            var rentalRepository = new RentalRepository(db);
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);
            DateOnly startDate = GetStartDate();
            DateOnly endDate = GetEndDate();
            int dayCount = Math.Max(1, (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days + 1);
            DateOnly previousEndDate = startDate.AddDays(-1);
            DateOnly previousStartDate = previousEndDate.AddDays(-dayCount + 1);

            List<Rental> rentals = await rentalRepository.GetRentalHistoryInDateRangeAsync(previousStartDate, endDate);
            List<Rental> filteredRentals = ApplyReportFilters(rentals, today, startDate, endDate);
            List<Rental> previousRentals = ApplyReportFilters(rentals, today, previousStartDate, previousEndDate);
            List<ReportRentalHistoryRow> history = BuildRentalHistoryRows(filteredRentals, today);

            view.BindReports(new ReportsData(
                filteredRentals.Count,
                filteredRentals.Count(r => r.Status == RentalStatus.Returned),
                filteredRentals.Count(r => IsOverdue(r, today)),
                filteredRentals.Sum(r => r.AmountPaid),
                FormatCountTrend(filteredRentals.Count, previousRentals.Count),
                FormatCountTrend(
                    filteredRentals.Count(r => r.Status == RentalStatus.Returned),
                    previousRentals.Count(r => r.Status == RentalStatus.Returned)),
                FormatCountTrend(
                    filteredRentals.Count(r => IsOverdue(r, today)),
                    previousRentals.Count(r => IsOverdue(r, today))),
                FormatCurrencyTrend(filteredRentals.Sum(r => r.AmountPaid), previousRentals.Sum(r => r.AmountPaid)),
                BuildMonthlyRevenue(filteredRentals),
                history));
        }

        private List<Rental> ApplyReportFilters(
            List<Rental> rentals,
            DateOnly today,
            DateOnly startDate,
            DateOnly endDate)
        {
            IEnumerable<Rental> query = rentals
                .Where(r => r.RentalDate >= startDate && r.RentalDate <= endDate);

            return query
                .OrderByDescending(r => r.RentalId)
                .ToList();
        }

        private static List<ReportRentalHistoryRow> BuildRentalHistoryRows(
            List<Rental> rentals,
            DateOnly today)
        {
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
                    LateDays = IsReturnedWithActualReturn(r) ? RentalStatusHelper.GetLateDays(r, today) : 0,
                    Penalty = IsReturnedWithActualReturn(r) ? RentalStatusHelper.GetLatePenalty(r, today) : 0,
                    TotalAmount = r.TotalAmount,
                    AmountPaid = r.AmountPaid,
                    Balance = r.Balance,
                    PaymentStatus = r.PaymentStatus,
                    PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                    Status = RentalStatusHelper.GetDisplayStatus(r, today)
                })
                .ToList();
        }

        private static bool IsReturnedWithActualReturn(Rental rental)
        {
            return rental.Status == RentalStatus.Returned && rental.ActualReturnDate.HasValue;
        }

        private List<DashboardRevenuePoint> BuildMonthlyRevenue(List<Rental> rentals)
        {
            DateOnly startDate = GetStartDate();
            DateOnly endDate = GetEndDate();
            DateOnly firstMonth = new(startDate.Year, startDate.Month, 1);
            DateOnly lastMonth = new(endDate.Year, endDate.Month, 1);

            int monthCount = ((lastMonth.Year - firstMonth.Year) * 12) + lastMonth.Month - firstMonth.Month + 1;
            if (monthCount > 12)
            {
                firstMonth = lastMonth.AddMonths(-11);
            }

            var points = new List<DashboardRevenuePoint>();
            for (DateOnly month = firstMonth; month <= lastMonth; month = month.AddMonths(1))
            {
                DateOnly nextMonth = month.AddMonths(1);
                decimal amount = rentals
                    .Where(r => r.RentalDate >= month && r.RentalDate < nextMonth)
                    .Sum(r => r.AmountPaid);

                points.Add(new DashboardRevenuePoint(month.ToString("MMM yyyy"), amount));
            }

            return points;
        }

        private static bool IsOverdue(Rental rental, DateOnly today)
        {
            return rental.Status == RentalStatus.Active &&
                rental.ExpectedReturnDate < today;
        }

        private DateOnly GetStartDate()
        {
            return view.DateFrom <= view.DateTo ? view.DateFrom : view.DateTo;
        }

        private DateOnly GetEndDate()
        {
            return view.DateFrom <= view.DateTo ? view.DateTo : view.DateFrom;
        }

        private static string FormatCountTrend(int current, int previous)
        {
            int difference = current - previous;
            if (difference == 0)
            {
                return "No change vs previous period";
            }

            return $"{(difference > 0 ? "+" : "")}{difference} vs previous period";
        }

        private static string FormatCurrencyTrend(decimal current, decimal previous)
        {
            decimal difference = current - previous;
            if (difference == 0)
            {
                return "No change vs previous period";
            }

            return $"{(difference > 0 ? "+" : "-")}{RentalCalculator.FormatCurrency(Math.Abs(difference))} vs previous period";
        }
    }
}
