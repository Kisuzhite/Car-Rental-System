using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System
{
    public partial class MainForm : Form, IMainFormView
    {
        private readonly Padding dashboardPadding;
        private readonly AppUser currentUser;
        private readonly MainFormPresenter presenter;
        private readonly List<CarLookupItem> availableCarLookupItems = [];
        private Form? activeModuleForm;
        private CarLookupItem? selectedQuickRentalCar;

        public MainForm() : this(new AppUser { UserName = "owner", Role = AppConstants.OwnerRole })
        {
        }

        internal MainForm(AppUser currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            dashboardPadding = contentPanel.Padding;
            presenter = new MainFormPresenter(this);
        }

        public string DashboardSearchText => searchTextBox.Text;

        void IMainFormView.BindDashboard(DashboardData dashboardData)
        {
            totalCarsValueLabel.Text = dashboardData.TotalCars.ToString();
            availableCarsValueLabel.Text = dashboardData.AvailableCars.ToString();
            rentedCarsValueLabel.Text = dashboardData.OverdueRentals.ToString();
            incomeValueLabel.Text = RentalCalculator.FormatCurrency(dashboardData.MonthlyCollected);

            carsGrid.Columns.Clear();
            carsGrid.DataSource = dashboardData.AvailableCarsGrid;
            DataGridViewHelper.ApplyFriendlyHeaders(carsGrid);
            DataGridViewHelper.HideColumn(carsGrid, "CarId");
            DataGridViewHelper.ApplyStatusFormatting(carsGrid);
            carsGrid.ClearSelection();

            recentRentalsGrid.Columns.Clear();
            recentRentalsGrid.DataSource = dashboardData.RecentRentals;
            DataGridViewHelper.ApplyFriendlyHeaders(recentRentalsGrid);
            DataGridViewHelper.ApplyStatusFormatting(recentRentalsGrid);
            recentRentalsGrid.ClearSelection();
        }

        void IMainFormView.RefreshAvailableCarSelection(List<CarLookupItem> availableCars)
        {
            availableCarLookupItems.Clear();
            availableCarLookupItems.AddRange(availableCars);

            if (selectedQuickRentalCar != null &&
                !availableCarLookupItems.Any(c => c.CarId == selectedQuickRentalCar.CarId))
            {
                ClearSelectedQuickRentalCar();
            }
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToString("MMMM d, yyyy");
            staffBadgeLabel.Text = $"{currentUser.Role} Account";
            SetActiveNavigationButton(dashboardButton);
            PrepareDashboardForm();

            if (await TryPrepareDatabaseAsync())
            {
                await LoadDashboardDataAsync();
            }
        }

        private static async Task<bool> TryPrepareDatabaseAsync()
        {
            try
            {
                bool databaseIsReady = await MainFormPresenter.DatabaseIsReadyAsync();
                if (!databaseIsReady)
                {
                    MessageBox.Show(
                        "The database needs to be updated before the app can continue.\n\nRun this command from the project folder:\n\ndotnet ef database update",
                        "Database Update Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }

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
            paymentMethodComboBox.Format += PaymentMethodComboBox_Format;
            paymentMethodComboBox.DataSource = PaymentHelper.GetSelectablePaymentMethods();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);
            CalculateQuickRentalTotal();
        }

        private async Task LoadDashboardDataAsync()
        {
            try
            {
                await presenter.LoadDashboardDataAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private async void NavigationButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button selectedButton) return;

            ResetNavigationButtons();
            SetActiveNavigationButton(selectedButton);
            pageTitleLabel.Text = selectedButton.Text;

            if (selectedButton == dashboardButton)
            {
                ShowDashboard();
                await LoadDashboardDataAsync();
                return;
            }

            Form? moduleForm = selectedButton == carsButton ? new Cars()
                : selectedButton == ratesButton ? new CarTypes()
                : selectedButton == customersButton ? new Customers()
                : selectedButton == rentalsButton ? new Rentals()
                : selectedButton == reservationsButton ? new Reservations()
                : selectedButton == returnsButton ? new ReturnCar()
                : selectedButton == reportsButton ? new Reports()
                : null;

            if (moduleForm == null) return;

            ShowModule(moduleForm);
        }

        private void ResetNavigationButtons()
        {
            Button[] buttons = { dashboardButton, carsButton, ratesButton, customersButton, rentalsButton, reservationsButton, returnsButton, reportsButton };

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

        private async void CreateRentalButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateQuickRental(out QuickRentalFormInput input))
            {
                return;
            }

            try
            {
                RentalOperationResult result = await presenter.CreateQuickRentalAsync(input);

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        result.Title,
                        MessageBoxButtons.OK,
                        GetRentalFailureIcon(result));

                    if (result.CarWasUnavailable || result.HasScheduleConflict)
                    {
                        await LoadDashboardDataAsync();
                    }
                    return;
                }

                MessageBox.Show(result.Message, result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearQuickRentalForm();
                await LoadDashboardDataAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to create rental.", ex);
            }
        }

        private bool ValidateQuickRental(out QuickRentalFormInput input)
        {
            input = new QuickRentalFormInput("", 0, rentalDatePicker.Value, returnDatePicker.Value, 0, PaymentMethod.Cash);

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

            if (!TryGetSelectedCarId(out int carId))
            {
                MessageBox.Show("Please click an available car from the grid first.", "No Car Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.TryValidateRentalDates(rentalDatePicker.Value, returnDatePicker.Value, out string dateError))
            {
                MessageBox.Show(dateError, "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(dailyRateTextBox.Text, out decimal dailyRate))
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

            decimal totalAmount = CalculateQuickRentalTotal();
            decimal amountPaid = ParseOptionalPaymentAmount(paymentAmountTextBox.Text);

            if (!PaymentHelper.IsValidPaymentAmount(amountPaid, totalAmount))
            {
                MessageBox.Show("Amount paid cannot be negative or greater than the total amount.", "Invalid Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            PaymentMethod paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
                ? selectedMethod
                : PaymentMethod.Cash;

            input = new QuickRentalFormInput(
                customerNameTextBox.Text,
                carId,
                rentalDatePicker.Value,
                returnDatePicker.Value,
                amountPaid,
                paymentMethod);
            return true;
        }

        private static MessageBoxIcon GetRentalFailureIcon(RentalOperationResult result)
        {
            return result.CarWasUnavailable || result.HasScheduleConflict
                ? MessageBoxIcon.Information
                : MessageBoxIcon.Warning;
        }

        private bool TryGetSelectedCarId(out int carId)
        {
            carId = 0;

            if (selectedQuickRentalCar?.CarId is int selectedCarId)
            {
                carId = selectedCarId;
                return true;
            }

            return false;
        }

        private async void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            await LoadDashboardDataAsync();
        }

        private void DateOrRate_Changed(object? sender, EventArgs e)
        {
            CalculateQuickRentalTotal();
        }

        private static void PaymentMethodComboBox_Format(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is PaymentMethod paymentMethod)
            {
                e.Value = PaymentHelper.FormatPaymentMethod(paymentMethod);
            }
        }

        private void PaymentAmountTextBox_TextChanged(object? sender, EventArgs e)
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

        private static decimal ParseOptionalPaymentAmount(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? 0
                : decimal.TryParse(value, out decimal amount) ? amount : -1;
        }

        private void CarsGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (!TryGetClickedCarId(e.RowIndex, out int carId))
                return;

            foreach (CarLookupItem item in availableCarLookupItems)
            {
                if (item.CarId == carId)
                {
                    SelectQuickRentalCar(item);
                    break;
                }
            }
        }

        private bool TryGetClickedCarId(int rowIndex, out int carId)
        {
            carId = 0;

            if (rowIndex < 0 ||
                carsGrid.Rows[rowIndex].DataBoundItem == null ||
                !carsGrid.Columns.Contains("CarId"))
            {
                return false;
            }

            object? value = carsGrid.Rows[rowIndex].Cells["CarId"].Value;
            return value != null && int.TryParse(value.ToString(), out carId);
        }

        private void ClearQuickRentalForm()
        {
            customerNameTextBox.Clear();
            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);

            ClearSelectedQuickRentalCar();
            paymentAmountTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            CalculateQuickRentalTotal();
        }

        private void SelectQuickRentalCar(CarLookupItem car)
        {
            selectedQuickRentalCar = car;
            selectedCarTextBox.Text = car.CarName;
            dailyRateTextBox.Text = car.DailyRate?.ToString("0.##") ?? "";
            CalculateQuickRentalTotal();
        }

        private void ClearSelectedQuickRentalCar()
        {
            selectedQuickRentalCar = null;
            selectedCarTextBox.Clear();
            dailyRateTextBox.Clear();
            totalAmountTextBox.Text = RentalCalculator.FormatCurrency(0);
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
}
