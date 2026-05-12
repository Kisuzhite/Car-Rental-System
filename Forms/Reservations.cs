using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Services;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class Reservations : Form
    {
        private int? selectedReservationId;
        private bool isLoadingReservations;

        public Reservations()
        {
            InitializeComponent();
        }

        private void Reservations_Load(object? sender, EventArgs e)
        {
            PrepareForm();
            LoadLookups();
            LoadReservations();
        }

        private void PrepareForm()
        {
            pickupDatePicker.Value = DateTime.Today.AddDays(1);
            returnDatePicker.Value = DateTime.Today.AddDays(2);

            paymentMethodComboBox.Format += PaymentMethodComboBox_Format;
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

        private void LoadLookups()
        {
            try
            {
                using var db = new AppDbContext();
                var cars = db.Cars
                    .Where(c => c.Status == CarStatus.Available)
                    .OrderBy(c => c.Brand)
                    .ThenBy(c => c.Model)
                    .Select(c => new ReservationCarLookupItem
                    {
                        CarId = c.CarId,
                        CarName = c.PlateNumber + " - " + c.Brand + " " + c.Model +
                            (c.CarType != null ? " (" + c.CarType.TypeName + ")" : ""),
                        DailyRate = c.CarType != null ? c.CarType.DailyRate : c.DailyRate
                    })
                    .ToList();

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
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadReservations(string search = "")
        {
            isLoadingReservations = true;

            try
            {
                using var db = new AppDbContext();
                string trimmed = search.Trim();

                var query = db.Reservations
                    .Include(r => r.Car).ThenInclude(c => c.CarType)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    query = query.Where(r =>
                        r.CustomerName.Contains(trimmed) ||
                        r.ContactNumber.Contains(trimmed) ||
                        r.PaymentReferenceNumber.Contains(trimmed) ||
                        r.Car.PlateNumber.Contains(trimmed) ||
                        r.Car.Brand.Contains(trimmed) ||
                        r.Car.Model.Contains(trimmed) ||
                        (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(trimmed)));
                }

                var reservations = query
                    .OrderBy(r => r.Status == ReservationStatus.Converted)
                    .ThenBy(r => r.Status == ReservationStatus.Cancelled)
                    .ThenBy(r => r.PickupDate)
                    .ToList()
                    .Select(r => new ReservationGridRow
                    {
                        ReservationId = r.ReservationId,
                        CarId = r.CarId,
                        CustomerName = r.CustomerName,
                        ContactNumber = r.ContactNumber,
                        Car = r.Car.PlateNumber + " - " + r.Car.Brand + " " + r.Car.Model +
                            (r.Car.CarType != null ? " (" + r.Car.CarType.TypeName + ")" : ""),
                        ReservationDate = r.ReservationDate,
                        PickupDate = r.PickupDate,
                        ExpectedReturnDate = r.ExpectedReturnDate,
                        DepositAmount = r.DepositAmount,
                        PaymentMethod = PaymentHelper.FormatPaymentMethod(r.PaymentMethod),
                        PaymentReferenceNumber = r.PaymentReferenceNumber,
                        Status = r.Status
                    })
                    .ToList();

                reservationsGrid.Columns.Clear();
                reservationsGrid.DataSource = reservations;

                DataGridViewHelper.ApplyFriendlyHeaders(reservationsGrid);
                DataGridViewHelper.HideColumn(reservationsGrid, "ReservationId");
                DataGridViewHelper.HideColumn(reservationsGrid, "CarId");
                DataGridViewHelper.ApplyStatusFormatting(reservationsGrid);
                FormatReservationsGridColumns();
                ClearFormAfterGridLoad();
            }
            catch (Exception ex)
            {
                isLoadingReservations = false;
                AppMessageBox.ShowDatabaseError("Unable to load reservations.", ex);
            }
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(
                    out int carId,
                    out string customerName,
                    out string contactNumber,
                    out DateOnly pickupDate,
                    out DateOnly expectedReturnDate,
                    out decimal depositAmount,
                    out PaymentMethod paymentMethod,
                    out string paymentReferenceNumber))
            {
                return;
            }

            try
            {
                using var db = new AppDbContext();

                if (ReservationService.HasCarScheduleConflict(db, carId, pickupDate, expectedReturnDate))
                {
                    ShowScheduleConflictMessage();
                    return;
                }

                db.Reservations.Add(new Reservation
                {
                    CarId = carId,
                    CustomerName = customerName,
                    ContactNumber = contactNumber,
                    ReservationDate = RentalCalculator.ToDateOnly(DateTime.Today),
                    PickupDate = pickupDate,
                    ExpectedReturnDate = expectedReturnDate,
                    DepositAmount = depositAmount,
                    PaymentMethod = paymentMethod,
                    PaymentReferenceNumber = paymentReferenceNumber,
                    Source = "Facebook",
                    Status = ReservationStatus.Confirmed
                });

                db.SaveChanges();

                MessageBox.Show("Reservation saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add reservation.", ex);
            }
        }

        private void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedReservationId == null)
            {
                MessageBox.Show("Please select a reservation to update.", "No Reservation Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var db = new AppDbContext();
                var reservation = db.Reservations.Find(selectedReservationId.Value);
                if (reservation == null)
                {
                    MessageBox.Show("Reservation not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadReservations(searchTextBox.Text);
                    return;
                }

                if (reservation.Status == ReservationStatus.Converted)
                {
                    MessageBox.Show("This reservation has already been converted to a rental.", "Already Converted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReservations(searchTextBox.Text);
                    return;
                }

                if (reservation.Status == ReservationStatus.Cancelled)
                {
                    MessageBox.Show("Cancelled reservations cannot be edited. Create a new reservation instead.", "Already Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReservations(searchTextBox.Text);
                    return;
                }

                if (!ValidateInputs(
                        out int carId,
                        out string customerName,
                        out string contactNumber,
                        out DateOnly pickupDate,
                        out DateOnly expectedReturnDate,
                        out decimal depositAmount,
                        out PaymentMethod paymentMethod,
                        out string paymentReferenceNumber))
                {
                    return;
                }

                if (ReservationService.HasCarScheduleConflict(db, carId, pickupDate, expectedReturnDate, selectedReservationId.Value))
                {
                    ShowScheduleConflictMessage();
                    return;
                }

                reservation.CarId = carId;
                reservation.CustomerName = customerName;
                reservation.ContactNumber = contactNumber;
                reservation.PickupDate = pickupDate;
                reservation.ExpectedReturnDate = expectedReturnDate;
                reservation.DepositAmount = depositAmount;
                reservation.PaymentMethod = paymentMethod;
                reservation.PaymentReferenceNumber = paymentReferenceNumber;

                db.SaveChanges();

                MessageBox.Show("Reservation updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update reservation.", ex);
            }
        }

        private void CancelReservationButton_Click(object? sender, EventArgs e)
        {
            if (selectedReservationId == null)
            {
                MessageBox.Show("Please select a reservation to cancel.", "No Reservation Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Cancel this reservation?",
                "Confirm Cancellation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using var db = new AppDbContext();
                var reservation = db.Reservations.Find(selectedReservationId.Value);
                if (reservation == null)
                {
                    MessageBox.Show("Reservation not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadReservations(searchTextBox.Text);
                    return;
                }

                if (reservation.Status == ReservationStatus.Converted)
                {
                    MessageBox.Show("Converted reservations cannot be cancelled.", "Already Converted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                reservation.Status = ReservationStatus.Cancelled;
                db.SaveChanges();

                MessageBox.Show("Reservation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to cancel reservation.", ex);
            }
        }

        private void ConvertButton_Click(object? sender, EventArgs e)
        {
            if (selectedReservationId == null)
            {
                MessageBox.Show("Please select a reservation to convert.", "No Reservation Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Convert this reservation to an active rental now?",
                "Convert to Rental",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var reservationService = new ReservationService();
                ReservationOperationResult operationResult = reservationService.ConvertToRental(selectedReservationId.Value);

                if (!operationResult.Success)
                {
                    MessageBox.Show(
                        operationResult.Message,
                        operationResult.Title,
                        MessageBoxButtons.OK,
                        operationResult.HasScheduleConflict || operationResult.CarWasUnavailable ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    LoadLookups();
                    LoadReservations(searchTextBox.Text);
                    return;
                }

                MessageBox.Show(operationResult.Message, operationResult.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                RentalDocumentPrinter.PromptToPrintRentalAgreement(this, operationResult.RentalId);
                LoadLookups();
                LoadReservations(searchTextBox.Text);
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

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadReservations(searchTextBox.Text);
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

        private bool ValidateInputs(
            out int carId,
            out string customerName,
            out string contactNumber,
            out DateOnly pickupDate,
            out DateOnly expectedReturnDate,
            out decimal depositAmount,
            out PaymentMethod paymentMethod,
            out string paymentReferenceNumber)
        {
            carId = 0;
            customerName = ValidationHelper.CollapseSpaces(customerNameTextBox.Text);
            contactNumber = ValidationHelper.CollapseSpaces(contactTextBox.Text);
            pickupDate = RentalCalculator.ToDateOnly(pickupDatePicker.Value);
            expectedReturnDate = RentalCalculator.ToDateOnly(returnDatePicker.Value);
            depositAmount = 0;
            paymentMethod = PaymentMethod.Cash;
            paymentReferenceNumber = ValidationHelper.CollapseSpaces(paymentReferenceTextBox.Text);

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
            depositAmount = ParseOptionalPaymentAmount(depositTextBox.Text);

            if (!PaymentHelper.IsValidPaymentAmount(depositAmount, estimatedTotal))
            {
                MessageBox.Show("Deposit cannot be negative or greater than the estimated total.", "Invalid Deposit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            paymentMethod = paymentMethodComboBox.SelectedItem is PaymentMethod selectedMethod
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
            bool canChangeReservation = selectedReservationId.HasValue &&
                status is ReservationStatus.Pending or ReservationStatus.Confirmed;

            SetActionButtonState(updateButton, canChangeReservation);
            SetActionButtonState(cancelReservationButton, canChangeReservation);
            SetActionButtonState(convertButton, canChangeReservation);
        }

        private static void SetActionButtonState(Button button, bool enabled)
        {
            button.Enabled = enabled;
            button.ForeColor = enabled
                ? Color.White
                : Color.FromArgb(107, 114, 128);
        }

        private static void ShowScheduleConflictMessage()
        {
            MessageBox.Show(
                "This car already has an active rental or reservation for those dates.",
                "Schedule Conflict",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private sealed class ReservationCarLookupItem
        {
            public int CarId { get; set; }
            public string CarName { get; set; } = "";
            public decimal DailyRate { get; set; }
        }

        private sealed class ReservationGridRow
        {
            public int ReservationId { get; set; }
            public int CarId { get; set; }
            public string CustomerName { get; set; } = "";
            public string ContactNumber { get; set; } = "";
            public string Car { get; set; } = "";
            public DateOnly ReservationDate { get; set; }
            public DateOnly PickupDate { get; set; }
            public DateOnly ExpectedReturnDate { get; set; }
            public decimal DepositAmount { get; set; }
            public string PaymentMethod { get; set; } = "";
            public string PaymentReferenceNumber { get; set; } = "";
            public ReservationStatus Status { get; set; }
        }
    }
}
