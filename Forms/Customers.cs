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

                var query = db.Customers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c =>
                        c.FullName.Contains(search) ||
                        c.ContactNumber.Contains(search) ||
                        c.LicenseNumber.Contains(search));
                }

                var customers = query
                    .OrderByDescending(c => c.CustomerId)
                    .Select(c => new
                    {
                        c.CustomerId,
                        FullName = c.FullName,
                        ContactNumber = c.ContactNumber,
                        LicenseNumber = c.LicenseNumber,
                        c.Address
                    })
                    .ToList();

                customersGrid.Columns.Clear();
                customersGrid.DataSource = customers;

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

                var customer = new Customer
                {
                    FullName = nameTextBox.Text.Trim(),
                    ContactNumber = contactTextBox.Text.Trim(),
                    LicenseNumber = licenseTextBox.Text.Trim(),
                    Address = addressTextBox.Text.Trim()
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

                customer.FullName = nameTextBox.Text.Trim();
                customer.ContactNumber = contactTextBox.Text.Trim();
                customer.LicenseNumber = licenseTextBox.Text.Trim();
                customer.Address = addressTextBox.Text.Trim();

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

            DialogResult result = MessageBox.Show(
                "Are you sure you want to permanently delete this customer and their returned rental history?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                using var db = new AppDbContext();

                bool hasActiveRentals = db.Rentals.Any(r =>
                    r.CustomerId == selectedCustomerId.Value &&
                    r.Status == AppConstants.ActiveRentalStatus);

                if (hasActiveRentals)
                {
                    MessageBox.Show(
                        "This customer still has an active rental. Process the return before deleting the customer.",
                        "Active Rental Found",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                var rentals = db.Rentals.Where(r => r.CustomerId == selectedCustomerId.Value).ToList();
                db.Rentals.RemoveRange(rentals);

                var customer = db.Customers.Find(selectedCustomerId.Value);
                if (customer != null)
                    db.Customers.Remove(customer);

                db.SaveChanges();

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
            if (customersGrid.CurrentRow?.DataBoundItem == null || !customersGrid.CurrentRow.Selected)
            {
                selectedCustomerId = null;
                return;
            }

            dynamic row = customersGrid.CurrentRow.DataBoundItem;
            selectedCustomerId = (int)row.CustomerId;
            nameTextBox.Text = row.FullName;
            contactTextBox.Text = row.ContactNumber;
            licenseTextBox.Text = row.LicenseNumber;
            addressTextBox.Text = row.Address;
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