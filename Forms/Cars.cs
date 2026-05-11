using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class Cars : Form
    {
        private int? selectedCarId;

        public Cars()
        {
            InitializeComponent();
        }

        private void Cars_Load(object? sender, EventArgs e)
        {
            PrepareForm();
            LoadCars();
        }

        private void PrepareForm()
        {
            UpdateStatusOptions();
            LoadCarTypes();
        }

        private void LoadCarTypes(int? includeCarTypeId = null)
        {
            try
            {
                using AppDbContext db = new();

                var carTypes = db.CarTypes
                    .Where(ct => ct.IsActive || (includeCarTypeId.HasValue && ct.CarTypeId == includeCarTypeId.Value))
                    .OrderBy(ct => ct.TypeName)
                    .Select(ct => new CarTypeLookupItem
                    {
                        CarTypeId = ct.CarTypeId,
                        TypeName = ct.TypeName,
                        DailyRate = ct.DailyRate
                    })
                    .ToList();

                carTypeComboBox.DisplayMember = "DisplayName";
                carTypeComboBox.ValueMember = "CarTypeId";
                carTypeComboBox.DataSource = carTypes;
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void LoadCars(string search = "")
        {
            try
            {
                using AppDbContext db = new();

                string trimmed = search.Trim();

                List<Car> cars = db.Cars
                    .Include(c => c.CarType)
                    .Where(c => c.Status != CarStatus.Rented &&
                                (trimmed == "" ||
                                 c.PlateNumber.Contains(trimmed) ||
                                 c.Brand.Contains(trimmed) ||
                                 c.Model.Contains(trimmed) ||
                                 (c.CarType != null && c.CarType.TypeName.Contains(trimmed))))
                    .OrderByDescending(c => c.CarId)
                    .ToList();

                carsGrid.Columns.Clear();
                carsGrid.DataSource = cars;

                DataGridViewHelper.ApplyFriendlyHeaders(carsGrid);
                DataGridViewHelper.HideColumn(carsGrid, "CarId");
                DataGridViewHelper.HideColumn(carsGrid, "CarTypeId");
                DataGridViewHelper.HideColumn(carsGrid, "CarType");
                DataGridViewHelper.HideColumn(carsGrid, "ImagePath");
                DataGridViewHelper.HideColumn(carsGrid, "Rentals");
                DataGridViewHelper.MoveColumnToEnd(carsGrid, "Status");
                DataGridViewHelper.ApplyStatusFormatting(carsGrid);

                BeginInvoke(new Action(ClearForm));
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void CarsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (carsGrid.CurrentRow?.DataBoundItem is not Car car || !carsGrid.CurrentRow.Selected)
            {
                selectedCarId = null;
                return;
            }

            selectedCarId = car.CarId;
            plateTextBox.Text = car.PlateNumber;
            brandTextBox.Text = car.Brand;
            modelTextBox.Text = car.Model;
            yearTextBox.Text = car.Year.ToString();
            colorTextBox.Text = car.Color;
            dailyRateTextBox.Text = car.DailyRate.ToString("0.##");
            LoadCarTypes(car.CarTypeId);
            carTypeComboBox.SelectedValue = car.CarTypeId ?? -1;
            if (car.CarTypeId == null)
            {
                dailyRateTextBox.Text = car.DailyRate.ToString("0.##");
            }
            UpdateStatusOptions(car.Status);
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateCarInputs(out int year, out int carTypeId, out decimal dailyRate, out CarStatus status))
            {
                return;
            }

            try
            {
                using AppDbContext db = new();

                string plateNumber = ValidationHelper.NormalizePlateNumber(plateTextBox.Text);

                if (IsDuplicatePlateNumber(db, plateNumber))
                {
                    ShowDuplicatePlateMessage(plateNumber);
                    return;
                }

                db.Cars.Add(new Car
                {
                    PlateNumber = plateNumber,
                    Brand = ValidationHelper.CollapseSpaces(brandTextBox.Text),
                    Model = ValidationHelper.CollapseSpaces(modelTextBox.Text),
                    Year = year,
                    Color = ValidationHelper.CollapseSpaces(colorTextBox.Text),
                    DailyRate = dailyRate,
                    CarTypeId = carTypeId,
                    Status = status,
                    ImagePath = null
                });

                db.SaveChanges();

                MessageBox.Show("Car added successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadCars(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add car.", ex);
            }
        }

        private void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarId == null)
            {
                MessageBox.Show("Please select a car to update.", "No Car Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateCarInputs(out int year, out int carTypeId, out decimal dailyRate, out CarStatus status))
            {
                return;
            }

            try
            {
                using AppDbContext db = new();

                Car? car = db.Cars.Find(selectedCarId.Value);

                if (car == null)
                {
                    MessageBox.Show("Car not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string plateNumber = ValidationHelper.NormalizePlateNumber(plateTextBox.Text);

                if (IsDuplicatePlateNumber(db, plateNumber, selectedCarId.Value))
                {
                    ShowDuplicatePlateMessage(plateNumber);
                    return;
                }

                car.PlateNumber = plateNumber;
                car.Brand = ValidationHelper.CollapseSpaces(brandTextBox.Text);
                car.Model = ValidationHelper.CollapseSpaces(modelTextBox.Text);
                car.Year = year;
                car.Color = ValidationHelper.CollapseSpaces(colorTextBox.Text);
                car.DailyRate = dailyRate;
                car.CarTypeId = carTypeId;
                car.Status = status;
                car.ImagePath = null;

                db.SaveChanges();

                MessageBox.Show("Car updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadCars(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update car.", ex);
            }
        }

        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarId == null)
            {
                MessageBox.Show("Please select a car to delete.", "No Car Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!CanDeleteSelectedCar())
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this car?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using AppDbContext db = new();

                Car? car = db.Cars.Find(selectedCarId.Value);

                if (car != null)
                {
                    db.Cars.Remove(car);
                    db.SaveChanges();
                }

                MessageBox.Show("Car deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadCars(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete car. If this car already has a rental record, keep it and change its status instead.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool CanDeleteSelectedCar()
        {
            if (selectedCarId == null)
            {
                return false;
            }

            try
            {
                using AppDbContext db = new();

                bool hasActiveRental = db.Rentals.Any(r =>
                    r.CarId == selectedCarId.Value &&
                    r.Status == RentalStatus.Active);

                if (hasActiveRental)
                {
                    MessageBox.Show(
                        "This car has an active rental. Process the return before deleting it.",
                        "Car Is Currently Rented",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return false;
                }

                bool hasRentalHistory = db.Rentals.Any(r => r.CarId == selectedCarId.Value);

                if (hasRentalHistory)
                {
                    MessageBox.Show(
                        "This car has rental history, so it cannot be deleted without breaking reports. Change its status to Maintenance instead.",
                        "Car Has Rental History",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to check car rental history.", ex);
                return false;
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadCars(searchTextBox.Text);
        }

        private void CarTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (carTypeComboBox.SelectedItem is CarTypeLookupItem item)
            {
                dailyRateTextBox.Text = item.DailyRate.ToString("0.##");
                return;
            }

            dailyRateTextBox.Clear();
        }

        private bool ValidateCarInputs(out int year, out int carTypeId, out decimal dailyRate, out CarStatus status)
        {
            year = 0;
            carTypeId = 0;
            dailyRate = 0;
            status = CarStatus.Available;

            string plateNumber = ValidationHelper.NormalizePlateNumber(plateTextBox.Text);
            string brand = ValidationHelper.CollapseSpaces(brandTextBox.Text);
            string model = ValidationHelper.CollapseSpaces(modelTextBox.Text);
            string color = ValidationHelper.CollapseSpaces(colorTextBox.Text);

            if (string.IsNullOrWhiteSpace(plateNumber) ||
                string.IsNullOrWhiteSpace(brand) ||
                string.IsNullOrWhiteSpace(model) ||
                string.IsNullOrWhiteSpace(color))
            {
                MessageBox.Show("Please complete all car details.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidPlateNumber(plateNumber))
            {
                MessageBox.Show("Plate number must be 3-20 letters/numbers and may only include spaces or hyphens.", "Invalid Plate Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (brand.Length > 50 || model.Length > 50 || color.Length > 30)
            {
                MessageBox.Show("Brand, model, or color is too long.", "Invalid Car Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!TryGetSelectedStatus(out status))
            {
                MessageBox.Show("Please select a valid car status.", "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(yearTextBox.Text, out year))
            {
                MessageBox.Show("Year must be a valid number.", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!TryGetSelectedCarType(out carTypeId, out dailyRate))
            {
                MessageBox.Show("Please select an active car type/rate first.", "Missing Car Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            if (year < AppConstants.MinimumCarYear || year > DateTime.Today.Year + 1)
            {
                MessageBox.Show("Year must be between 1900 and next year.", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool TryGetSelectedCarType(out int carTypeId, out decimal dailyRate)
        {
            carTypeId = 0;
            dailyRate = 0;

            if (carTypeComboBox.SelectedItem is not CarTypeLookupItem item)
            {
                return false;
            }

            carTypeId = item.CarTypeId;
            dailyRate = item.DailyRate;
            return true;
        }

        private static bool IsDuplicatePlateNumber(AppDbContext db, string plateNumber, int? excludedCarId = null)
        {
            plateNumber = ValidationHelper.NormalizePlateNumber(plateNumber);

            return db.Cars.Any(c =>
                c.PlateNumber == plateNumber &&
                (!excludedCarId.HasValue || c.CarId != excludedCarId.Value));
        }

        private static void ShowDuplicatePlateMessage(string plateNumber)
        {
            MessageBox.Show(
                $"A car with plate number '{plateNumber}' already exists.",
                "Duplicate Plate Number",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void ClearForm()
        {
            selectedCarId = null;
            plateTextBox.Clear();
            brandTextBox.Clear();
            modelTextBox.Clear();
            yearTextBox.Clear();
            colorTextBox.Clear();
            dailyRateTextBox.Clear();

            UpdateStatusOptions();
            LoadCarTypes();

            carsGrid.ClearSelection();
        }

        private static bool TryGetCarStatus(string statusText, out CarStatus status)
        {
            return Enum.TryParse(statusText, ignoreCase: false, out status) &&
                Enum.IsDefined(status);
        }

        private bool TryGetSelectedStatus(out CarStatus status)
        {
            if (statusComboBox.SelectedItem is CarStatus selectedStatus)
            {
                status = selectedStatus;
                return true;
            }

            return TryGetCarStatus(statusComboBox.Text, out status);
        }

        private void UpdateStatusOptions(CarStatus preferredStatus = CarStatus.Available)
        {
            statusComboBox.Items.Clear();
            statusComboBox.Items.Add(CarStatus.Available);
            statusComboBox.Items.Add(CarStatus.Maintenance);

            if (selectedCarId != null && preferredStatus == CarStatus.Rented)
            {
                statusComboBox.Items.Add(CarStatus.Rented);
                statusComboBox.SelectedItem = CarStatus.Rented;
                return;
            }

            statusComboBox.SelectedItem = preferredStatus == CarStatus.Maintenance
                ? CarStatus.Maintenance
                : CarStatus.Available;
        }
    }

    public class CarTypeLookupItem
    {
        public int CarTypeId { get; set; }
        public string TypeName { get; set; } = "";
        public decimal DailyRate { get; set; }
        public string DisplayName => TypeName;
    }
}
