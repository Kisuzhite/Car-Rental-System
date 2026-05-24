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
        private readonly UiDebouncer searchDebouncer = new();
        private int? selectedRentalId;
        private int? selectedCarId;
        private DateOnly _selectedExpectedReturn;
        private decimal _selectedDailyRate;
        private decimal _selectedBaseAmount;
        private decimal _selectedAmountPaid;
        private DateOnly _selectedRentalDate;
        private readonly int? initialRentalId;

        public ReturnCar() : this(null)
        {
        }

        public ReturnCar(int? rentalIdToSelect)
        {
            InitializeComponent();
            initialRentalId = rentalIdToSelect;
            presenter = new ReturnCarPresenter(this);
            FormClosed += (_, _) => searchDebouncer.Dispose();
        }

        public string SearchText => searchTextBox.Text;

        void IReturnCarView.BindActiveRentals(List<ActiveRentalGridRow> rentals)
        {
            activeRentalsGrid.DataSource = rentals;

            DataGridViewHelper.ApplyFriendlyHeaders(activeRentalsGrid);
            DataGridViewHelper.HideColumn(activeRentalsGrid, "RentalId");
            DataGridViewHelper.HideColumn(activeRentalsGrid, "CarId");
            DataGridViewHelper.ApplyStatusFormatting(activeRentalsGrid);
            DataGridViewHelper.ApplyFullTextColumn(activeRentalsGrid, "Car", 340);
            DataGridViewHelper.ApplyEmptyState(activeRentalsGrid, "No active rentals to return.");
            activeRentalsGrid.ClearSelection();
            ClearDetails();

            if (initialRentalId.HasValue && TrySelectRental(initialRentalId.Value))
            {
                return;
            }

            DataGridViewHelper.ClearSelectionWhenReady(activeRentalsGrid);
        }

        private async void ReturnCar_Load(object? sender, EventArgs e)
        {
            paymentMethodComboBox.Format += PaymentMethodComboBox_Format;
            paymentMethodComboBox.SelectedIndexChanged += PaymentMethodComboBox_SelectedIndexChanged;
            paymentMethodComboBox.DataSource = PaymentHelper.GetSelectablePaymentMethods();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            UpdateReturnPaymentFields();
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
            dailyRateTextBox.Text = RentalCalculator.FormatCurrency(_selectedDailyRate);
            baseAmountTextBox.Text = RentalCalculator.FormatCurrency(_selectedBaseAmount);
            amountPaidTextBox.Text = RentalCalculator.FormatCurrency(_selectedAmountPaid);
            actualReturnDatePicker.Value = DateTime.Today;
            additionalPaymentTextBox.Clear();
            paymentReferenceTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            UpdateReturnPaymentFields();

            CalculateFinalAmount();
        }

        private void ActualReturnDatePicker_ValueChanged(object? sender, EventArgs e)
        {
            if (selectedRentalId != null)
            {
                CalculateFinalAmount();
                UpdateReturnPaymentFields();
            }
        }

        private void PaymentMethodComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateReturnPaymentFields();
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
            penaltyTextBox.Text = RentalCalculator.FormatCurrency(penalty);
            finalAmountTextBox.Text = RentalCalculator.FormatCurrency(finalAmount);

            decimal additionalPayment = ParseOptionalPaymentAmount(additionalPaymentTextBox.Text);
            if (additionalPayment < 0)
            {
                additionalPayment = 0;
            }

            decimal balance = PaymentHelper.GetBalance(finalAmount, _selectedAmountPaid + additionalPayment);
            balanceTextBox.Text = RentalCalculator.FormatCurrency(balance);
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

            using var busy = UiBusyScope.Disable(processReturnButton);

            PaymentMethod paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
                ? selectedMethod
                : PaymentMethod.Cash;
            string paymentReferenceNumber = ValidationHelper.CollapseSpaces(paymentReferenceTextBox.Text);

            if (additionalPayment <= 0)
            {
                paymentMethod = PaymentMethod.Cash;
                paymentReferenceNumber = AppConstants.NotProvided;
            }
            else
            {
                bool requiresReference = PaymentHelper.RequiresReferenceNumber(paymentMethod);
                if (requiresReference && string.IsNullOrWhiteSpace(paymentReferenceNumber))
                {
                    MessageBox.Show(
                        $"Please enter the payment reference for {PaymentHelper.FormatPaymentMethod(paymentMethod)}.",
                        "Missing Payment Reference",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                paymentReferenceNumber = requiresReference
                    ? paymentReferenceNumber
                    : AppConstants.NotProvided;
            }

            if (paymentReferenceNumber.Length > 80)
            {
                MessageBox.Show("Payment reference number cannot be longer than 80 characters.", "Invalid Reference", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                RentalOperationResult result = await presenter.ProcessReturnAsync(new ReturnFormInput(
                    selectedRentalId.Value,
                    actualReturnDatePicker.Value,
                    additionalPayment,
                    paymentMethod,
                    paymentReferenceNumber));

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

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            searchDebouncer.Run(LoadActiveRentalsAsync);
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
            paymentReferenceTextBox.Clear();
            balanceTextBox.Clear();
            lateDaysTextBox.Clear();
            penaltyTextBox.Clear();
            finalAmountTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            UpdateReturnPaymentFields();
        }

        private static decimal ParseOptionalPaymentAmount(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? 0
                : RentalCalculator.TryParseCurrency(value, out decimal amount) ? amount : -1;
        }

        private bool TrySelectRental(int rentalId)
        {
            foreach (DataGridViewRow row in activeRentalsGrid.Rows)
            {
                if (row.DataBoundItem is ActiveRentalGridRow rental &&
                    rental.RentalId == rentalId)
                {
                    row.Selected = true;
                    activeRentalsGrid.CurrentCell = row.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                    return true;
                }
            }

            return false;
        }

        private void UpdateReturnPaymentFields()
        {
            decimal additionalPayment = ParseOptionalPaymentAmount(additionalPaymentTextBox.Text);
            bool hasPaymentNow = additionalPayment > 0;
            PaymentMethod paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
                ? selectedMethod
                : PaymentMethod.Cash;
            bool requiresReference = hasPaymentNow && PaymentHelper.RequiresReferenceNumber(paymentMethod);

            paymentMethodLabel.Enabled = hasPaymentNow;
            paymentMethodComboBox.Enabled = hasPaymentNow;
            paymentReferenceLabel.Visible = true;
            paymentReferenceTextBox.Visible = true;
            paymentReferenceTextBox.Enabled = true;
            paymentReferenceTextBox.ReadOnly = !requiresReference;
            paymentReferenceTextBox.FillColor = requiresReference
                ? Color.White
                : Color.FromArgb(243, 244, 246);
            paymentReferenceTextBox.PlaceholderText = requiresReference
                ? "Receipt or transaction reference"
                : hasPaymentNow ? "Not needed for cash" : "No payment now";

            if (!hasPaymentNow && paymentMethodComboBox.SelectedItem is not PaymentMethod.Cash)
            {
                paymentMethodComboBox.SelectedItem = PaymentMethod.Cash;
            }

            if (!requiresReference)
            {
                paymentReferenceTextBox.Clear();
            }
        }
    }
}

