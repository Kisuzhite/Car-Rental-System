using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class Rentals : Form
    {
        public Rentals()
        {
            InitializeComponent();
        }

        private void Rentals_Load(object? sender, EventArgs e)
        {
            PrepareForm();
            LoadLookups();
            LoadRentals();
        }

        private void PrepareForm()
        {
            returnDatePicker.Value = DateTime.Today.AddDays(1);
            CalculateTotal();
        }

        private void LoadLookups()
        {
            try
            {
                using var db = new AppDbContext();
                LoadCustomers(db);
                LoadAvailableCars(db);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadCustomers(AppDbContext db)
        {
            var customers = db.Customers
                .Where(c => !c.IsArchived)
                .OrderBy(c => c.FullName)
                .Select(c => new { c.CustomerId, c.FullName })
                .ToList();

            customerComboBox.DataSource = customers;
            customerComboBox.DisplayMember = "FullName";
            customerComboBox.ValueMember = "CustomerId";
        }

        private void LoadAvailableCars(AppDbContext db)
        {
            var cars = db.Cars
                .Where(c => c.Status == AppConstants.AvailableStatus)
                .OrderBy(c => c.Brand).ThenBy(c => c.Model)
                .Select(c => new
                {
                    c.CarId,
                    CarName = c.PlateNumber + " - " + c.Brand + " " + c.Model +
                        (c.CarType != null ? " (" + c.CarType.TypeName + ")" : ""),
                    DailyRate = c.CarType != null ? c.CarType.DailyRate : c.DailyRate
                })
                .ToList();

            carComboBox.DataSource = cars;
            carComboBox.DisplayMember = "CarName";
            carComboBox.ValueMember = "CarId";

            if (cars.Count == 0)
            {
                rateTextBox.Clear();
                totalTextBox.Text = RentalCalculator.FormatCurrency(0);
            }

            CarComboBox_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void LoadRentals(string search = "")
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
                        r.Car.Model.Contains(search) ||
                        (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(search)));
                }

                var rentals = query
                    .OrderByDescending(r => r.RentalId)
                    .Select(r => new
                    {
                        r.RentalId,
                        Customer = r.Customer.FullName,
                        Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                            (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                        RentalDate = r.RentalDate,
                        ReturnDate = r.ExpectedReturnDate,
                        TotalAmount = r.TotalAmount,
                        r.Status
                    })
                    .ToList();

                rentalsGrid.Columns.Clear();
                rentalsGrid.DataSource = rentals;

                DataGridViewHelper.ApplyFriendlyHeaders(rentalsGrid);
                DataGridViewHelper.HideColumn(rentalsGrid, "RentalId");
                DataGridViewHelper.ApplyStatusFormatting(rentalsGrid);
                DataGridViewHelper.ClearSelectionWhenReady(rentalsGrid);
            }
            catch
            {
                // The lookup loader already shows the LocalDB setup message.
            }
        }

        private void CreateButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out int customerId, out int carId, out decimal dailyRate, out decimal totalAmount))
                return;

            try
            {
                using var db = new AppDbContext();

                bool hasActiveRental = db.Rentals.Any(r =>
                    r.CustomerId == customerId &&
                    r.Status == AppConstants.ActiveRentalStatus);

                if (hasActiveRental)
                {
                    MessageBox.Show(
                        "This customer already has an active rental.\nProcess the return first before renting another car.",
                        "Customer Has Active Rental",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var car = db.Cars
                    .Include(c => c.CarType)
                    .FirstOrDefault(c =>
                    c.CarId == carId &&
                    c.Status == AppConstants.AvailableStatus);

                if (car == null)
                {
                    MessageBox.Show(
                        "The selected car is no longer available. Please choose another car.",
                        "Car Not Available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadLookups();
                    LoadRentals(searchTextBox.Text);
                    return;
                }

                car.Status = AppConstants.RentedStatus;
                dailyRate = car.CarType?.DailyRate ?? car.DailyRate;
                totalAmount = RentalCalculator.CalculateBaseAmount(
                    rentalDatePicker.Value,
                    returnDatePicker.Value,
                    dailyRate);

                db.Rentals.Add(new Rental
                {
                    CustomerId = customerId,
                    CarId = carId,
                    RentalDate = RentalCalculator.ToDateOnly(rentalDatePicker.Value),
                    ExpectedReturnDate = RentalCalculator.ToDateOnly(returnDatePicker.Value),
                    DailyRate = dailyRate,
                    TotalAmount = totalAmount,
                    Status = AppConstants.ActiveRentalStatus
                });

                db.SaveChanges();

                MessageBox.Show("Rental transaction created successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadLookups();
                LoadRentals(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to create rental.", ex);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadRentals(searchTextBox.Text);
        }

        private void CarComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (carComboBox.SelectedItem != null)
            {
                dynamic selected = carComboBox.SelectedItem;
                rateTextBox.Text = selected.DailyRate.ToString();
            }

            CalculateTotal();
        }

        private void DateOrRate_Changed(object? sender, EventArgs e)
        {
            CalculateTotal();
        }

        private bool ValidateInputs(out int customerId, out int carId, out decimal dailyRate, out decimal totalAmount)
        {
            customerId = 0;
            carId = 0;
            dailyRate = 0;
            totalAmount = 0;

            if (!TryGetSelectedValue(customerComboBox, out customerId) ||
                !TryGetSelectedValue(carComboBox, out carId))
            {
                MessageBox.Show("Please select a customer and an available car.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.TryValidateRentalDates(rentalDatePicker.Value, returnDatePicker.Value, out string dateError))
            {
                MessageBox.Show(dateError, "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(rateTextBox.Text, out dailyRate))
            {
                MessageBox.Show("Daily rate must be a valid amount.", "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidDailyRate(dailyRate))
            {
                MessageBox.Show(
                    $"Daily rate must be between {RentalCalculator.FormatCurrency(AppConstants.MinimumDailyRate)} and {RentalCalculator.FormatCurrency(AppConstants.MaximumDailyRate)}.",
                    "Invalid Rate",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            totalAmount = CalculateTotal();
            return true;
        }

        private static bool TryGetSelectedValue(ComboBox comboBox, out int value)
        {
            value = 0;
            return comboBox.SelectedValue != null &&
                int.TryParse(comboBox.SelectedValue.ToString(), out value);
        }

        private decimal CalculateTotal()
        {
            if (!decimal.TryParse(rateTextBox.Text, out decimal dailyRate))
            {
                totalTextBox.Text = RentalCalculator.FormatCurrency(0);
                return 0;
            }

            decimal total = RentalCalculator.CalculateBaseAmount(
                rentalDatePicker.Value,
                returnDatePicker.Value,
                dailyRate);

            totalTextBox.Text = RentalCalculator.FormatCurrency(total);
            return total;
        }

        private void ClearForm()
        {
            if (customerComboBox.Items.Count > 0) customerComboBox.SelectedIndex = 0;
            if (carComboBox.Items.Count > 0) carComboBox.SelectedIndex = 0;

            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);

            rentalsGrid.ClearSelection();
            CalculateTotal();
        }
    }
}
