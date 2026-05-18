using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;
namespace Car_Rental_System
{
    public partial class Reservations : Form, IReservationsView
    {
        private readonly ReservationsPresenter presenter;
        private int? selectedReservationId;
        private int? selectedReservationCarId;
        private bool isLoadingReservations;

        public Reservations()
        {
            InitializeComponent();
            presenter = new ReservationsPresenter(this);
        }

        public string SearchText => searchTextBox.Text;

        void IReservationsView.BindAvailableCars(List<ReservationCarLookupItem> cars)
        {
            // Reservations show the already-reserved car in a read-only textbox.
        }

        void IReservationsView.BindReservations(List<ReservationGridRow> reservations)
        {
            reservationsGrid.DataSource = reservations;

            DataGridViewHelper.ApplyFriendlyHeaders(reservationsGrid);
            DataGridViewHelper.HideColumn(reservationsGrid, "ReservationId");
            DataGridViewHelper.HideColumn(reservationsGrid, "CarId");
            DataGridViewHelper.HideColumn(reservationsGrid, "DailyRate");
            DataGridViewHelper.HideColumn(reservationsGrid, "PaymentMethodValue");
            DataGridViewHelper.ApplyStatusFormatting(reservationsGrid);
            DataGridViewHelper.ApplyFullTextColumn(reservationsGrid, "Car", 340);
            FormatReservationsGridColumns();
            ClearFormAfterGridLoad();
        }

        private async void Reservations_Load(object? sender, EventArgs e)
        {
            PrepareForm();
            await LoadReservationsAsync();
        }

        private void PrepareForm()
        {
            pickupDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);

            paymentMethodComboBox.Format += PaymentMethodComboBox_Format;
            paymentMethodComboBox.SelectedIndexChanged += PaymentMethodComboBox_SelectedIndexChanged;
            paymentMethodComboBox.DataSource = PaymentHelper.GetSelectablePaymentMethods();
            paymentMethodComboBox.SelectedItem = PaymentMethod.GCash;
            UpdatePaymentReferenceVisibility();

            CalculateEstimatedTotal();
        }

        private static void PaymentMethodComboBox_Format(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is PaymentMethod paymentMethod)
            {
                e.Value = PaymentHelper.FormatPaymentMethod(paymentMethod);
            }
        }

        private async Task LoadReservationsAsync()
        {
            isLoadingReservations = true;

            try
            {
                await presenter.LoadReservationsAsync();
            }
            catch (Exception ex)
            {
                isLoadingReservations = false;
                AppMessageBox.ShowDatabaseError("Unable to load reservations.", ex);
            }
        }

        private async void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedReservationId == null)
            {
                MessageBox.Show("Please select a reservation to update.", "No Reservation Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInputs(out ReservationFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.UpdateReservationAsync(selectedReservationId.Value, input);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    await LoadReservationsAsync();
                }
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update reservation.", ex);
            }
        }

        private async void CancelReservationButton_Click(object? sender, EventArgs e)
        {
            if (selectedReservationId == null)
            {
                MessageBox.Show("Please select a reservation to cancel.", "No Reservation Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Cancel this reservation?",
                "Confirm Cancellation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.CancelReservationAsync(selectedReservationId.Value);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    await LoadReservationsAsync();
                }
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to cancel reservation.", ex);
            }
        }

        private async void ConvertButton_Click(object? sender, EventArgs e)
        {
            if (selectedReservationId == null)
            {
                MessageBox.Show("Please select a reservation to convert.", "No Reservation Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Convert this reservation to an active rental now?",
                "Convert to Rental",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            try
            {
                ReservationOperationResult result = await presenter.ConvertToRentalAsync(selectedReservationId.Value);

                if (!result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        result.Title,
                        MessageBoxButtons.OK,
                        result.HasScheduleConflict || result.CarWasUnavailable ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    await LoadReservationsAsync();
                    return;
                }

                MessageBox.Show(result.Message, result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RentalDocumentPrinter.PromptToPrintRentalAgreementAsync(this, result.RentalId);
                await LoadReservationsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to convert reservation.", ex);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private async void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            await LoadReservationsAsync();
        }

        private void DateOrRate_Changed(object? sender, EventArgs e)
        {
            CalculateEstimatedTotal();
        }

        private void DepositTextBox_TextChanged(object? sender, EventArgs e)
        {
            CalculateEstimatedTotal();
        }

        private void PaymentMethodComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdatePaymentReferenceVisibility();
        }

        private async void ReservationsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (isLoadingReservations)
            {
                return;
            }

            if (reservationsGrid.CurrentRow?.DataBoundItem is not ReservationGridRow row ||
                !reservationsGrid.CurrentRow.Selected)
            {
                selectedReservationId = null;
                selectedReservationCarId = null;
                UpdateCarSelectionLock();
                return;
            }

            selectedReservationId = row.ReservationId;
            selectedReservationCarId = row.CarId;

            customerNameTextBox.Text = row.CustomerName;
            contactTextBox.Text = row.ContactNumber;
            pickupDatePicker.Value = row.PickupDate.ToDateTime(TimeOnly.MinValue);
            returnDatePicker.Value = row.ExpectedReturnDate.ToDateTime(TimeOnly.MinValue);
            reservedCarTextBox.Text = row.Car;
            rateTextBox.Text = RentalCalculator.FormatCurrency(row.DailyRate);
            depositTextBox.Text = row.DepositAmount.ToString("0.##");
            paymentMethodComboBox.SelectedItem = row.PaymentMethodValue;
            paymentReferenceTextBox.Text = row.PaymentReferenceNumber == AppConstants.NotProvided ? "" : row.PaymentReferenceNumber;
            UpdatePaymentReferenceVisibility();

            UpdateCarSelectionLock();
        }

        private bool ValidateInputs(out ReservationFormInput input)
        {
            int carId = 0;
            string customerName = ValidationHelper.CollapseSpaces(customerNameTextBox.Text);
            string contactNumber = ValidationHelper.CollapseSpaces(contactTextBox.Text);
            DateOnly pickupDate = RentalCalculator.ToDateOnly(pickupDatePicker.Value);
            DateOnly expectedReturnDate = RentalCalculator.ToDateOnly(returnDatePicker.Value);
            string paymentReferenceNumber = ValidationHelper.CollapseSpaces(paymentReferenceTextBox.Text);
            input = new ReservationFormInput(carId, customerName, contactNumber, pickupDate, expectedReturnDate, 0, PaymentMethod.Cash, paymentReferenceNumber);

            if (!ValidationHelper.IsValidPersonName(customerName))
            {
                MessageBox.Show("Customer name must be 2-100 characters and contain only letters, spaces, apostrophes, periods, or hyphens.", "Invalid Customer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidContactNumber(contactNumber))
            {
                MessageBox.Show("Please enter a valid customer contact number.", "Invalid Contact Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!TryGetSelectedCarId(out carId))
            {
                MessageBox.Show("Please select a car.", "Missing Car", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (pickupDatePicker.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Pickup date cannot be in the past.", "Invalid Pickup Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.TryValidateRentalDates(pickupDatePicker.Value, returnDatePicker.Value, out string dateError))
            {
                MessageBox.Show(dateError, "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            decimal estimatedTotal = CalculateEstimatedTotal();
            decimal depositAmount = ParseOptionalPaymentAmount(depositTextBox.Text);

            if (!PaymentHelper.IsValidInitialPaymentAmount(depositAmount, estimatedTotal))
            {
                decimal minimumDeposit = PaymentHelper.GetMinimumDepositAmount(estimatedTotal);
                MessageBox.Show(
                    $"Deposit must be at least the minimum deposit of {RentalCalculator.FormatCurrency(minimumDeposit)} and cannot be greater than the estimated total.",
                    "Invalid Deposit",
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

            input = new ReservationFormInput(
                carId,
                customerName,
                contactNumber,
                pickupDate,
                expectedReturnDate,
                depositAmount,
                paymentMethod,
                paymentReferenceNumber);
            return true;
        }

        private bool TryGetSelectedCarId(out int carId)
        {
            carId = selectedReservationCarId.GetValueOrDefault();
            return selectedReservationCarId.HasValue;
        }

        private decimal CalculateEstimatedTotal()
        {
            if (!RentalCalculator.TryParseCurrency(rateTextBox.Text, out decimal dailyRate))
            {
                estimatedTotalTextBox.Text = RentalCalculator.FormatCurrency(0);
                minimumDepositTextBox.Text = RentalCalculator.FormatCurrency(0);
                balanceTextBox.Text = RentalCalculator.FormatCurrency(0);
                return 0;
            }

            decimal total = RentalCalculator.CalculateBaseAmount(
                pickupDatePicker.Value,
                returnDatePicker.Value,
                dailyRate);
            decimal deposit = ParseOptionalPaymentAmount(depositTextBox.Text);
            if (deposit < 0)
            {
                deposit = 0;
            }

            estimatedTotalTextBox.Text = RentalCalculator.FormatCurrency(total);
            minimumDepositTextBox.Text = RentalCalculator.FormatCurrency(PaymentHelper.GetMinimumDepositAmount(total));
            balanceTextBox.Text = RentalCalculator.FormatCurrency(PaymentHelper.GetBalance(total, deposit));
            depositTextBox.PlaceholderText = "0";
            return total;
        }

        private static decimal ParseOptionalPaymentAmount(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? 0
                : RentalCalculator.TryParseCurrency(value, out decimal amount) ? amount : -1;
        }

        private void FormatReservationsGridColumns()
        {
            DataGridViewColumn? depositColumn = reservationsGrid.Columns["DepositAmount"];
            if (depositColumn != null)
            {
                depositColumn.DefaultCellStyle.Format = RentalCalculator.CurrencyFormat;
            }

            DataGridViewHelper.MoveColumnToEnd(reservationsGrid, "Status");
        }

        private void ClearFormAfterGridLoad()
        {
            if (!reservationsGrid.IsHandleCreated)
            {
                isLoadingReservations = false;
                ClearForm();
                return;
            }

            BeginInvoke(new Action(() =>
            {
                ClearForm();
                isLoadingReservations = false;
            }));
        }

        private void ClearForm()
        {
            selectedReservationId = null;
            selectedReservationCarId = null;
            customerNameTextBox.Clear();
            contactTextBox.Clear();
            reservedCarTextBox.Clear();
            rateTextBox.Clear();
            pickupDatePicker.Value = DateTime.Today;
            returnDatePicker.Value = DateTime.Today.AddDays(1);
            depositTextBox.Clear();
            paymentReferenceTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.GCash;
            UpdatePaymentReferenceVisibility();
            reservationsGrid.ClearSelection();
            reservationsGrid.CurrentCell = null;
            CalculateEstimatedTotal();
            UpdateCarSelectionLock();
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
                ? "Reference number"
                : "Not needed for cash";

            if (!requiresReference)
            {
                paymentReferenceTextBox.Clear();
            }
        }

        private void UpdateCarSelectionLock()
        {
            bool hasReservationSelected = selectedReservationId.HasValue;
            reservedCarTextBox.ReadOnly = true;
            reservedCarTextBox.FillColor = hasReservationSelected
                ? Color.FromArgb(243, 244, 246)
                : Color.FromArgb(249, 250, 251);
        }
    }
}

