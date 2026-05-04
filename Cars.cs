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
            LoadCarImage("");
        }

        private void LoadCars(string search = "")
        {
            try
            {
                using AppDbContext db = new();

                string trimmed = search.Trim();

                List<Car> cars = db.Cars
                    .Where(c => c.Status != AppConstants.RentedStatus &&
                                (trimmed == "" ||
                                 c.PlateNumber.Contains(trimmed) ||
                                 c.Brand.Contains(trimmed) ||
                                 c.Model.Contains(trimmed)))
                    .OrderByDescending(c => c.CarId)
                    .ToList();

                carsGrid.Columns.Clear();
                carsGrid.DataSource = cars;

                DataGridViewHelper.HideColumn(carsGrid, "CarId");
                DataGridViewHelper.HideColumn(carsGrid, "ImagePath");
                DataGridViewHelper.HideColumn(carsGrid, "Rentals");
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
            statusComboBox.Text = car.Status;
            UpdateStatusOptions();
            imagePathTextBox.Text = car.ImagePath;
            LoadCarImage(car.ImagePath);
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateCarInputs(out int year, out decimal dailyRate))
            {
                return;
            }

            try
            {
                using AppDbContext db = new();

                db.Cars.Add(new Car
                {
                    PlateNumber = plateTextBox.Text.Trim(),
                    Brand = brandTextBox.Text.Trim(),
                    Model = modelTextBox.Text.Trim(),
                    Year = year,
                    Color = colorTextBox.Text.Trim(),
                    DailyRate = dailyRate,
                    Status = statusComboBox.Text.Trim(),
                    ImagePath = string.IsNullOrWhiteSpace(imagePathTextBox.Text) ? null : imagePathTextBox.Text.Trim()
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

            if (!ValidateCarInputs(out int year, out decimal dailyRate))
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

                car.PlateNumber = plateTextBox.Text.Trim();
                car.Brand = brandTextBox.Text.Trim();
                car.Model = modelTextBox.Text.Trim();
                car.Year = year;
                car.Color = colorTextBox.Text.Trim();
                car.DailyRate = dailyRate;
                car.Status = statusComboBox.Text.Trim();
                car.ImagePath = string.IsNullOrWhiteSpace(imagePathTextBox.Text) ? null : imagePathTextBox.Text.Trim();

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
                    r.Status == AppConstants.ActiveRentalStatus);

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

        private void BrowseImageButton_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog dialog = new()
            {
                Title = "Select Car Photo",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            string userImagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Cars", "UserImages");
            Directory.CreateDirectory(userImagesFolder);

            string extension = Path.GetExtension(dialog.FileName);
            string fileName = $"{Guid.NewGuid():N}{extension}";
            string destination = Path.Combine(userImagesFolder, fileName);
            File.Copy(dialog.FileName, destination, overwrite: true);

            imagePathTextBox.Text = Path.Combine("Assets", "Cars", "UserImages", fileName);
            LoadCarImage(imagePathTextBox.Text);
        }

        private bool ValidateCarInputs(out int year, out decimal dailyRate)
        {
            year = 0;
            dailyRate = 0;

            if (string.IsNullOrWhiteSpace(plateTextBox.Text) ||
                string.IsNullOrWhiteSpace(brandTextBox.Text) ||
                string.IsNullOrWhiteSpace(modelTextBox.Text) ||
                string.IsNullOrWhiteSpace(colorTextBox.Text) ||
                string.IsNullOrWhiteSpace(statusComboBox.Text))
            {
                MessageBox.Show("Please complete all car details.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(yearTextBox.Text, out year))
            {
                MessageBox.Show("Year must be a valid number.", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(dailyRateTextBox.Text, out dailyRate))
            {
                MessageBox.Show("Daily rate must be a valid amount.", "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (year < AppConstants.MinimumCarYear || year > DateTime.Today.Year + 1)
            {
                MessageBox.Show("Year must be between 1900 and next year.", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dailyRate <= 0)
            {
                MessageBox.Show("Daily rate must be greater than zero.", "Invalid Rate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
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
            imagePathTextBox.Clear();

            UpdateStatusOptions();

            LoadCarImage("");
            carsGrid.ClearSelection();
        }

        private void LoadCarImage(string? imagePath)
        {
            string resolvedPath = ResolveImagePath(imagePath);

            if (!File.Exists(resolvedPath))
            {
                resolvedPath = ResolveImagePath(AppConstants.DefaultCarImagePath);
            }

            carImagePictureBox.Image?.Dispose();

            if (!File.Exists(resolvedPath))
            {
                carImagePictureBox.Image = null;
                return;
            }

            try
            {
                using Image image = Image.FromFile(resolvedPath);
                carImagePictureBox.Image = new Bitmap(image);
            }
            catch
            {
                carImagePictureBox.Image = null;
            }
        }

        private string ResolveImagePath(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConstants.DefaultCarImagePath);
            }

            return Path.IsPathRooted(imagePath)
                ? imagePath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
        }

        private void UpdateStatusOptions()
        {
            string? currentStatus = statusComboBox.Text;

            statusComboBox.Items.Clear();
            statusComboBox.Items.Add(AppConstants.AvailableStatus);
            statusComboBox.Items.Add(AppConstants.MaintenanceStatus);

            if (selectedCarId != null && currentStatus == AppConstants.RentedStatus)
            {
                statusComboBox.Items.Add(AppConstants.RentedStatus);
                statusComboBox.Text = AppConstants.RentedStatus;
                return;
            }

            statusComboBox.SelectedIndex = 0;
        }
    }
}
