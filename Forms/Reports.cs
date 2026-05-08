using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class Reports : Form
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object? sender, EventArgs e)
        {
            LoadReports();
        }

        private void RefreshButton_Click(object? sender, EventArgs e)
        {
            LoadReports(searchTextBox.Text);
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadReports(searchTextBox.Text);
        }

        private void LoadReports(string search = "")
        {
            try
            {
                using var db = new AppDbContext();
                DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

                totalCarsValueLabel.Text = db.Cars.Count().ToString();
                availableCarsValueLabel.Text = db.Cars.Count(c => c.Status == CarStatus.Available).ToString();

                int activeRentals = db.Rentals.Count(r => r.Status == RentalStatus.Active);
                int overdueRentals = db.Rentals.Count(r =>
                    r.Status == RentalStatus.Active &&
                    r.ExpectedReturnDate < today);
                activeRentalsValueLabel.Text = $"{activeRentals} / {overdueRentals}";

                decimal income = db.Rentals
                    .Sum(r => (decimal?)r.AmountPaid) ?? 0;
                incomeValueLabel.Text = RentalCalculator.FormatCurrency(income);

                LoadRentalHistory(db, search);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadRentalHistory(AppDbContext db, string search)
        {
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            var query = db.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string trimmedSearch = search.Trim();
                List<RentalStatus> matchingStatuses = Enum.GetValues<RentalStatus>()
                    .Where(status => status.ToString().Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                List<PaymentStatus> matchingPaymentStatuses = Enum.GetValues<PaymentStatus>()
                    .Where(status => status.ToString().Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                List<PaymentMethod> matchingPaymentMethods = Enum.GetValues<PaymentMethod>()
                    .Where(method => PaymentHelper.FormatPaymentMethod(method).Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase) ||
                        method.ToString().Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                bool matchesOverdueStatus = AppConstants.OverdueRentalStatus
                    .Contains(trimmedSearch, StringComparison.OrdinalIgnoreCase);

                query = query.Where(r =>
                    r.Customer.FullName.Contains(search) ||
                    r.Car.PlateNumber.Contains(search) ||
                    r.Car.Brand.Contains(search) ||
                    r.Car.Model.Contains(search) ||
                    (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(search)) ||
                    matchingStatuses.Contains(r.Status) ||
                    matchingPaymentStatuses.Contains(r.PaymentStatus) ||
                    matchingPaymentMethods.Contains(r.PaymentMethod) ||
                    (matchesOverdueStatus &&
                        r.Status == RentalStatus.Active &&
                        r.ExpectedReturnDate < today));
            }

            var history = query
                .OrderByDescending(r => r.RentalId)
                .ToList()
                .Select(r => new
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
                    r.AmountPaid,
                    Balance = r.Balance,
                    r.PaymentStatus,
                    PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                    Status = RentalStatusHelper.GetDisplayStatus(r, today)
                })
                .ToList();

            reportGrid.Columns.Clear();
            reportGrid.DataSource = history;

            DataGridViewHelper.ApplyFriendlyHeaders(reportGrid);
            DataGridViewHelper.HideColumn(reportGrid, "RentalId");
            DataGridViewHelper.ApplyStatusFormatting(reportGrid);
            DataGridViewHelper.ClearSelectionWhenReady(reportGrid);
        }
    }
}
