using System.Data;
using Microsoft.Data.SqlClient;

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
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                totalCarsValueLabel.Text = DatabaseHelper.ExecuteScalar(connection, "SELECT COUNT(*) FROM Cars;").ToString();
                availableCarsValueLabel.Text = DatabaseHelper.ExecuteScalar(
                    connection,
                    $"SELECT COUNT(*) FROM Cars WHERE Status = '{AppConstants.AvailableStatus}';").ToString();
                activeRentalsValueLabel.Text = DatabaseHelper.ExecuteScalar(
                    connection,
                    $"SELECT COUNT(*) FROM Rentals WHERE Status = '{AppConstants.ActiveRentalStatus}';").ToString();

                object income = DatabaseHelper.ExecuteScalar(
                    connection,
                    $"SELECT ISNULL(SUM(TotalAmount), 0) FROM Rentals WHERE Status = '{AppConstants.ReturnedRentalStatus}';");
                incomeValueLabel.Text = RentalCalculator.FormatCurrency(Convert.ToDecimal(income));

                LoadRentalHistory(connection, search);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadRentalHistory(SqlConnection connection, string search)
        {
            string query = @"
SELECT
    r.RentalId AS [Rental ID],
    c.FullName AS Customer,
    ca.PlateNumber + ' - ' + ca.Brand + ' ' + ca.Model AS Car,
    r.RentalDate AS [Rental Date],
    r.ExpectedReturnDate AS [Expected Return],
    r.ActualReturnDate AS [Actual Return],
    r.TotalAmount AS [Total Amount],
    r.Status
FROM Rentals r
INNER JOIN Customers c ON c.CustomerId = r.CustomerId
INNER JOIN Cars ca ON ca.CarId = r.CarId
WHERE
    @Search = ''
    OR c.FullName LIKE @SearchText
    OR ca.PlateNumber LIKE @SearchText
    OR ca.Brand LIKE @SearchText
    OR ca.Model LIKE @SearchText
    OR r.Status LIKE @SearchText
ORDER BY r.RentalId DESC;";

            using SqlCommand command = new(query, connection);
            command.AddSearchParameters(search);

            DataTable table = new();
            using SqlDataAdapter adapter = new(command);
            adapter.Fill(table);

            reportGrid.Columns.Clear();
            reportGrid.DataSource = table;

            DataGridViewHelper.HideColumn(reportGrid, "Rental ID");
            DataGridViewHelper.ClearSelectionWhenReady(reportGrid);
        }
    }
}
