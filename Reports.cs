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

                totalCarsValueLabel.Text = db.Cars.Count().ToString();
                availableCarsValueLabel.Text = db.Cars.Count(c => c.Status == AppConstants.AvailableStatus).ToString();
                activeRentalsValueLabel.Text = db.Rentals.Count(r => r.Status == AppConstants.ActiveRentalStatus).ToString();

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
            var query = db.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Car)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.Customer.FullName.Contains(search) ||
                    r.Car.PlateNumber.Contains(search) ||
                    r.Car.Brand.Contains(search) ||
                    r.Car.Model.Contains(search) ||
                    r.Status.Contains(search));
            }

            var history = query
                .OrderByDescending(r => r.RentalId)
                .Select(r => new
                {
                    RentalId = r.RentalId,
                    Customer = r.Customer.FullName,
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model,
                    RentalDate = r.RentalDate,
                    ExpectedReturn = r.ExpectedReturnDate,
                    ActualReturn = r.ActualReturnDate,
                    TotalAmount = r.TotalAmount,
                    r.Status
                })
                .ToList();

            reportGrid.Columns.Clear();
            reportGrid.DataSource = history;

            DataGridViewHelper.HideColumn(reportGrid, "RentalId");
            DataGridViewHelper.ClearSelectionWhenReady(reportGrid);
        }
    }
}