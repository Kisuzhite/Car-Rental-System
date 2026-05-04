using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace Car_Rental_System
{
    public partial class ReturnCar : Form
    {
        private int? selectedRentalId;
        private int? selectedCarId;

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
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                string query = @"
SELECT
    r.RentalId,
    r.CarId,
    c.FullName AS Customer,
    ca.PlateNumber + ' - ' + ca.Brand + ' ' + ca.Model AS Car,
    r.RentalDate AS [Rental Date],
    r.ExpectedReturnDate AS [Expected Return],
    r.DailyRate AS [Daily Rate],
    r.TotalAmount AS [Base Amount],
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
ORDER BY r.ExpectedReturnDate ASC;";

                using SqlCommand command = new(query, connection);
                command.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);
                command.AddSearchParameters(search);

                DataTable table = new();
                using SqlDataAdapter adapter = new(command);
                adapter.Fill(table);

                activeRentalsGrid.Columns.Clear();
                activeRentalsGrid.DataSource = table;

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
            if (activeRentalsGrid.CurrentRow?.DataBoundItem is not DataRowView row || !activeRentalsGrid.CurrentRow.Selected)
            {
                ClearDetails();
                return;
            }

            selectedRentalId = Convert.ToInt32(row["RentalId"]);
            selectedCarId = Convert.ToInt32(row["CarId"]);
            customerTextBox.Text = row["Customer"].ToString();
            carTextBox.Text = row["Car"].ToString();
            expectedReturnTextBox.Text = Convert.ToDateTime(row["Expected Return"]).ToShortDateString();
            dailyRateTextBox.Text = Convert.ToDecimal(row["Daily Rate"]).ToString("N2");
            baseAmountTextBox.Text = Convert.ToDecimal(row["Base Amount"]).ToString("N2");
            CalculateFinalAmount(row);
        }

        private void ActualReturnDatePicker_ValueChanged(object? sender, EventArgs e)
        {
            if (activeRentalsGrid.CurrentRow?.DataBoundItem is DataRowView row)
            {
                CalculateFinalAmount(row);
            }
        }

        private void CalculateFinalAmount(DataRowView row)
        {
            DateTime expectedReturn = Convert.ToDateTime(row["Expected Return"]);
            decimal dailyRate = Convert.ToDecimal(row["Daily Rate"]);
            decimal baseAmount = Convert.ToDecimal(row["Base Amount"]);
            int lateDays = RentalCalculator.GetLateDays(expectedReturn, actualReturnDatePicker.Value);
            decimal penalty = lateDays * dailyRate;
            decimal finalAmount = RentalCalculator.CalculateFinalAmount(
                expectedReturn,
                actualReturnDatePicker.Value,
                dailyRate,
                baseAmount);

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

            if (activeRentalsGrid.CurrentRow?.DataBoundItem is not DataRowView row)
            {
                MessageBox.Show("Please select an active rental to return.", "No Rental Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime rentalDate = Convert.ToDateTime(row["Rental Date"]);
            if (actualReturnDatePicker.Value.Date < rentalDate.Date)
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
                using SqlConnection connection = DatabaseHelper.OpenConnection();
                using SqlTransaction transaction = connection.BeginTransaction();

                using SqlCommand rentalCommand = new(
                    "UPDATE Rentals SET ActualReturnDate = @ActualReturnDate, TotalAmount = @TotalAmount, Status = @ReturnedStatus WHERE RentalId = @RentalId AND Status = @ActiveStatus;",
                    connection,
                    transaction);
                rentalCommand.AddDate("@ActualReturnDate", actualReturnDatePicker.Value);
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
                    LoadActiveRentals(searchTextBox.Text);
                    return;
                }

                using SqlCommand carCommand = new("UPDATE Cars SET Status = @AvailableStatus WHERE CarId = @CarId;", connection, transaction);
                carCommand.AddString("@AvailableStatus", AppConstants.AvailableStatus, 20);
                carCommand.AddInt("@CarId", selectedCarId.Value);
                carCommand.ExecuteNonQuery();

                transaction.Commit();

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
