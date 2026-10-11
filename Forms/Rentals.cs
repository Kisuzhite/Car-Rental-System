using System.Diagnostics;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System
{
    public partial class Rentals : Form, IRentalsView
    {
        private readonly RentalsPresenter presenter;
        private List<CustomerLookupItem> customerLookupItems = [];
        private string matchedCustomerName = "";

        public Rentals()
        {
            InitializeComponent();
            presenter = new RentalsPresenter(this);
        }

        public string SearchText => string.Empty;

        void IRentalsView.BindCustomers(List<CustomerLookupItem> customers)
        {
            customerLookupItems = customers;

            var customerNames = new AutoCompleteStringCollection();
            customerNames.AddRange(customers.Select(c => c.FullName).ToArray());

            customerNameTextBox.AutoCompleteCustomSource = customerNames;
            customerNameTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            customerNameTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            FillCustomerDetailsForMatchedName();
        }

        void IRentalsView.BindAvailableCars(List<RentalCarLookupItem> cars)
        {
            carComboBox.DataSource = cars;
            carComboBox.DisplayMember = "CarName";
            carComboBox.ValueMember = "CarId";

            if (cars.Count == 0)
            {
                rateTextBox.Clear();
                totalTextBox.Text = RentalCalculator.FormatCurrency(0);
            }

            CarComboBox_SelectedIndexChanged(this, EventArgs.Empty);
        }

        void IRentalsView.BindRentals(List<RentalGridRow> rentals)
        {
            rentalsGrid.DataSource = rentals;

            DataGridViewHelper.ApplyFriendlyHeaders(rentalsGrid);
            DataGridViewHelper.HideColumn(rentalsGrid, "RentalId");
            DataGridViewHelper.ApplyStatusFormatting(rentalsGrid);
            DataGridViewHelper.ApplyFullTextColumn(rentalsGrid, "Car", 340);
            DataGridViewHelper.ApplyEmptyState(rentalsGrid, "No active rentals found.");
            DataGridViewHelper.ClearSelectionWhenReady(rentalsGrid);
        }

        private async void Rentals_Load(object? sender, EventArgs e)
        {
            PrepareForm();
            await LoadLookupsAsync();
            await LoadRentalsAsync();
        }

        private void PrepareForm()
        {
            paymentMethodComboBox.Format += PaymentMethodComboBox_Format;
            paymentMethodComboBox.SelectedIndexChanged += PaymentMethodComboBox_SelectedIndexChanged;
            paymentMethodComboBox.DataSource = PaymentHelper.GetSelectablePaymentMethods();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            SetLicenseFieldLocked(false);
            UpdatePaymentReferenceVisibility();
            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);
            UpdateCreateButtonText();
            CalculateTotal();
        }

        private static void PaymentMethodComboBox_Format(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is PaymentMethod paymentMethod)
            {
                e.Value = PaymentHelper.FormatPaymentMethod(paymentMethod);
            }
        }

        private async Task LoadLookupsAsync()
        {
            try
            {
                await presenter.LoadLookupsAsync(
                    RentalCalculator.ToDateOnly(rentalDatePicker.Value),
                    RentalCalculator.ToDateOnly(returnDatePicker.Value));
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private async Task LoadAvailableCarsAsync()
        {
            try
            {
                await presenter.LoadAvailableCarsAsync(
                    RentalCalculator.ToDateOnly(rentalDatePicker.Value),
                    RentalCalculator.ToDateOnly(returnDatePicker.Value));
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to load available cars.", ex);
            }
        }

        private async Task LoadRentalsAsync()
        {
            try
            {
                await presenter.LoadRentalsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                AppMessageBox.ShowDatabaseError("Unable to load active rentals.", ex);
            }
        }

        private async void CreateButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out BookingFormInput input))
            {
                return;
            }

            using var busy = UiBusyScope.Disable(createButton, checkCalendarButton);

            try
            {
                BookingOperationResult result = await presenter.CreateBookingAsync(input);

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        result.Title,
                        MessageBoxButtons.OK,
                        GetRentalFailureIcon(result));

                    if (result.CarWasUnavailable || result.HasScheduleConflict)
                    {
                        await LoadLookupsAsync();
                        await LoadRentalsAsync();
                    }
                    return;
                }

                MessageBox.Show(result.Message, result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result.CreatedRental)
                {
                    await RentalDocumentPrinter.PromptToPrintRentalAgreementAsync(this, result.RentalId);
                }

                ClearForm();
                await LoadLookupsAsync();
                await LoadRentalsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to create rental.", ex);
            }
        }

        private void CarComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (carComboBox.SelectedItem is RentalCarLookupItem selected)
            {
                rateTextBox.Text = RentalCalculator.FormatCurrency(selected.DailyRate);
            }
            else
            {
                rateTextBox.Clear();
            }

            CalculateTotal();
        }

        private void CheckCalendarButton_Click(object? sender, EventArgs e)
        {
            int? selectedCarId = TryGetSelectedValue(carComboBox, out int carId)
                ? carId
                : null;

            using var calendar = new AvailabilityCalendar(
                RentalCalculator.ToDateOnly(rentalDatePicker.Value),
                RentalCalculator.ToDateOnly(returnDatePicker.Value),
                selectedCarId);

            calendar.ShowDialog(this);
        }

        private void CustomerNameTextBox_TextChanged(object? sender, EventArgs e)
        {
            FillCustomerDetailsForMatchedName();
        }

        private void FillCustomerDetailsForMatchedName()
        {
            string customerName = ValidationHelper.CollapseSpaces(customerNameTextBox.Text);
            CustomerLookupItem? selected = customerLookupItems.FirstOrDefault(c =>
                string.Equals(c.FullName, customerName, StringComparison.OrdinalIgnoreCase));

            if (selected == null)
            {
                if (!string.IsNullOrWhiteSpace(matchedCustomerName))
                {
                    contactTextBox.Clear();
                    licenseTextBox.Clear();
                    addressTextBox.Clear();
                    matchedCustomerName = "";
                }

                SetLicenseFieldLocked(false);
                return;
            }

            if (string.Equals(matchedCustomerName, selected.FullName, StringComparison.OrdinalIgnoreCase))
            {
                SetLicenseFieldLocked(true);
                return;
            }

            matchedCustomerName = selected.FullName;
            contactTextBox.Text = selected.ContactNumber == AppConstants.NotProvided
                ? ""
                : selected.ContactNumber;
            licenseTextBox.Text = selected.LicenseNumber == AppConstants.NotProvided
                ? ""
                : selected.LicenseNumber;
            addressTextBox.Text = selected.Address == AppConstants.NotProvided
                ? ""
                : selected.Address;
            SetLicenseFieldLocked(true);
        }

        private async void DateOrRate_Changed(object? sender, EventArgs e)
        {
            UpdateCreateButtonText();
            CalculateTotal();

            if (ReferenceEquals(sender, rentalDatePicker) || ReferenceEquals(sender, returnDatePicker))
            {
                await LoadAvailableCarsAsync();
            }
        }

        private void PaymentAmountTextBox_TextChanged(object? sender, EventArgs e)
        {
            CalculateBalance();
        }

        private void PaymentMethodComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdatePaymentReferenceVisibility();
        }

        private bool ValidateInputs(out BookingFormInput input)
        {
            input = new BookingFormInput("", AppConstants.NotProvided, AppConstants.NotProvided, AppConstants.NotProvided, 0, rentalDatePicker.Value, returnDatePicker.Value, 0, PaymentMethod.Cash, AppConstants.NotProvided);

            string customerName = ValidationHelper.CollapseSpaces(customerNameTextBox.Text);
            string contactNumber = ValidationHelper.CollapseSpaces(contactTextBox.Text);
            string licenseNumber = ValidationHelper.NormalizeLicenseNumber(licenseTextBox.Text);
            string address = ValidationHelper.CollapseSpaces(addressTextBox.Text);
            string paymentReferenceNumber = ValidationHelper.CollapseSpaces(paymentReferenceTextBox.Text);

            if (!ValidationHelper.IsValidPersonName(customerName))
            {
                MessageBox.Show("Customer name must be 2-100 characters and contain only letters, spaces, apostrophes, periods, or hyphens.", "Invalid Customer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidContactNumber(contactNumber))
            {
                MessageBox.Show("Please enter a valid customer contact number.", "Invalid Contact Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidLicenseNumber(licenseNumber))
            {
                MessageBox.Show("License number must be 3-50 letters/numbers and may only include spaces or hyphens.", "Invalid License Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (address.Length < 5 || address.Length > 200)
            {
                MessageBox.Show("Billing address must be between 5 and 200 characters.", "Invalid Billing Address", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!TryGetSelectedValue(carComboBox, out int carId))
            {
                MessageBox.Show("Please select a car available for those dates.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (rentalDatePicker.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Pickup date cannot be in the past.", "Invalid Pickup Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.TryValidateRentalDates(rentalDatePicker.Value, returnDatePicker.Value, out string dateError))
            {
                MessageBox.Show(dateError, "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!RentalCalculator.TryParseCurrency(rateTextBox.Text, out decimal dailyRate))
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

            decimal totalAmount = CalculateTotal();
            decimal amountPaid = ParseOptionalPaymentAmount(paymentAmountTextBox.Text);

            if (!PaymentHelper.IsValidInitialPaymentAmount(amountPaid, totalAmount))
            {
                decimal minimumDeposit = PaymentHelper.GetMinimumDepositAmount(totalAmount);
                MessageBox.Show(
                    $"Amount paid must be at least the minimum deposit of {RentalCalculator.FormatCurrency(minimumDeposit)} and cannot be greater than the total amount.",
                    "Invalid Payment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            PaymentMethod paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
                ? selectedMethod
                : PaymentMethod.Cash;

            bool requiresReference = PaymentHelper.RequiresReferenceNumber(paymentMethod);
            if (requiresReference && string.IsNullOrWhiteSpace(paymentReferenceNumber))
            {
                MessageBox.Show(
                    $"Please enter the payment reference for {PaymentHelper.FormatPaymentMethod(paymentMethod)}.",
                    "Missing Payment Reference",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            paymentReferenceNumber = requiresReference
                ? paymentReferenceNumber
                : AppConstants.NotProvided;

            if (paymentReferenceNumber.Length > 80)
            {
                MessageBox.Show("Payment reference number cannot be longer than 80 characters.", "Invalid Reference", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            input = new BookingFormInput(
                customerName,
                contactNumber,
                licenseNumber,
                address,
                carId,
                rentalDatePicker.Value,
                returnDatePicker.Value,
                amountPaid,
                paymentMethod,
                paymentReferenceNumber);
            return true;
        }

        private static MessageBoxIcon GetRentalFailureIcon(BookingOperationResult result)
        {
            return result.CarWasUnavailable || result.HasScheduleConflict
                ? MessageBoxIcon.Information
                : MessageBoxIcon.Warning;
        }

        private static bool TryGetSelectedValue(ComboBox comboBox, out int value)
        {
            value = 0;
            return comboBox.SelectedValue != null &&
                int.TryParse(comboBox.SelectedValue.ToString(), out value);
        }

        private decimal CalculateTotal()
        {
            if (!RentalCalculator.TryParseCurrency(rateTextBox.Text, out decimal dailyRate))
            {
                totalTextBox.Text = RentalCalculator.FormatCurrency(0);
                minimumDepositTextBox.Text = RentalCalculator.FormatCurrency(0);
                CalculateBalance();
                return 0;
            }

            decimal total = RentalCalculator.CalculateBaseAmount(
                rentalDatePicker.Value,
                returnDatePicker.Value,
                dailyRate);

            totalTextBox.Text = RentalCalculator.FormatCurrency(total);
            minimumDepositTextBox.Text = RentalCalculator.FormatCurrency(PaymentHelper.GetMinimumDepositAmount(total));
            CalculateBalance();
            return total;
        }

        private decimal CalculateBalance()
        {
            decimal total = CalculateDisplayedTotalAmount();
            decimal amountPaid = ParseOptionalPaymentAmount(paymentAmountTextBox.Text);
            if (amountPaid < 0)
            {
                amountPaid = 0;
            }

            decimal balance = PaymentHelper.GetBalance(total, amountPaid);
            balanceTextBox.Text = RentalCalculator.FormatCurrency(balance);
            paymentAmountTextBox.PlaceholderText = "0";
            return balance;
        }

        private decimal CalculateDisplayedTotalAmount()
        {
            if (!RentalCalculator.TryParseCurrency(rateTextBox.Text, out decimal dailyRate))
            {
                return 0;
            }

            return RentalCalculator.CalculateBaseAmount(
                rentalDatePicker.Value,
                returnDatePicker.Value,
                dailyRate);
        }

        private static decimal ParseOptionalPaymentAmount(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? 0
                : RentalCalculator.TryParseCurrency(value, out decimal amount) ? amount : -1;
        }

        private void ClearForm()
        {
            matchedCustomerName = "";
            customerNameTextBox.Clear();
            if (carComboBox.Items.Count > 0) carComboBox.SelectedIndex = 0;

            contactTextBox.Clear();
            licenseTextBox.Clear();
            addressTextBox.Clear();
            SetLicenseFieldLocked(false);

            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);
            paymentAmountTextBox.Clear();
            paymentReferenceTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            UpdatePaymentReferenceVisibility();
            UpdateCreateButtonText();

            rentalsGrid.ClearSelection();
            CalculateTotal();
        }

        private void UpdateCreateButtonText()
        {
            createButton.Text = rentalDatePicker.Value.Date == DateTime.Today
                ? "Start Rental"
                : "Save Reservation";
        }

        private void UpdatePaymentReferenceVisibility()
        {
            PaymentMethod paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
                ? selectedMethod
                : PaymentMethod.Cash;
            bool requiresReference = PaymentHelper.RequiresReferenceNumber(paymentMethod);

            paymentReferenceLabel.Visible = true;
            paymentReferenceTextBox.Visible = true;
            paymentReferenceTextBox.Enabled = true;
            paymentReferenceTextBox.ReadOnly = !requiresReference;
            paymentReferenceTextBox.FillColor = requiresReference
                ? Color.White
                : Color.FromArgb(243, 244, 246);
            paymentReferenceTextBox.PlaceholderText = requiresReference
                ? "Receipt or transaction reference"
                : "Not needed for cash";

            if (!requiresReference)
            {
                paymentReferenceTextBox.Clear();
            }
        }

        private void SetLicenseFieldLocked(bool isLocked)
        {
            licenseTextBox.ReadOnly = isLocked;
            licenseTextBox.FillColor = isLocked
                ? Color.FromArgb(243, 244, 246)
                : Color.White;
            licenseTextBox.FocusedState.BorderColor = isLocked
                ? Color.FromArgb(209, 213, 219)
                : Color.FromArgb(37, 99, 235);
            licenseTextBox.HoverState.BorderColor = isLocked
                ? Color.FromArgb(209, 213, 219)
                : Color.FromArgb(37, 99, 235);
            licenseTextBox.PlaceholderText = isLocked
                ? "Saved in customer record"
                : "Driver's license number";
        }
    }
}

