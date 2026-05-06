using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
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
                availableCarsValueLabel.Text = db.Cars.Count(c => c.Status == AppConstants.AvailableStatus).ToString();

                int activeRentals = db.Rentals.Count(r => r.Status == AppConstants.ActiveRentalStatus);
                int overdueRentals = db.Rentals.Count(r =>
                    r.Status == AppConstants.ActiveRentalStatus &&
                    r.ExpectedReturnDate < today);
                activeRentalsValueLabel.Text = $"{activeRentals} / {overdueRentals}";

                decimal income = db.Rentals
                    .Where(r => r.Status == AppConstants.ReturnedRentalStatus)
                    .Sum(r => (decimal?)r.TotalAmount) ?? 0;
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
                query = query.Where(r =>
                    r.Customer.FullName.Contains(search) ||
                    r.Car.PlateNumber.Contains(search) ||
                    r.Car.Brand.Contains(search) ||
                    r.Car.Model.Contains(search) ||
                    (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(search)) ||
                    r.Status.Contains(search) ||
                    (r.Status == AppConstants.ActiveRentalStatus &&
                        r.ExpectedReturnDate < today &&
                        AppConstants.OverdueRentalStatus.Contains(search)));
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
