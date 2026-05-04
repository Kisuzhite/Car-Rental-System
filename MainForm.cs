using Microsoft.Data.SqlClient;
using System.Data;

namespace Car_Rental_System
{
    public partial class MainForm : Form
    {
        private readonly Padding dashboardPadding;
        private Form? activeModuleForm;

        public MainForm()
        {
            InitializeComponent();
            dashboardPadding = contentPanel.Padding;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToString("MMMM d, yyyy");
            SetActiveNavigationButton(dashboardButton);
            PrepareDashboardForm();
            LoadDashboardData();
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
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                LoadDashboardStats(connection);
                LoadAvailableCars(connection, search);
                LoadRecentRentals(connection);
                LoadAvailableCarLookup(connection);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadDashboardStats(SqlConnection connection)
        {
            totalCarsValueLabel.Text = DatabaseHelper.ExecuteScalar(connection, "SELECT COUNT(*) FROM Cars;").ToString();
            availableCarsValueLabel.Text = DatabaseHelper.ExecuteScalar(
                connection,
                $"SELECT COUNT(*) FROM Cars WHERE Status = '{AppConstants.AvailableStatus}';").ToString();
            rentedCarsValueLabel.Text = DatabaseHelper.ExecuteScalar(
                connection,
                $"SELECT COUNT(*) FROM Cars WHERE Status = '{AppConstants.RentedStatus}';").ToString();

            string monthlyIncomeSql = $@"
SELECT ISNULL(SUM(TotalAmount), 0)
FROM Rentals
WHERE Status = '{AppConstants.ReturnedRentalStatus}'
    AND ActualReturnDate >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
    AND ActualReturnDate < DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1));";

            object income = DatabaseHelper.ExecuteScalar(connection, monthlyIncomeSql);
            incomeValueLabel.Text = RentalCalculator.FormatCurrency(Convert.ToDecimal(income));
        }

        private void LoadAvailableCars(SqlConnection connection, string search)
        {
            string query = @"
SELECT
    PlateNumber AS [Plate No.],
    Brand,
    Model,
    DailyRate AS [Rate/Day],
    Status
FROM Cars
WHERE
    Status = @AvailableStatus
    AND (
        @Search = ''
        OR PlateNumber LIKE @SearchText
        OR Brand LIKE @SearchText
        OR Model LIKE @SearchText
    )
ORDER BY Brand, Model;";

            using SqlCommand command = new(query, connection);
            command.AddString("@AvailableStatus", AppConstants.AvailableStatus, 20);
            command.AddSearchParameters(search);

            DataTable table = new();
            using SqlDataAdapter adapter = new(command);
            adapter.Fill(table);

            carsGrid.Columns.Clear();
            carsGrid.DataSource = table;
            carsGrid.ClearSelection();
        }

        private void LoadRecentRentals(SqlConnection connection)
        {
            string query = @"
SELECT TOP 10
    r.RentalId AS [Rental ID],
    c.FullName AS Customer,
    ca.PlateNumber + ' - ' + ca.Brand + ' ' + ca.Model AS Car,
    r.ExpectedReturnDate AS [Return Date],
    r.TotalAmount AS Amount,
    r.Status
FROM Rentals r
INNER JOIN Customers c ON c.CustomerId = r.CustomerId
INNER JOIN Cars ca ON ca.CarId = r.CarId
ORDER BY r.RentalId DESC;";

            using SqlCommand command = new(query, connection);
            DataTable table = new();
            using SqlDataAdapter adapter = new(command);
            adapter.Fill(table);

            recentRentalsGrid.Columns.Clear();
            recentRentalsGrid.DataSource = table;
            recentRentalsGrid.ClearSelection();
        }

        private void LoadAvailableCarLookup(SqlConnection connection)
        {
            string query = @"
SELECT
    CarId,
    PlateNumber + ' - ' + Brand + ' ' + Model AS CarName,
    DailyRate
FROM Cars
WHERE Status = @AvailableStatus
ORDER BY Brand, Model;";

            using SqlCommand command = new(query, connection);
            command.AddString("@AvailableStatus", AppConstants.AvailableStatus, 20);
            DataTable table = new();
            using SqlDataAdapter adapter = new(command);
            adapter.Fill(table);

            DataRow blankRow = table.NewRow();
            blankRow["CarId"] = DBNull.Value;
            blankRow["CarName"] = "-- Select a car --";
            blankRow["DailyRate"] = DBNull.Value;
            table.Rows.InsertAt(blankRow, 0);

            selectedCarComboBox.DisplayMember = "CarName";
            selectedCarComboBox.ValueMember = "CarId";
            selectedCarComboBox.DataSource = table;

            if (table.Rows.Count <= 1)
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
            if (sender is not Button selectedButton)
            {
                return;
            }

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
                : selectedButton == customersButton ? new Customers()
                : selectedButton == rentalsButton ? new Rentals()
                : selectedButton == returnsButton ? new ReturnCar()
                : selectedButton == reportsButton ? new Reports()
                : null;

            if (moduleForm == null)
            {
                return;
            }

            ShowModule(moduleForm);
        }

        private void ResetNavigationButtons()
        {
            Button[] buttons = { dashboardButton, carsButton, customersButton, rentalsButton, returnsButton, reportsButton };

            foreach (Button button in buttons)
            {
                button.BackColor = Color.FromArgb(52, 58, 64);
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
            {
                control.Visible = true;
            }
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
            {
                control.Visible = false;
            }

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
            {
                return;
            }

            try
            {
                using SqlConnection connection = DatabaseHelper.OpenConnection();
                using SqlTransaction transaction = connection.BeginTransaction();

                int customerId = GetOrCreateCustomer(connection, transaction, customerNameTextBox.Text.Trim());
                if (CustomerHasActiveRental(connection, transaction, customerId))
                {
                    transaction.Rollback();
                    MessageBox.Show(
                        "This customer already has an active rental.\nProcess the return first before renting another car.",
                        "Customer Has Active Rental",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using SqlCommand carCommand = new(
                    "UPDATE Cars SET Status = @RentedStatus WHERE CarId = @CarId AND Status = @AvailableStatus;",
                    connection,
                    transaction);
                carCommand.AddString("@RentedStatus", AppConstants.RentedStatus, 20);
                carCommand.AddInt("@CarId", carId);
                carCommand.AddString("@AvailableStatus", AppConstants.AvailableStatus, 20);

                if (carCommand.ExecuteNonQuery() == 0)
                {
                    transaction.Rollback();
                    MessageBox.Show(
                        "The selected car is no longer available. Please choose another car.",
                        "Car Not Available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadDashboardData(searchTextBox.Text);
                    return;
                }

                string insertRentalSql = @"
INSERT INTO Rentals (CustomerId, CarId, RentalDate, ExpectedReturnDate, DailyRate, TotalAmount, Status)
VALUES (@CustomerId, @CarId, @RentalDate, @ExpectedReturnDate, @DailyRate, @TotalAmount, @ActiveStatus);";

                using SqlCommand rentalCommand = new(insertRentalSql, connection, transaction);
                rentalCommand.AddInt("@CustomerId", customerId);
                rentalCommand.AddInt("@CarId", carId);
                rentalCommand.AddDate("@RentalDate", rentalDatePicker.Value);
                rentalCommand.AddDate("@ExpectedReturnDate", returnDatePicker.Value);
                rentalCommand.AddMoney("@DailyRate", dailyRate);
                rentalCommand.AddMoney("@TotalAmount", totalAmount);
                rentalCommand.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);
                rentalCommand.ExecuteNonQuery();

                transaction.Commit();

                MessageBox.Show("Rental transaction created successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearQuickRentalForm();
                LoadDashboardData(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to create rental.", ex);
            }
        }

        private int GetOrCreateCustomer(SqlConnection connection, SqlTransaction transaction, string customerName)
        {
            using SqlCommand findCommand = new(
                "SELECT TOP 1 CustomerId FROM Customers WHERE FullName = @FullName ORDER BY CustomerId DESC;",
                connection,
                transaction);
            findCommand.AddString("@FullName", customerName, 100);

            object? existingCustomerId = findCommand.ExecuteScalar();
            if (existingCustomerId != null)
            {
                return Convert.ToInt32(existingCustomerId);
            }

            string insertCustomerSql = @"
INSERT INTO Customers (FullName, ContactNumber, LicenseNumber, Address)
OUTPUT INSERTED.CustomerId
VALUES (@FullName, @ContactNumber, @LicenseNumber, @Address);";

            using SqlCommand insertCommand = new(insertCustomerSql, connection, transaction);
            insertCommand.AddString("@FullName", customerName, 100);
            insertCommand.AddString("@ContactNumber", AppConstants.NotProvided, 30);
            insertCommand.AddString("@LicenseNumber", AppConstants.NotProvided, 50);
            insertCommand.AddString("@Address", AppConstants.NotProvided, 200);

            return Convert.ToInt32(insertCommand.ExecuteScalar());
        }

        private static bool CustomerHasActiveRental(SqlConnection connection, SqlTransaction transaction, int customerId)
        {
            using SqlCommand command = new(
                "SELECT COUNT(*) FROM Rentals WHERE CustomerId = @CustomerId AND Status = @ActiveStatus;",
                connection,
                transaction);
            command.AddInt("@CustomerId", customerId);
            command.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
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

            if (!TryGetSelectedCarId(out carId))
            {
                MessageBox.Show("Please add or select an available car first.", "No Available Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (returnDatePicker.Value.Date < rentalDatePicker.Value.Date)
            {
                MessageBox.Show("Return date cannot be earlier than rental date.", "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(dailyRateTextBox.Text, out dailyRate))
            {
                MessageBox.Show("Daily rate must be a valid amount.", "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dailyRate <= 0)
            {
                MessageBox.Show("Daily rate must be greater than zero.", "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            totalAmount = CalculateQuickRentalTotal();
            return true;
        }

        private bool TryGetSelectedCarId(out int carId)
        {
            carId = 0;

            if (selectedCarComboBox.SelectedValue is int selectedId)
            {
                carId = selectedId;
                return true;
            }

            if (selectedCarComboBox.SelectedValue != null && int.TryParse(selectedCarComboBox.SelectedValue.ToString(), out selectedId))
            {
                carId = selectedId;
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
            if (selectedCarComboBox.SelectedItem is DataRowView row
                && row.Row.Table.Columns.Contains("DailyRate")
                && row["DailyRate"] != DBNull.Value)
            {
                dailyRateTextBox.Text = Convert.ToDecimal(row["DailyRate"]).ToString("0.##");
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
            if (e.RowIndex < 0 || carsGrid.Rows[e.RowIndex].DataBoundItem is not DataRowView row)
                return;

            string plateNumber = row["Plate No."].ToString() ?? "";

            foreach (DataRowView item in selectedCarComboBox.Items)
            {
                if (item["CarName"].ToString()?.StartsWith(plateNumber) == true)
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
            {
                selectedCarComboBox.SelectedIndex = 0;
            }

            CalculateQuickRentalTotal();
        }

        private void LogoutButton_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            Close();
        }
    }
}
