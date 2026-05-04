using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class ReturnCar : Form
    {
        private int? selectedRentalId;
        private int? selectedCarId;

        // Store selected row data for calculations after grid binding changes
        private DateOnly _selectedExpectedReturn;
        private decimal _selectedDailyRate;
        private decimal _selectedBaseAmount;
        private DateOnly _selectedRentalDate;

        public ReturnCar()
        {
            InitializeComponent();
        }

        private void ReturnCar_Load(object? sender, EventArgs e)
        {
            actualReturnDatePicker.Value = DateTime.Today;
            LoadActiveRentals();
        }

        private void LoadActiveRentals(string search = "")
        {
            try
            {
                using var db = new AppDbContext();

                var query = db.Rentals
                    .Where(r => r.Status == AppConstants.ActiveRentalStatus)
                    .Include(r => r.Customer)
                    .Include(r => r.Car)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(r =>
                        r.Customer.FullName.Contains(search) ||
                        r.Car.PlateNumber.Contains(search) ||
                        r.Car.Brand.Contains(search) ||
                        r.Car.Model.Contains(search));
                }

                var rentals = query
                    .OrderBy(r => r.ExpectedReturnDate)
                    .Select(r => new
                    {
                        r.RentalId,
                        r.CarId,
                        Customer = r.Customer.FullName,
                        Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model,
                        RentalDate = r.RentalDate,
                        ExpectedReturn = r.ExpectedReturnDate,
                        DailyRate = r.DailyRate,
                        BaseAmount = r.TotalAmount,
                        r.Status
                    })
                    .ToList();

                activeRentalsGrid.Columns.Clear();
                activeRentalsGrid.DataSource = rentals;

                DataGridViewHelper.HideColumn(activeRentalsGrid, "RentalId");
                DataGridViewHelper.HideColumn(activeRentalsGrid, "CarId");
                activeRentalsGrid.ClearSelection();
                ClearDetails();
                DataGridViewHelper.ClearSelectionWhenReady(activeRentalsGrid);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void ActiveRentalsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (activeRentalsGrid.CurrentRow?.DataBoundItem == null || !activeRentalsGrid.CurrentRow.Selected)
            {
                ClearDetails();
                return;
            }

            dynamic row = activeRentalsGrid.CurrentRow.DataBoundItem;

            selectedRentalId = (int)row.RentalId;
            selectedCarId = (int)row.CarId;

            _selectedRentalDate = (DateOnly)row.RentalDate;
            _selectedExpectedReturn = (DateOnly)row.ExpectedReturn;
            _selectedDailyRate = (decimal)row.DailyRate;
            _selectedBaseAmount = (decimal)row.BaseAmount;

            customerTextBox.Text = row.Customer;
            carTextBox.Text = row.Car;
            expectedReturnTextBox.Text = _selectedExpectedReturn.ToString("d", CultureInfo.CurrentCulture);
            dailyRateTextBox.Text = _selectedDailyRate.ToString("N2");
            baseAmountTextBox.Text = _selectedBaseAmount.ToString("N2");

            CalculateFinalAmount();
        }

        private void ActualReturnDatePicker_ValueChanged(object? sender, EventArgs e)
        {
            if (selectedRentalId != null)
            {
                CalculateFinalAmount();
            }
        }

        private void CalculateFinalAmount()
        {
            int lateDays = RentalCalculator.GetLateDays(_selectedExpectedReturn, actualReturnDatePicker.Value);
            decimal penalty = lateDays * _selectedDailyRate;
            decimal finalAmount = RentalCalculator.CalculateFinalAmount(
                _selectedExpectedReturn,
                actualReturnDatePicker.Value,
                _selectedDailyRate,
                _selectedBaseAmount);

            lateDaysTextBox.Text = lateDays.ToString();
            penaltyTextBox.Text = penalty.ToString("N2");
            finalAmountTextBox.Text = finalAmount.ToString("N2");
        }

        private void ProcessReturnButton_Click(object? sender, EventArgs e)
        {
            if (selectedRentalId == null || selectedCarId == null)
            {
                MessageBox.Show("Please select an active rental to return.", "No Rental Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateOnly actualReturnDate = RentalCalculator.ToDateOnly(actualReturnDatePicker.Value);

            if (actualReturnDate < _selectedRentalDate)
            {
                MessageBox.Show("Actual return date cannot be earlier than the rental date.", "Invalid Return Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(finalAmountTextBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal finalAmount))
            {
                MessageBox.Show("Final amount could not be calculated. Please select the rental again.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new AppDbContext();

                var rental = db.Rentals
                    .Include(r => r.Car)
                    .FirstOrDefault(r =>
                        r.RentalId == selectedRentalId.Value &&
                        r.Status == AppConstants.ActiveRentalStatus);

                if (rental == null)
                {
                    MessageBox.Show(
                        "This rental is no longer active.",
                        "Rental Already Closed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadActiveRentals(searchTextBox.Text);
                    return;
                }

                rental.ActualReturnDate = actualReturnDate;
                rental.TotalAmount = finalAmount;
                rental.Status = AppConstants.ReturnedRentalStatus;
                rental.Car.Status = AppConstants.AvailableStatus;

                db.SaveChanges();

                MessageBox.Show("Car returned successfully.", "Return Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadActiveRentals(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to process return.", ex);
            }
        }

        private void RefreshButton_Click(object? sender, EventArgs e)
        {
            LoadActiveRentals(searchTextBox.Text);
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadActiveRentals(searchTextBox.Text);
        }

        private void ClearDetails()
        {
            selectedRentalId = null;
            selectedCarId = null;
            _selectedRentalDate = default;
            _selectedExpectedReturn = default;
            _selectedDailyRate = 0;
            _selectedBaseAmount = 0;
            customerTextBox.Clear();
            carTextBox.Clear();
            expectedReturnTextBox.Clear();
            dailyRateTextBox.Clear();
            baseAmountTextBox.Clear();
            lateDaysTextBox.Clear();
            penaltyTextBox.Clear();
            finalAmountTextBox.Clear();
        }
    }
}
