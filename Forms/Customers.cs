using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

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
                using var db = new AppDbContext();

                var query = db.Customers
                    .Where(c => !c.IsArchived)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.FullName.Contains(search) ||
                        c.ContactNumber.Contains(search) ||
                        c.LicenseNumber.Contains(search));
                }

                var customers = query
                    .OrderByDescending(c => c.CustomerId)
                    .Select(c => new CustomerGridRow
                    {
                        CustomerId = c.CustomerId,
                        FullName = c.FullName,
                        ContactNumber = c.ContactNumber,
                        LicenseNumber = c.LicenseNumber,
                        Address = c.Address
                    })
                    .ToList();

                customersGrid.Columns.Clear();
                customersGrid.DataSource = customers;

                DataGridViewHelper.ApplyFriendlyHeaders(customersGrid);
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
            if (!ValidateInputs()) return;

            try
            {
                using var db = new AppDbContext();

                string licenseNumber = ValidationHelper.NormalizeLicenseNumber(licenseTextBox.Text);

                if (IsDuplicateLicenseNumber(db, licenseNumber))
                {
                    ShowDuplicateLicenseMessage(licenseNumber);
                    return;
                }

                var customer = new Customer
                {
                    FullName = ValidationHelper.CollapseSpaces(nameTextBox.Text),
                    ContactNumber = ValidationHelper.CollapseSpaces(contactTextBox.Text),
                    LicenseNumber = licenseNumber,
                    Address = ValidationHelper.CollapseSpaces(addressTextBox.Text)
                };

                db.Customers.Add(customer);
                db.SaveChanges();

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

            if (!ValidateInputs()) return;

            try
            {
                using var db = new AppDbContext();

                var customer = db.Customers.Find(selectedCustomerId.Value);
                if (customer == null) return;

                string licenseNumber = ValidationHelper.NormalizeLicenseNumber(licenseTextBox.Text);

                if (IsDuplicateLicenseNumber(db, licenseNumber, selectedCustomerId.Value))
                {
                    ShowDuplicateLicenseMessage(licenseNumber);
                    return;
                }

                customer.FullName = ValidationHelper.CollapseSpaces(nameTextBox.Text);
                customer.ContactNumber = ValidationHelper.CollapseSpaces(contactTextBox.Text);
                customer.LicenseNumber = licenseNumber;
                customer.Address = ValidationHelper.CollapseSpaces(addressTextBox.Text);

                db.SaveChanges();

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

            try
            {
                using var db = new AppDbContext();

                var customer = db.Customers
                    .Include(c => c.Rentals)
                    .FirstOrDefault(c => c.CustomerId == selectedCustomerId.Value);

                if (customer == null)
                {
                    MessageBox.Show("Customer not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadCustomers(searchTextBox.Text);
                    return;
                }

                bool hasActiveRentals = customer.Rentals.Any(r =>
                    r.Status == RentalStatus.Active);

                if (hasActiveRentals)
                {
                    MessageBox.Show(
                        "This customer still has an active rental. Process the return before deleting the customer.",
                        "Active Rental Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                bool hasRentalHistory = customer.Rentals.Any();

                string confirmMessage = hasRentalHistory
                    ? "This customer has rental history. Archive the customer instead of deleting their records?"
                    : "Are you sure you want to delete this customer?";

                DialogResult result = MessageBox.Show(
                    confirmMessage,
                    hasRentalHistory ? "Confirm Archive" : "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    hasRentalHistory ? MessageBoxIcon.Information : MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                if (hasRentalHistory)
                {
                    customer.IsArchived = true;
                }
                else
                {
                    db.Customers.Remove(customer);
                }

                db.SaveChanges();

                MessageBox.Show(
                    hasRentalHistory
                        ? "Customer archived successfully. Rental history was preserved."
                        : "Customer deleted successfully.",
                    hasRentalHistory ? "Archived" : "Deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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
            if (customersGrid.CurrentRow?.DataBoundItem == null || !customersGrid.CurrentRow.Selected)
            {
                selectedCustomerId = null;
                return;
            }

            if (customersGrid.CurrentRow.DataBoundItem is not CustomerGridRow row)
            {
                selectedCustomerId = null;
                return;
            }

            selectedCustomerId = row.CustomerId;
            nameTextBox.Text = row.FullName;
            contactTextBox.Text = row.ContactNumber;
            licenseTextBox.Text = row.LicenseNumber;
            addressTextBox.Text = row.Address;
        }

        private bool ValidateInputs()
        {
            string fullName = ValidationHelper.CollapseSpaces(nameTextBox.Text);
            string contactNumber = ValidationHelper.CollapseSpaces(contactTextBox.Text);
            string licenseNumber = ValidationHelper.NormalizeLicenseNumber(licenseTextBox.Text);
            string address = ValidationHelper.CollapseSpaces(addressTextBox.Text);

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(contactNumber) ||
                string.IsNullOrWhiteSpace(licenseNumber) ||
                string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please complete all customer details.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidPersonName(fullName))
            {
                MessageBox.Show("Customer name must be 2-100 characters and contain only letters, spaces, apostrophes, periods, or hyphens.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidContactNumber(contactNumber))
            {
                MessageBox.Show("Contact number must contain 7-15 digits and may only include spaces, +, -, parentheses, or periods.", "Invalid Contact Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidLicenseNumber(licenseNumber))
            {
                MessageBox.Show("License number must be 3-50 letters/numbers and may only include spaces or hyphens.", "Invalid License Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (address.Length < 5 || address.Length > 200)
            {
                MessageBox.Show("Address must be between 5 and 200 characters.", "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private static bool IsDuplicateLicenseNumber(AppDbContext db, string licenseNumber, int? excludedCustomerId = null)
        {
            licenseNumber = ValidationHelper.NormalizeLicenseNumber(licenseNumber);

            if (string.Equals(licenseNumber, AppConstants.NotProvided, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return db.Customers.Any(c =>
                c.LicenseNumber == licenseNumber &&
                (!excludedCustomerId.HasValue || c.CustomerId != excludedCustomerId.Value));
        }

        private static void ShowDuplicateLicenseMessage(string licenseNumber)
        {
            MessageBox.Show(
                $"A customer with license number '{licenseNumber}' already exists.",
                "Duplicate License Number",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
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

        private sealed class CustomerGridRow
        {
            public int CustomerId { get; set; }
            public string FullName { get; set; } = "";
            public string ContactNumber { get; set; } = "";
            public string LicenseNumber { get; set; } = "";
            public string Address { get; set; } = "";
        }
    }
}
