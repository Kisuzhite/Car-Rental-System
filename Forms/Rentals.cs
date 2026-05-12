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

        public Rentals()
        {
            InitializeComponent();
            presenter = new RentalsPresenter(this);
        }

        public string SearchText => searchTextBox.Text;

        void IRentalsView.BindCustomers(List<CustomerLookupItem> customers)
        {
            customerComboBox.DataSource = customers;
            customerComboBox.DisplayMember = "FullName";
            customerComboBox.ValueMember = "CustomerId";
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
            rentalsGrid.Columns.Clear();
            rentalsGrid.DataSource = rentals;

            DataGridViewHelper.ApplyFriendlyHeaders(rentalsGrid);
            DataGridViewHelper.HideColumn(rentalsGrid, "RentalId");
            DataGridViewHelper.ApplyStatusFormatting(rentalsGrid);
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
            paymentMethodComboBox.DataSource = PaymentHelper.GetSelectablePaymentMethods();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            returnDatePicker.Value = DateTime.Today.AddDays(1);
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
                await presenter.LoadLookupsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
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
            if (!ValidateInputs(out RentalFormInput input))
            {
                return;
            }

            try
            {
                RentalOperationResult result = await presenter.CreateRentalAsync(input);

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
                await RentalDocumentPrinter.PromptToPrintRentalAgreementAsync(this, result.RentalId);
                ClearForm();
                await LoadLookupsAsync();
                await LoadRentalsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to create rental.", ex);
            }
        }

        private async void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            await LoadRentalsAsync();
        }

        private void CarComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (carComboBox.SelectedItem is RentalCarLookupItem selected)
            {
                rateTextBox.Text = selected.DailyRate.ToString("0.##");
            }
            else
            {
                rateTextBox.Clear();
            }

            CalculateTotal();
        }

        private void DateOrRate_Changed(object? sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void PaymentAmountTextBox_TextChanged(object? sender, EventArgs e)
        {
            CalculateBalance();
        }

        private bool ValidateInputs(out RentalFormInput input)
        {
            input = new RentalFormInput(0, 0, rentalDatePicker.Value, returnDatePicker.Value, 0, PaymentMethod.Cash);

            if (!TryGetSelectedValue(customerComboBox, out int customerId) ||
                !TryGetSelectedValue(carComboBox, out int carId))
            {
                MessageBox.Show("Please select a customer and an available car.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.TryValidateRentalDates(rentalDatePicker.Value, returnDatePicker.Value, out string dateError))
            {
                MessageBox.Show(dateError, "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(rateTextBox.Text, out decimal dailyRate))
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

            if (!PaymentHelper.IsValidPaymentAmount(amountPaid, totalAmount))
            {
                MessageBox.Show("Amount paid cannot be negative or greater than the total amount.", "Invalid Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            PaymentMethod paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
                ? selectedMethod
                : PaymentMethod.Cash;

            input = new RentalFormInput(customerId, carId, rentalDatePicker.Value, returnDatePicker.Value, amountPaid, paymentMethod);
            return true;
        }

        private static MessageBoxIcon GetRentalFailureIcon(RentalOperationResult result)
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
            if (!decimal.TryParse(rateTextBox.Text, out decimal dailyRate))
            {
                totalTextBox.Text = RentalCalculator.FormatCurrency(0);
                CalculateBalance();
                return 0;
            }

            decimal total = RentalCalculator.CalculateBaseAmount(
                rentalDatePicker.Value,
                returnDatePicker.Value,
                dailyRate);

            totalTextBox.Text = RentalCalculator.FormatCurrency(total);
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
            return balance;
        }

        private decimal CalculateDisplayedTotalAmount()
        {
            if (!decimal.TryParse(rateTextBox.Text, out decimal dailyRate))
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
                : decimal.TryParse(value, out decimal amount) ? amount : -1;
        }

        private void ClearForm()
        {
            if (customerComboBox.Items.Count > 0) customerComboBox.SelectedIndex = 0;
            if (carComboBox.Items.Count > 0) carComboBox.SelectedIndex = 0;

            rentalDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);
            paymentAmountTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;

            rentalsGrid.ClearSelection();
            CalculateTotal();
        }
    }
}
