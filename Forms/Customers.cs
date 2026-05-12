using Car_Rental_System.Helpers;
using Car_Rental_System.Presenters;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System
{
    public partial class Customers : Form, ICustomersView
    {
        private readonly CustomersPresenter presenter;
        private int? selectedCustomerId;

        public Customers()
        {
            InitializeComponent();
            presenter = new CustomersPresenter(this);
        }

        public string SearchText => searchTextBox.Text;

        void ICustomersView.BindCustomers(List<CustomerGridRow> customers)
        {
            customersGrid.Columns.Clear();
            customersGrid.DataSource = customers;

            DataGridViewHelper.ApplyFriendlyHeaders(customersGrid);
            DataGridViewHelper.HideColumn(customersGrid, "CustomerId");
            BeginInvoke(new Action(ClearForm));
        }

        private async void Customers_Load(object? sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                await presenter.LoadCustomersAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private async void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out CustomerFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.AddCustomerAsync(input);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    ClearForm();
                    await LoadCustomersAsync();
                }
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add customer.", ex);
            }
        }

        private async void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedCustomerId == null)
            {
                MessageBox.Show("Please select a customer to update.", "No Customer Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInputs(out CustomerFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.UpdateCustomerAsync(selectedCustomerId.Value, input);
                AppMessageBox.ShowPresenterResult(result);

                if (!result.Success)
                {
                    return;
                }

                await RentalDocumentPrinter.PromptToPrintRentalAgreementAsync(this, result.RentalId);
                ClearForm();
                await LoadCustomersAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update customer.", ex);
            }
        }

        private async void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (selectedCustomerId == null)
            {
                MessageBox.Show("Please select a customer to delete.", "No Customer Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                CustomerDeletePreview preview = await presenter.PreviewDeleteAsync(selectedCustomerId.Value);
                if (!preview.CanContinue)
                {
                    MessageBox.Show(preview.Message, preview.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult confirmation = MessageBox.Show(
                    preview.Message,
                    preview.Title,
                    MessageBoxButtons.YesNo,
                    preview.HasRentalHistory ? MessageBoxIcon.Information : MessageBoxIcon.Question);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                PresenterResult result = await presenter.DeleteCustomerAsync(selectedCustomerId.Value);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    ClearForm();
                    await LoadCustomersAsync();
                }
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

        private async void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private void CustomersGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (customersGrid.CurrentRow?.DataBoundItem is not CustomerGridRow row ||
                !customersGrid.CurrentRow.Selected)
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

        private bool ValidateInputs(out CustomerFormInput input)
        {
            string fullName = ValidationHelper.CollapseSpaces(nameTextBox.Text);
            string contactNumber = ValidationHelper.CollapseSpaces(contactTextBox.Text);
            string licenseNumber = ValidationHelper.NormalizeLicenseNumber(licenseTextBox.Text);
            string address = ValidationHelper.CollapseSpaces(addressTextBox.Text);
            input = new CustomerFormInput(fullName, contactNumber, licenseNumber, address);

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
