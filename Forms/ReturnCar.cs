using System.Globalization;
using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Services;
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
                DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

                var query = db.Rentals
                    .Where(r => r.Status == RentalStatus.Active)
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
                        (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(search)));
                }

                var rentals = query
                    .OrderBy(r => r.ExpectedReturnDate)
                    .ToList()
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
                        LateDays = RentalStatusHelper.GetLateDays(r, today),
                        Penalty = RentalStatusHelper.GetLatePenalty(r, today),
                        Status = RentalStatusHelper.GetDisplayStatus(r, today)
                    })
                    .ToList();

                activeRentalsGrid.Columns.Clear();
                activeRentalsGrid.DataSource = rentals;

                DataGridViewHelper.ApplyFriendlyHeaders(activeRentalsGrid);
                DataGridViewHelper.HideColumn(activeRentalsGrid, "RentalId");
                DataGridViewHelper.HideColumn(activeRentalsGrid, "CarId");
                DataGridViewHelper.ApplyStatusFormatting(activeRentalsGrid);
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

            if (activeRentalsGrid.CurrentRow.DataBoundItem is not ActiveRentalGridRow row)
            {
                ClearDetails();
                return;
            }

            selectedRentalId = row.RentalId;
            selectedCarId = row.CarId;

            _selectedRentalDate = row.RentalDate;
            _selectedExpectedReturn = row.ExpectedReturn;
            _selectedDailyRate = row.DailyRate;
            _selectedBaseAmount = row.BaseAmount;

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

            try
            {
                var rentalService = new RentalService();
                RentalOperationResult result = rentalService.ProcessReturn(
                    selectedRentalId.Value,
                    actualReturnDatePicker.Value);

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        result.Title,
                        MessageBoxButtons.OK,
                        result.RentalWasAlreadyClosed ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    if (result.RentalWasAlreadyClosed)
                        LoadActiveRentals(searchTextBox.Text);
                    return;
                }

                MessageBox.Show(result.Message, result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private sealed class ActiveRentalGridRow
        {
            public int RentalId { get; set; }
            public int CarId { get; set; }
            public string Customer { get; set; } = "";
            public string Car { get; set; } = "";
            public DateOnly RentalDate { get; set; }
            public DateOnly ExpectedReturn { get; set; }
            public decimal DailyRate { get; set; }
            public decimal BaseAmount { get; set; }
            public int LateDays { get; set; }
            public decimal Penalty { get; set; }
            public string Status { get; set; } = "";
        }
    }
}
