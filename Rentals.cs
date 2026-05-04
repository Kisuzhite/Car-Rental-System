using System.Data;
using Microsoft.Data.SqlClient;

namespace Car_Rental_System
{
    public partial class Rentals : Form
    {
        private int? selectedRentalId;

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
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                LoadCustomers(connection);
                LoadAvailableCars(connection);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadCustomers(SqlConnection connection)
        {
            using SqlCommand command = new("SELECT CustomerId, FullName FROM Customers ORDER BY FullName;", connection);
            DataTable table = new();
            using SqlDataAdapter adapter = new(command);
            adapter.Fill(table);

            customerComboBox.DataSource = table;
            customerComboBox.DisplayMember = "FullName";
            customerComboBox.ValueMember = "CustomerId";
        }

        private void LoadAvailableCars(SqlConnection connection)
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

            carComboBox.DataSource = table;
            carComboBox.DisplayMember = "CarName";
            carComboBox.ValueMember = "CarId";

            if (table.Rows.Count == 0)
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
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                string query = @"
SELECT
    r.RentalId,
    c.FullName AS Customer,
    ca.PlateNumber + ' - ' + ca.Brand + ' ' + ca.Model AS Car,
    r.RentalDate AS [Rental Date],
    r.ExpectedReturnDate AS [Return Date],
    r.TotalAmount AS [Total Amount],
    r.Status
FROM Rentals r
INNER JOIN Customers c ON c.CustomerId = r.CustomerId
INNER JOIN Cars ca ON ca.CarId = r.CarId
WHERE
    r.Status = @ActiveStatus
    AND (
        @Search = ''
        OR c.FullName LIKE @SearchText
        OR ca.PlateNumber LIKE @SearchText
        OR ca.Brand LIKE @SearchText
        OR ca.Model LIKE @SearchText
    )
ORDER BY r.RentalId DESC;";

                using SqlCommand command = new(query, connection);
                command.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);
                command.AddSearchParameters(search);

                DataTable table = new();
                using SqlDataAdapter adapter = new(command);
                adapter.Fill(table);

                rentalsGrid.Columns.Clear();
                rentalsGrid.DataSource = table;

                DataGridViewHelper.HideColumn(rentalsGrid, "RentalId");
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
            {
                return;
            }

            try
            {
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                using SqlCommand activeCheckCommand = new(
                    "SELECT COUNT(*) FROM Rentals WHERE CustomerId = @CustomerId AND Status = @ActiveStatus;",
                    connection);
                activeCheckCommand.AddInt("@CustomerId", customerId);
                activeCheckCommand.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);
                int activeRentals = Convert.ToInt32(activeCheckCommand.ExecuteScalar());

                if (activeRentals > 0)
                {
                    MessageBox.Show(
                        "This customer already has an active rental.\nProcess the return first before renting another car.",
                        "Customer Has Active Rental",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using SqlTransaction transaction = connection.BeginTransaction();

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
                    LoadLookups();
                    LoadRentals(searchTextBox.Text);
                    return;
                }

                string insertRental = @"
INSERT INTO Rentals (CustomerId, CarId, RentalDate, ExpectedReturnDate, DailyRate, TotalAmount, Status)
VALUES (@CustomerId, @CarId, @RentalDate, @ExpectedReturnDate, @DailyRate, @TotalAmount, @ActiveStatus);";

                using SqlCommand rentalCommand = new(insertRental, connection, transaction);
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
                ClearForm();
                LoadLookups();
                LoadRentals(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to create rental.", ex);
            }
        }

        private void ReturnedButton_Click(object? sender, EventArgs e)
        {
            if (selectedRentalId == null)
            {
                MessageBox.Show("Please select a rental transaction first.", "No Rental Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using SqlConnection connection = DatabaseHelper.OpenConnection();
                using SqlTransaction transaction = connection.BeginTransaction();

                using SqlCommand getRentalCommand = new(
                    "SELECT CarId, ExpectedReturnDate, DailyRate, TotalAmount FROM Rentals WHERE RentalId = @RentalId AND Status = @ActiveStatus;",
                    connection,
                    transaction);
                getRentalCommand.AddInt("@RentalId", selectedRentalId.Value);
                getRentalCommand.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);

                int carId;
                DateTime expectedReturnDate;
                decimal dailyRate;
                decimal baseAmount;
                bool rentalFound = false;

                using (SqlDataReader reader = getRentalCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rentalFound = true;
                        carId = Convert.ToInt32(reader["CarId"]);
                        expectedReturnDate = Convert.ToDateTime(reader["ExpectedReturnDate"]);
                        dailyRate = Convert.ToDecimal(reader["DailyRate"]);
                        baseAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    }
                    else
                    {
                        carId = 0;
                        expectedReturnDate = DateTime.Today;
                        dailyRate = 0;
                        baseAmount = 0;
                    }
                }

                if (!rentalFound)
                {
                    transaction.Rollback();
                    MessageBox.Show(
                        "This rental is no longer active.",
                        "Rental Already Closed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadRentals(searchTextBox.Text);
                    return;
                }

                DateTime actualReturnDate = DateTime.Today;
                decimal finalAmount = RentalCalculator.CalculateFinalAmount(
                    expectedReturnDate,
                    actualReturnDate,
                    dailyRate,
                    baseAmount);

                using SqlCommand rentalCommand = new(
                    "UPDATE Rentals SET ActualReturnDate = @ActualReturnDate, TotalAmount = @TotalAmount, Status = @ReturnedStatus WHERE RentalId = @RentalId AND Status = @ActiveStatus;",
                    connection,
                    transaction);
                rentalCommand.AddDate("@ActualReturnDate", actualReturnDate);
                rentalCommand.AddMoney("@TotalAmount", finalAmount);
                rentalCommand.AddString("@ReturnedStatus", AppConstants.ReturnedRentalStatus, 20);
                rentalCommand.AddInt("@RentalId", selectedRentalId.Value);
                rentalCommand.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);

                if (rentalCommand.ExecuteNonQuery() == 0)
                {
                    transaction.Rollback();
                    MessageBox.Show(
                        "This rental is no longer active.",
                        "Rental Already Closed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadRentals(searchTextBox.Text);
                    return;
                }

                using SqlCommand carCommand = new("UPDATE Cars SET Status = @AvailableStatus WHERE CarId = @CarId;", connection, transaction);
                carCommand.AddString("@AvailableStatus", AppConstants.AvailableStatus, 20);
                carCommand.AddInt("@CarId", carId);
                carCommand.ExecuteNonQuery();

                transaction.Commit();

                MessageBox.Show("Rental marked as returned.", "Returned", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadLookups();
                LoadRentals(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to mark rental as returned.", ex);
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

        private void RentalsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (rentalsGrid.CurrentRow?.DataBoundItem is not DataRowView row || !rentalsGrid.CurrentRow.Selected)
            {
                selectedRentalId = null;
                return;
            }

            selectedRentalId = Convert.ToInt32(row["RentalId"]);
        }

        private void CarComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (carComboBox.SelectedItem is DataRowView row && row.Row.Table.Columns.Contains("DailyRate"))
            {
                rateTextBox.Text = row["DailyRate"].ToString();
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

            if (returnDatePicker.Value.Date < rentalDatePicker.Value.Date)
            {
                MessageBox.Show("Return date cannot be earlier than rental date.", "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(rateTextBox.Text, out dailyRate))
            {
                MessageBox.Show("Daily rate must be a valid amount.", "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dailyRate <= 0)
            {
                MessageBox.Show("Daily rate must be greater than zero.", "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            selectedRentalId = null;
            if (customerComboBox.Items.Count > 0)
            {
                customerComboBox.SelectedIndex = 0;
            }

            if (carComboBox.Items.Count > 0)
            {
                carComboBox.SelectedIndex = 0;
            }

            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);

            rentalsGrid.ClearSelection();
            CalculateTotal();
        }
    }
}
