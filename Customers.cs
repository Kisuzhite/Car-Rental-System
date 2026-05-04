using System.Data;
using Microsoft.Data.SqlClient;

namespace Car_Rental_System
{
    public partial class Customers : Form
    {
        private int? selectedCustomerId;

        public Customers()
        {
            InitializeComponent();
        }

        private void Customers_Load(object? sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers(string search = "")
        {
            try
            {
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                string query = @"
SELECT
    CustomerId,
    FullName AS [Full Name],
    ContactNumber AS [Contact Number],
    LicenseNumber AS [License Number],
    Address
FROM Customers
WHERE
    @Search = ''
    OR FullName LIKE @SearchText
    OR ContactNumber LIKE @SearchText
    OR LicenseNumber LIKE @SearchText
ORDER BY CustomerId DESC;";

                using SqlCommand command = new(query, connection);
                command.AddSearchParameters(search);

                DataTable table = new();
                using SqlDataAdapter adapter = new(command);
                adapter.Fill(table);

                customersGrid.Columns.Clear();
                customersGrid.DataSource = table;

                DataGridViewHelper.HideColumn(customersGrid, "CustomerId");
                BeginInvoke(new Action(ClearForm));
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                string query = @"
INSERT INTO Customers (FullName, ContactNumber, LicenseNumber, Address)
VALUES (@FullName, @ContactNumber, @LicenseNumber, @Address);";

                using SqlCommand command = new(query, connection);
                AddCustomerParameters(command);
                command.ExecuteNonQuery();

                MessageBox.Show("Customer added successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadCustomers(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add customer.", ex);
            }
        }

        private void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedCustomerId == null)
            {
                MessageBox.Show("Please select a customer to update.", "No Customer Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                using SqlConnection connection = DatabaseHelper.OpenConnection();

                string query = @"
UPDATE Customers
SET FullName = @FullName,
    ContactNumber = @ContactNumber,
    LicenseNumber = @LicenseNumber,
    Address = @Address
WHERE CustomerId = @CustomerId;";

                using SqlCommand command = new(query, connection);
                AddCustomerParameters(command);
                command.AddInt("@CustomerId", selectedCustomerId.Value);
                command.ExecuteNonQuery();

                MessageBox.Show("Customer updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadCustomers(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update customer.", ex);
            }
        }

        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (selectedCustomerId == null)
            {
                MessageBox.Show("Please select a customer to delete.", "No Customer Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to permanently delete this customer and their returned rental history?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using SqlConnection connection = DatabaseHelper.OpenConnection();
                using SqlTransaction transaction = connection.BeginTransaction();

                using SqlCommand activeCommand = new(
                    "SELECT COUNT(*) FROM Rentals WHERE CustomerId = @CustomerId AND Status = @ActiveStatus;",
                    connection,
                    transaction);
                activeCommand.AddInt("@CustomerId", selectedCustomerId.Value);
                activeCommand.AddString("@ActiveStatus", AppConstants.ActiveRentalStatus, 20);
                int activeRentals = Convert.ToInt32(activeCommand.ExecuteScalar());

                if (activeRentals > 0)
                {
                    transaction.Rollback();
                    MessageBox.Show(
                        "This customer still has an active rental. Process the return before deleting the customer.",
                        "Active Rental Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                using SqlCommand deleteRentalsCommand = new("DELETE FROM Rentals WHERE CustomerId = @CustomerId;", connection, transaction);
                deleteRentalsCommand.AddInt("@CustomerId", selectedCustomerId.Value);
                deleteRentalsCommand.ExecuteNonQuery();

                using SqlCommand deleteCustomerCommand = new("DELETE FROM Customers WHERE CustomerId = @CustomerId;", connection, transaction);
                deleteCustomerCommand.AddInt("@CustomerId", selectedCustomerId.Value);
                deleteCustomerCommand.ExecuteNonQuery();

                transaction.Commit();

                MessageBox.Show("Customer deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadCustomers(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to delete customer.", ex);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadCustomers(searchTextBox.Text);
        }

        private void CustomersGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (customersGrid.CurrentRow?.DataBoundItem is not DataRowView row || !customersGrid.CurrentRow.Selected)
            {
                selectedCustomerId = null;
                return;
            }

            selectedCustomerId = Convert.ToInt32(row["CustomerId"]);
            nameTextBox.Text = row["Full Name"].ToString();
            contactTextBox.Text = row["Contact Number"].ToString();
            licenseTextBox.Text = row["License Number"].ToString();
            addressTextBox.Text = row["Address"].ToString();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(contactTextBox.Text) ||
                string.IsNullOrWhiteSpace(licenseTextBox.Text) ||
                string.IsNullOrWhiteSpace(addressTextBox.Text))
            {
                MessageBox.Show("Please complete all customer details.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void AddCustomerParameters(SqlCommand command)
        {
            command.AddString("@FullName", nameTextBox.Text.Trim(), 100);
            command.AddString("@ContactNumber", contactTextBox.Text.Trim(), 30);
            command.AddString("@LicenseNumber", licenseTextBox.Text.Trim(), 50);
            command.AddString("@Address", addressTextBox.Text.Trim(), 200);
        }

        private void ClearForm()
        {
            selectedCustomerId = null;
            nameTextBox.Clear();
            contactTextBox.Clear();
            licenseTextBox.Clear();
            addressTextBox.Clear();
            customersGrid.ClearSelection();
        }
    }
}
