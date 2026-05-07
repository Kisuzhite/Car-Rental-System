using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Services;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class MainForm : Form
    {
        private readonly Padding dashboardPadding;
        private readonly AppUser currentUser;
        private Form? activeModuleForm;

        public MainForm() : this(new AppUser { UserName = "owner", Role = AppConstants.OwnerRole })
        {
        }

        internal MainForm(AppUser currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            dashboardPadding = contentPanel.Padding;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToString("MMMM d, yyyy");
            staffBadgeLabel.Text = $"{currentUser.Role} Account";
            SetActiveNavigationButton(dashboardButton);
            PrepareDashboardForm();

            if (TryPrepareDatabase())
            {
                LoadDashboardData();
            }
        }

        private static bool TryPrepareDatabase()
        {
            try
            {
                using var db = new AppDbContext();
                db.Database.Migrate();
                return true;
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
                return false;
            }
        }

        private void PrepareDashboardForm()
        {
            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);
            CalculateQuickRentalTotal();
        }

        private void LoadDashboardData(string search = "")
        {
            try
            {
                using var db = new AppDbContext();
                LoadDashboardStats(db);
                LoadAvailableCars(db, search);
                LoadRecentRentals(db);
                LoadAvailableCarLookup(db);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadDashboardStats(AppDbContext db)
        {
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            totalCarsValueLabel.Text = db.Cars.Count().ToString();
            availableCarsValueLabel.Text = db.Cars.Count(c => c.Status == CarStatus.Available).ToString();
            rentedCarsValueLabel.Text = db.Rentals.Count(r =>
                r.Status == RentalStatus.Active &&
                r.ExpectedReturnDate < today).ToString();

            DateOnly firstOfMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateOnly firstOfNextMonth = firstOfMonth.AddMonths(1);

            decimal income = db.Rentals
                .Where(r => r.Status == RentalStatus.Returned
                    && r.ActualReturnDate >= firstOfMonth
                    && r.ActualReturnDate < firstOfNextMonth)
                .Sum(r => (decimal?)r.TotalAmount) ?? 0;

            incomeValueLabel.Text = RentalCalculator.FormatCurrency(income);
        }

        private void LoadAvailableCars(AppDbContext db, string search)
        {
            var query = db.Cars
                .Where(c => c.Status == CarStatus.Available)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.PlateNumber.Contains(search) ||
                    c.Brand.Contains(search) ||
                    c.Model.Contains(search) ||
                    (c.CarType != null && c.CarType.TypeName.Contains(search)));
            }

            var cars = query
                .OrderBy(c => c.Brand).ThenBy(c => c.Model)
                .Select(c => new
                {
                    PlateNo = c.PlateNumber,
                    Type = c.CarType != null ? c.CarType.TypeName : AppConstants.NotProvided,
                    c.Brand,
                    c.Model,
                    RatePerDay = c.CarType != null ? c.CarType.DailyRate : c.DailyRate,
                    c.Status
                })
                .ToList();

            carsGrid.Columns.Clear();
            carsGrid.DataSource = cars;
            DataGridViewHelper.ApplyFriendlyHeaders(carsGrid);
            DataGridViewHelper.ApplyStatusFormatting(carsGrid);
            carsGrid.ClearSelection();
        }

        private void LoadRecentRentals(AppDbContext db)
        {
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);

            var rentals = db.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .OrderByDescending(r => r.RentalId)
                .Take(10)
                .ToList()
                .Select(r => new
                {
                    RentalID = r.RentalId,
                    Customer = r.Customer.FullName,
                    Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                        (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                    ReturnDate = r.ExpectedReturnDate,
                    Amount = r.TotalAmount,
                    Status = RentalStatusHelper.GetDisplayStatus(r, today)
                })
                .ToList();

            recentRentalsGrid.Columns.Clear();
            recentRentalsGrid.DataSource = rentals;
            DataGridViewHelper.ApplyFriendlyHeaders(recentRentalsGrid);
            DataGridViewHelper.ApplyStatusFormatting(recentRentalsGrid);
            recentRentalsGrid.ClearSelection();
        }

        private void LoadAvailableCarLookup(AppDbContext db)
        {
            var cars = db.Cars
                .Where(c => c.Status == CarStatus.Available)
                .OrderBy(c => c.Brand).ThenBy(c => c.Model)
                .Select(c => new CarLookupItem
                {
                    CarId = c.CarId,
                    CarName = c.PlateNumber + " - " + c.Brand + " " + c.Model +
                        (c.CarType != null ? " (" + c.CarType.TypeName + ")" : ""),
                    DailyRate = c.CarType != null ? (decimal?)c.CarType.DailyRate : c.DailyRate
                })
                .ToList();

            // Prepend blank placeholder
            cars.Insert(0, new CarLookupItem
            {
                CarId = null,
                CarName = "-- Select a car --",
                DailyRate = null
            });

            selectedCarComboBox.DisplayMember = "CarName";
            selectedCarComboBox.ValueMember = "CarId";
            selectedCarComboBox.DataSource = cars;

            if (cars.Count <= 1)
            {
                dailyRateTextBox.Clear();
                totalAmountTextBox.Text = RentalCalculator.FormatCurrency(0);
            }
        }

        // -------------------------------------------------------
        // NAVIGATION
        // -------------------------------------------------------

        private void NavigationButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button selectedButton) return;

            ResetNavigationButtons();
            SetActiveNavigationButton(selectedButton);
            pageTitleLabel.Text = selectedButton.Text;

            if (selectedButton == dashboardButton)
            {
                ShowDashboard();
                LoadDashboardData(searchTextBox.Text);
                return;
            }

            Form? moduleForm = selectedButton == carsButton ? new Cars()
                : selectedButton == ratesButton ? new CarTypes()
                : selectedButton == customersButton ? new Customers()
                : selectedButton == rentalsButton ? new Rentals()
                : selectedButton == returnsButton ? new ReturnCar()
                : selectedButton == reportsButton ? new Reports()
                : null;

            if (moduleForm == null) return;

            ShowModule(moduleForm);
        }

        private void ResetNavigationButtons()
        {
            Button[] buttons = { dashboardButton, carsButton, ratesButton, customersButton, rentalsButton, returnsButton, reportsButton };

            foreach (Button button in buttons)
            {
                button.BackColor = Color.FromArgb(31, 41, 55);
                button.ForeColor = Color.White;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            }
        }

        private static void SetActiveNavigationButton(Button button)
        {
            button.BackColor = Color.FromArgb(37, 99, 235);
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
        }

        private void ShowDashboard()
        {
            topPanel.Visible = true;
            contentPanel.Padding = dashboardPadding;

            if (activeModuleForm != null)
            {
                contentPanel.Controls.Remove(activeModuleForm);
                activeModuleForm.Dispose();
                activeModuleForm = null;
            }

            foreach (Control control in contentPanel.Controls)
                control.Visible = true;
        }

        private void ShowModule(Form moduleForm)
        {
            topPanel.Visible = false;
            contentPanel.Padding = Padding.Empty;

            if (activeModuleForm != null)
            {
                contentPanel.Controls.Remove(activeModuleForm);
                activeModuleForm.Dispose();
            }

            foreach (Control control in contentPanel.Controls)
                control.Visible = false;

            activeModuleForm = moduleForm;
            moduleForm.TopLevel = false;
            moduleForm.FormBorderStyle = FormBorderStyle.None;
            moduleForm.Dock = DockStyle.Fill;
            moduleForm.ShowInTaskbar = false;

            contentPanel.Controls.Add(moduleForm);
            moduleForm.Show();
        }

        private void CreateRentalButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateQuickRental(out int carId, out decimal dailyRate, out decimal totalAmount))
                return;

            try
            {
                var rentalService = new RentalService();
                RentalOperationResult result = rentalService.CreateQuickRental(
                    customerNameTextBox.Text,
                    carId,
                    rentalDatePicker.Value,
                    returnDatePicker.Value);

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        result.Title,
                        MessageBoxButtons.OK,
                        GetRentalFailureIcon(result));

                    if (result.CarWasUnavailable)
                        LoadDashboardData(searchTextBox.Text);
                    return;
                }

                MessageBox.Show(result.Message, result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearQuickRentalForm();
                LoadDashboardData(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to create rental.", ex);
            }
        }

        private bool ValidateQuickRental(out int carId, out decimal dailyRate, out decimal totalAmount)
        {
            carId = 0;
            dailyRate = 0;
            totalAmount = 0;

            if (string.IsNullOrWhiteSpace(customerNameTextBox.Text))
            {
                MessageBox.Show("Please enter the customer name.", "Missing Customer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidPersonName(customerNameTextBox.Text))
            {
                MessageBox.Show("Customer name must be 2-100 characters and contain only letters, spaces, apostrophes, periods, or hyphens.", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!TryGetSelectedCarId(out carId))
            {
                MessageBox.Show("Please add or select an available car first.", "No Available Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.TryValidateRentalDates(rentalDatePicker.Value, returnDatePicker.Value, out string dateError))
            {
                MessageBox.Show(dateError, "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(dailyRateTextBox.Text, out dailyRate))
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

            totalAmount = CalculateQuickRentalTotal();
            return true;
        }

        private static MessageBoxIcon GetRentalFailureIcon(RentalOperationResult result)
        {
            return result.CarWasUnavailable
                ? MessageBoxIcon.Information
                : MessageBoxIcon.Warning;
        }

        private bool TryGetSelectedCarId(out int carId)
        {
            carId = 0;

            if (selectedCarComboBox.SelectedItem is CarLookupItem item && item.CarId.HasValue)
            {
                carId = item.CarId.Value;
                return true;
            }

            return false;
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearQuickRentalForm();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadDashboardData(searchTextBox.Text);
        }

        private void SelectedCarComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (selectedCarComboBox.SelectedItem is CarLookupItem item && item.DailyRate.HasValue)
            {
                dailyRateTextBox.Text = item.DailyRate.Value.ToString("0.##");
            }
            else
            {
                dailyRateTextBox.Clear();
                totalAmountTextBox.Text = RentalCalculator.FormatCurrency(0);
            }

            CalculateQuickRentalTotal();
        }

        private void DateOrRate_Changed(object? sender, EventArgs e)
        {
            CalculateQuickRentalTotal();
        }

        private decimal CalculateQuickRentalTotal()
        {
            if (!decimal.TryParse(dailyRateTextBox.Text, out decimal dailyRate))
            {
                totalAmountTextBox.Text = RentalCalculator.FormatCurrency(0);
                return 0;
            }

            decimal total = RentalCalculator.CalculateBaseAmount(
                rentalDatePicker.Value,
                returnDatePicker.Value,
                dailyRate);

            totalAmountTextBox.Text = RentalCalculator.FormatCurrency(total);
            return total;
        }

        private void CarsGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || carsGrid.Rows[e.RowIndex].DataBoundItem == null)
                return;

            string plateNumber = carsGrid.Rows[e.RowIndex].Cells["PlateNo"].Value?.ToString() ?? "";

            foreach (var item in selectedCarComboBox.Items.OfType<CarLookupItem>())
            {
                if (item.CarName.StartsWith(plateNumber))
                {
                    selectedCarComboBox.SelectedItem = item;
                    break;
                }
            }

            carsGrid.ClearSelection();
        }

        private void ClearQuickRentalForm()
        {
            customerNameTextBox.Clear();
            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);

            if (selectedCarComboBox.Items.Count > 0)
                selectedCarComboBox.SelectedIndex = 0;

            CalculateQuickRentalTotal();
        }

        private void LogoutButton_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            Close();
        }
    }

    // Helper class to replace DataTable rows in the car combo box
    public class CarLookupItem
    {
        public int? CarId { get; set; }
        public string CarName { get; set; } = "";
        public decimal? DailyRate { get; set; }
    }
}
