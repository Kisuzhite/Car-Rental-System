using System.Globalization;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System
{
    public partial class ReturnCar : Form, IReturnCarView
    {
        private readonly ReturnCarPresenter presenter;
        private int? selectedRentalId;
        private int? selectedCarId;
        private DateOnly _selectedExpectedReturn;
        private decimal _selectedDailyRate;
        private decimal _selectedBaseAmount;
        private decimal _selectedAmountPaid;
        private DateOnly _selectedRentalDate;

        public ReturnCar()
        {
            InitializeComponent();
            presenter = new ReturnCarPresenter(this);
        }

        public string SearchText => searchTextBox.Text;

        void IReturnCarView.BindActiveRentals(List<ActiveRentalGridRow> rentals)
        {
            activeRentalsGrid.DataSource = rentals;

            DataGridViewHelper.ApplyFriendlyHeaders(activeRentalsGrid);
            DataGridViewHelper.HideColumn(activeRentalsGrid, "RentalId");
            DataGridViewHelper.HideColumn(activeRentalsGrid, "CarId");
            DataGridViewHelper.ApplyStatusFormatting(activeRentalsGrid);
            activeRentalsGrid.ClearSelection();
            ClearDetails();
            DataGridViewHelper.ClearSelectionWhenReady(activeRentalsGrid);
        }

        private async void ReturnCar_Load(object? sender, EventArgs e)
        {
            paymentMethodComboBox.Format += PaymentMethodComboBox_Format;
            paymentMethodComboBox.DropDownWidth = 190;
            paymentMethodComboBox.DataSource = PaymentHelper.GetSelectablePaymentMethods();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            actualReturnDatePicker.Value = DateTime.Today;
            await LoadActiveRentalsAsync();
        }

        private static void PaymentMethodComboBox_Format(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is PaymentMethod paymentMethod)
            {
                e.Value = PaymentHelper.FormatPaymentMethod(paymentMethod);
            }
        }

        private async Task LoadActiveRentalsAsync()
        {
            try
            {
                await presenter.LoadActiveRentalsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void ActiveRentalsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (activeRentalsGrid.CurrentRow?.DataBoundItem is not ActiveRentalGridRow row ||
                !activeRentalsGrid.CurrentRow.Selected)
            {
                ClearDetails();
                return;
            }

            selectedRentalId = row.RentalId;
            selectedCarId = row.CarId;

            _selectedRentalDate = row.RentalDate;
            _selectedExpectedReturn = row.ExpectedReturn;
            _selectedDailyRate = row.DailyRate;
            _selectedBaseAmount = row.BaseAmount;
            _selectedAmountPaid = row.AmountPaid;

            customerTextBox.Text = row.Customer;
            carTextBox.Text = row.Car;
            expectedReturnTextBox.Text = _selectedExpectedReturn.ToString("d", CultureInfo.CurrentCulture);
            dailyRateTextBox.Text = _selectedDailyRate.ToString("N2");
            baseAmountTextBox.Text = _selectedBaseAmount.ToString("N2");
            amountPaidTextBox.Text = _selectedAmountPaid.ToString("N2");
            additionalPaymentTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;

            CalculateFinalAmount();
        }

        private void ActualReturnDatePicker_ValueChanged(object? sender, EventArgs e)
        {
            if (selectedRentalId != null)
            {
                CalculateFinalAmount();
            }
        }

        private void AdditionalPaymentTextBox_TextChanged(object? sender, EventArgs e)
        {
            if (selectedRentalId != null)
            {
                CalculateFinalAmount();
            }
        }

        private decimal CalculateFinalAmount()
        {
            int lateDays = RentalCalculator.GetLateDays(_selectedExpectedReturn, actualReturnDatePicker.Value);
            decimal penalty = lateDays * _selectedDailyRate;
            decimal finalAmount = RentalCalculator.CalculateFinalAmount(
                _selectedExpectedReturn,
                actualReturnDatePicker.Value,
                _selectedDailyRate,
                _selectedBaseAmount);

            lateDaysTextBox.Text = lateDays.ToString();
            penaltyTextBox.Text = penalty.ToString("N2");
            finalAmountTextBox.Text = finalAmount.ToString("N2");

            decimal additionalPayment = ParseOptionalPaymentAmount(additionalPaymentTextBox.Text);
            if (additionalPayment < 0)
            {
                additionalPayment = 0;
            }

            decimal balance = PaymentHelper.GetBalance(finalAmount, _selectedAmountPaid + additionalPayment);
            balanceTextBox.Text = balance.ToString("N2");
            return finalAmount;
        }

        private async void ProcessReturnButton_Click(object? sender, EventArgs e)
        {
            if (selectedRentalId == null || selectedCarId == null)
            {
                MessageBox.Show("Please select an active rental to return.", "No Rental Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateOnly actualReturnDate = RentalCalculator.ToDateOnly(actualReturnDatePicker.Value);

            if (actualReturnDate < _selectedRentalDate)
            {
                MessageBox.Show("Actual return date cannot be earlier than the rental date.", "Invalid Return Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal additionalPayment = ParseOptionalPaymentAmount(additionalPaymentTextBox.Text);
            if (additionalPayment < 0)
            {
                MessageBox.Show("Payment now must be a valid amount.", "Invalid Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal finalAmount = CalculateFinalAmount();
            decimal newAmountPaid = _selectedAmountPaid + additionalPayment;
            if (!PaymentHelper.IsValidPaymentAmount(newAmountPaid, finalAmount))
            {
                MessageBox.Show("Payment now cannot make the total paid greater than the final amount.", "Invalid Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal remainingBalance = PaymentHelper.GetBalance(finalAmount, newAmountPaid);
            if (remainingBalance > 0 && !ConfirmReturnWithRemainingBalance(remainingBalance))
            {
                return;
            }

            PaymentMethod paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
                ? selectedMethod
                : PaymentMethod.Cash;

            try
            {
                RentalOperationResult result = await presenter.ProcessReturnAsync(new ReturnFormInput(
                    selectedRentalId.Value,
                    actualReturnDatePicker.Value,
                    additionalPayment,
                    paymentMethod));

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        result.Title,
                        MessageBoxButtons.OK,
                        result.RentalWasAlreadyClosed ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    if (result.RentalWasAlreadyClosed)
                    {
                        await LoadActiveRentalsAsync();
                    }
                    return;
                }

                MessageBox.Show(result.Message, result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RentalDocumentPrinter.PromptToPrintFinalReceiptAsync(this, result.RentalId);
                await LoadActiveRentalsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to process return.", ex);
            }
        }

        private static bool ConfirmReturnWithRemainingBalance(decimal remainingBalance)
        {
            DialogResult result = MessageBox.Show(
                $"This rental will still have a balance of {RentalCalculator.FormatCurrency(remainingBalance)} after processing this return.\n\nThe car will be marked available, but the payment status will stay unpaid or partial.\n\nProcess return anyway?",
                "Balance Still Unpaid",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            return result == DialogResult.Yes;
        }

        private async void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            await LoadActiveRentalsAsync();
        }

        private void ClearDetails()
        {
            selectedRentalId = null;
            selectedCarId = null;
            _selectedRentalDate = default;
            _selectedExpectedReturn = default;
            _selectedDailyRate = 0;
            _selectedBaseAmount = 0;
            _selectedAmountPaid = 0;
            customerTextBox.Clear();
            carTextBox.Clear();
            expectedReturnTextBox.Clear();
            dailyRateTextBox.Clear();
            baseAmountTextBox.Clear();
            amountPaidTextBox.Clear();
            additionalPaymentTextBox.Clear();
            balanceTextBox.Clear();
            lateDaysTextBox.Clear();
            penaltyTextBox.Clear();
            finalAmountTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
        }

        private static decimal ParseOptionalPaymentAmount(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? 0
                : decimal.TryParse(value, out decimal amount) ? amount : -1;
        }
    }
}

