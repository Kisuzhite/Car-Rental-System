using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System
{
    public partial class Cars : Form, ICarsView
    {
        private readonly CarsPresenter presenter;
        private int? selectedCarId;

        public Cars()
        {
            InitializeComponent();
            presenter = new CarsPresenter(this);
        }

        public string SearchText => searchTextBox.Text;

        void ICarsView.BindCarTypes(List<CarTypeLookupItem> carTypes)
        {
            carTypeComboBox.DisplayMember = "TypeName";
            carTypeComboBox.ValueMember = "CarTypeId";
            carTypeComboBox.DataSource = carTypes;
        }

        void ICarsView.BindCars(List<CarGridRow> cars)
        {
            carsGrid.DataSource = cars;

            DataGridViewHelper.ApplyFriendlyHeaders(carsGrid);
            DataGridViewHelper.HideColumn(carsGrid, "CarId");
            DataGridViewHelper.MoveColumnToEnd(carsGrid, "Status");
            DataGridViewHelper.ApplyStatusFormatting(carsGrid);

            BeginInvoke(new Action(async () => await ClearFormAsync()));
        }

        private async void Cars_Load(object? sender, EventArgs e)
        {
            await PrepareFormAsync();
            await LoadCarsAsync();
        }

        private async Task PrepareFormAsync()
        {
            UpdateStatusOptions();
            await LoadCarTypesAsync();
        }

        private async Task LoadCarTypesAsync(int? includeCarTypeId = null)
        {
            try
            {
                await presenter.LoadCarTypesAsync(includeCarTypeId);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private async Task LoadCarsAsync()
        {
            try
            {
                await presenter.LoadCarsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private async void CarsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (carsGrid.CurrentRow?.DataBoundItem is not CarGridRow car || !carsGrid.CurrentRow.Selected)
            {
                selectedCarId = null;
                return;
            }

            selectedCarId = car.CarId;
            int carId = car.CarId;
            plateTextBox.Text = car.PlateNumber;
            brandTextBox.Text = car.Brand;
            modelTextBox.Text = car.Model;
            yearTextBox.Text = car.Year.ToString();
            colorTextBox.Text = car.Color;
            dailyRateTextBox.Text = car.DailyRate.ToString("0.##");
            await LoadCarTypesAsync();

            if (selectedCarId != carId)
            {
                return;
            }

            SelectCarType(car.CarType);

            UpdateStatusOptions(car.Status);
        }

        private async void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateCarInputs(out CarFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.AddCarAsync(input);
                AppMessageBox.ShowPresenterResult(result);

                if (!result.Success)
                {
                    return;
                }

                await ClearFormAsync();
                await LoadCarsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add car.", ex);
            }
        }

        private async void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarId == null)
            {
                MessageBox.Show("Please select a car to update.", "No Car Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateCarInputs(out CarFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.UpdateCarAsync(selectedCarId.Value, input);
                AppMessageBox.ShowPresenterResult(result);

                if (!result.Success)
                {
                    return;
                }

                await ClearFormAsync();
                await LoadCarsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update car.", ex);
            }
        }

        private async void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarId == null)
            {
                MessageBox.Show("Please select a car to delete.", "No Car Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                PresenterResult canDelete = await presenter.CanDeleteCarAsync(selectedCarId.Value);
                if (!canDelete.Success)
                {
                    AppMessageBox.ShowPresenterResult(canDelete);
                    return;
                }

                DialogResult confirmation = MessageBox.Show(
                    "Are you sure you want to delete this car?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                PresenterResult result = await presenter.DeleteCarAsync(selectedCarId.Value);
                AppMessageBox.ShowPresenterResult(result);

                if (!result.Success)
                {
                    return;
                }

                await ClearFormAsync();
                await LoadCarsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to delete car.", ex);
            }
        }

        private async void ClearButton_Click(object? sender, EventArgs e)
        {
            await ClearFormAsync();
        }

        private async void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            await LoadCarsAsync();
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

        private bool ValidateCarInputs(out CarFormInput input)
        {
            input = new CarFormInput("", "", "", 0, "", 0, 0, CarStatus.Available);

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

            if (!TryGetSelectedStatus(out CarStatus status))
            {
                MessageBox.Show("Please select a valid car status.", "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(yearTextBox.Text, out int year))
            {
                MessageBox.Show("Year must be a valid number.", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!TryGetSelectedCarType(out int carTypeId, out decimal dailyRate))
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

            input = new CarFormInput(plateNumber, brand, model, year, color, carTypeId, dailyRate, status);
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

        private void SelectCarType(string typeName)
        {
            foreach (object item in carTypeComboBox.Items)
            {
                if (item is CarTypeLookupItem carType &&
                    string.Equals(carType.TypeName, typeName, StringComparison.OrdinalIgnoreCase))
                {
                    carTypeComboBox.SelectedItem = item;
                    return;
                }
            }
        }

        private async Task ClearFormAsync()
        {
            selectedCarId = null;
            plateTextBox.Clear();
            brandTextBox.Clear();
            modelTextBox.Clear();
            yearTextBox.Clear();
            colorTextBox.Clear();
            dailyRateTextBox.Clear();

            UpdateStatusOptions();
            await LoadCarTypesAsync();

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
}

