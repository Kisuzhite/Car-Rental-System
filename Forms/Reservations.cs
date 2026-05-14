using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.Services;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;
using Guna.UI2.WinForms;

namespace Car_Rental_System
{
    public partial class Reservations : Form, IReservationsView
    {
        private readonly ReservationsPresenter presenter;
        private int? selectedReservationId;
        private bool isLoadingReservations;

        public Reservations()
        {
            InitializeComponent();
            presenter = new ReservationsPresenter(this);
        }

        public string SearchText => searchTextBox.Text;

        void IReservationsView.BindAvailableCars(List<ReservationCarLookupItem> cars)
        {
            carComboBox.DataSource = cars;
            carComboBox.DisplayMember = "CarName";
            carComboBox.ValueMember = "CarId";

            if (cars.Count == 0)
            {
                rateTextBox.Clear();
                estimatedTotalTextBox.Text = RentalCalculator.FormatCurrency(0);
            }

            CarComboBox_SelectedIndexChanged(this, EventArgs.Empty);
        }

        void IReservationsView.BindReservations(List<ReservationGridRow> reservations)
        {
            reservationsGrid.DataSource = reservations;

            DataGridViewHelper.ApplyFriendlyHeaders(reservationsGrid);
            DataGridViewHelper.HideColumn(reservationsGrid, "ReservationId");
            DataGridViewHelper.HideColumn(reservationsGrid, "CarId");
            DataGridViewHelper.ApplyStatusFormatting(reservationsGrid);
            FormatReservationsGridColumns();
            ClearFormAfterGridLoad();
        }

        private async void Reservations_Load(object? sender, EventArgs e)
        {
            PrepareForm();
            await LoadLookupsAsync();
            await LoadReservationsAsync();
        }

        private void PrepareForm()
        {
            pickupDatePicker.Value = DateTime.Today.AddDays(1);
            returnDatePicker.Value = DateTime.Today.AddDays(2);

            paymentMethodComboBox.Format += PaymentMethodComboBox_Format;
            paymentMethodComboBox.DropDownWidth = 190;
            paymentMethodComboBox.DataSource = PaymentHelper.GetSelectablePaymentMethods();
            paymentMethodComboBox.SelectedItem = PaymentMethod.GCash;

            CalculateEstimatedTotal();
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

        private async void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out ReservationFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.AddReservationAsync(input);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    await LoadReservationsAsync();
                }
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add reservation.", ex);
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

                    await LoadLookupsAsync();
                    await LoadReservationsAsync();
                    return;
                }

                MessageBox.Show(result.Message, result.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RentalDocumentPrinter.PromptToPrintRentalAgreementAsync(this, result.RentalId);
                await LoadLookupsAsync();
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

        private void CarComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (carComboBox.SelectedItem is ReservationCarLookupItem selected)
            {
                rateTextBox.Text = selected.DailyRate.ToString("0.##");
            }
            else
            {
                rateTextBox.Clear();
            }

            CalculateEstimatedTotal();
        }

        private void DateOrRate_Changed(object? sender, EventArgs e)
        {
            CalculateEstimatedTotal();
        }

        private void DepositTextBox_TextChanged(object? sender, EventArgs e)
        {
            CalculateEstimatedTotal();
        }

        private void ReservationsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (isLoadingReservations)
            {
                return;
            }

            if (reservationsGrid.CurrentRow?.DataBoundItem is not ReservationGridRow row ||
                !reservationsGrid.CurrentRow.Selected)
            {
                selectedReservationId = null;
                return;
            }

            selectedReservationId = row.ReservationId;
            customerNameTextBox.Text = row.CustomerName;
            contactTextBox.Text = row.ContactNumber;
            pickupDatePicker.Value = row.PickupDate.ToDateTime(TimeOnly.MinValue);
            returnDatePicker.Value = row.ExpectedReturnDate.ToDateTime(TimeOnly.MinValue);
            depositTextBox.Text = row.DepositAmount.ToString("0.##");
            paymentReferenceTextBox.Text = row.PaymentReferenceNumber == AppConstants.NotProvided ? "" : row.PaymentReferenceNumber;
            SelectCar(row.CarId);
            UpdateActionButtons(row.Status);
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

            paymentReferenceNumber = string.IsNullOrWhiteSpace(paymentReferenceNumber)
                ? AppConstants.NotProvided
                : paymentReferenceNumber;

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
            carId = 0;
            return carComboBox.SelectedValue != null &&
                int.TryParse(carComboBox.SelectedValue.ToString(), out carId);
        }

        private decimal CalculateEstimatedTotal()
        {
            if (!decimal.TryParse(rateTextBox.Text, out decimal dailyRate))
            {
                estimatedTotalTextBox.Text = RentalCalculator.FormatCurrency(0);
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
            balanceTextBox.Text = RentalCalculator.FormatCurrency(PaymentHelper.GetBalance(total, deposit));
            return total;
        }

        private static decimal ParseOptionalPaymentAmount(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? 0
                : decimal.TryParse(value, out decimal amount) ? amount : -1;
        }

        private bool SelectCar(int carId)
        {
            foreach (object item in carComboBox.Items)
            {
                if (item is ReservationCarLookupItem car && car.CarId == carId)
                {
                    carComboBox.SelectedItem = item;
                    return true;
                }
            }

            carComboBox.SelectedIndex = -1;
            rateTextBox.Clear();
            CalculateEstimatedTotal();
            return false;
        }

        private void FormatReservationsGridColumns()
        {
            DataGridViewColumn? depositColumn = reservationsGrid.Columns["DepositAmount"];
            if (depositColumn != null)
            {
                depositColumn.DefaultCellStyle.Format = "N2";
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
            customerNameTextBox.Clear();
            contactTextBox.Clear();
            pickupDatePicker.Value = DateTime.Today.AddDays(1);
            returnDatePicker.Value = DateTime.Today.AddDays(2);
            depositTextBox.Clear();
            paymentReferenceTextBox.Clear();
            paymentMethodComboBox.SelectedItem = PaymentMethod.GCash;
            if (carComboBox.Items.Count > 0)
            {
                carComboBox.SelectedIndex = 0;
            }

            reservationsGrid.ClearSelection();
            reservationsGrid.CurrentCell = null;
            UpdateActionButtons(null);
            CalculateEstimatedTotal();
        }

        private void UpdateActionButtons(ReservationStatus? status)
        {
            SetActionButtonStyle(updateButton);
            SetActionButtonStyle(cancelReservationButton);
            SetActionButtonStyle(convertButton);
        }

        private static void SetActionButtonStyle(Guna2Button button)
        {
            button.Enabled = true;
            button.ForeColor = Color.White;
        }
    }
}

