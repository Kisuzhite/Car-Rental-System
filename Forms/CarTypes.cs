using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class CarTypes : Form
    {
        private int? selectedCarTypeId;
        private bool isLoadingCarTypes;

        public CarTypes()
        {
            InitializeComponent();
        }

        private void CarTypes_Load(object? sender, EventArgs e)
        {
            LoadCarTypes();
        }

        private void LoadCarTypes(string search = "")
        {
            isLoadingCarTypes = true;

            try
            {
                using var db = new AppDbContext();

                string trimmed = search.Trim();

                var query = db.CarTypes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    query = query.Where(ct =>
                        ct.TypeName.Contains(trimmed) ||
                        ct.Description.Contains(trimmed));
                }

                var carTypes = query
                    .OrderByDescending(ct => ct.IsActive)
                    .ThenBy(ct => ct.TypeName)
                    .Select(ct => new CarTypeGridRow
                    {
                        CarTypeId = ct.CarTypeId,
                        TypeName = ct.TypeName,
                        DailyRate = ct.DailyRate,
                        Description = ct.Description,
                        Status = ct.IsActive ? "Active" : "Inactive"
                    })
                    .ToList();

                carTypesGrid.Columns.Clear();
                carTypesGrid.DataSource = carTypes;

                DataGridViewHelper.ApplyFriendlyHeaders(carTypesGrid);
                DataGridViewHelper.HideColumn(carTypesGrid, "CarTypeId");
                FormatCarTypesGridColumns();
                ClearFormAfterGridLoad();
            }
            catch (Exception ex)
            {
                isLoadingCarTypes = false;
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out string typeName, out decimal dailyRate, out string description))
            {
                return;
            }

            try
            {
                using var db = new AppDbContext();

                if (IsDuplicateTypeName(db, typeName))
                {
                    ShowDuplicateTypeMessage(typeName);
                    return;
                }

                db.CarTypes.Add(new CarType
                {
                    TypeName = typeName,
                    DailyRate = dailyRate,
                    Description = description,
                    IsActive = isActiveCheckBox.Checked
                });

                db.SaveChanges();

                MessageBox.Show("Car type saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCarTypes(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add car type.", ex);
            }
        }

        private void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarTypeId == null)
            {
                MessageBox.Show("Please select a car type to update.", "No Car Type Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInputs(out string typeName, out decimal dailyRate, out string description))
            {
                return;
            }

            try
            {
                using var db = new AppDbContext();

                var carType = db.CarTypes.Find(selectedCarTypeId.Value);
                if (carType == null)
                {
                    MessageBox.Show("Car type not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadCarTypes(searchTextBox.Text);
                    return;
                }

                if (IsDuplicateTypeName(db, typeName, selectedCarTypeId.Value))
                {
                    ShowDuplicateTypeMessage(typeName);
                    return;
                }

                carType.TypeName = typeName;
                carType.DailyRate = dailyRate;
                carType.Description = description;
                carType.IsActive = isActiveCheckBox.Checked;

                foreach (Car car in db.Cars.Where(c => c.CarTypeId == carType.CarTypeId))
                {
                    car.DailyRate = dailyRate;
                }

                db.SaveChanges();

                MessageBox.Show("Car type updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCarTypes(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update car type.", ex);
            }
        }

        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarTypeId == null)
            {
                MessageBox.Show("Please select a car type to delete.", "No Car Type Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using var db = new AppDbContext();

                var carType = db.CarTypes
                    .Include(ct => ct.Cars)
                    .FirstOrDefault(ct => ct.CarTypeId == selectedCarTypeId.Value);

                if (carType == null)
                {
                    MessageBox.Show("Car type not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadCarTypes(searchTextBox.Text);
                    return;
                }

                bool hasCars = carType.Cars.Any();

                string message = hasCars
                    ? "This car type is assigned to cars. Deactivate it instead of deleting it?"
                    : "Are you sure you want to delete this car type?";

                DialogResult result = MessageBox.Show(
                    message,
                    hasCars ? "Confirm Deactivate" : "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    hasCars ? MessageBoxIcon.Information : MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                if (hasCars)
                {
                    carType.IsActive = false;
                }
                else
                {
                    db.CarTypes.Remove(carType);
                }

                db.SaveChanges();

                MessageBox.Show(
                    hasCars ? "Car type deactivated successfully." : "Car type deleted successfully.",
                    hasCars ? "Deactivated" : "Deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadCarTypes(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to delete car type.", ex);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            LoadCarTypes(searchTextBox.Text);
        }

        private void CarTypesGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (isLoadingCarTypes)
            {
                return;
            }

            if (carTypesGrid.CurrentRow?.DataBoundItem == null || !carTypesGrid.CurrentRow.Selected)
            {
                selectedCarTypeId = null;
                return;
            }

            if (carTypesGrid.CurrentRow.DataBoundItem is not CarTypeGridRow row)
            {
                selectedCarTypeId = null;
                return;
            }

            selectedCarTypeId = row.CarTypeId;
            typeNameTextBox.Text = row.TypeName;
            dailyRateTextBox.Text = row.DailyRate.ToString("0.##");
            descriptionTextBox.Text = row.Description;
            isActiveCheckBox.Checked = row.Status == "Active";
        }

        private void FormatCarTypesGridColumns()
        {
            DataGridViewColumn? typeNameColumn = carTypesGrid.Columns["TypeName"];
            if (typeNameColumn != null)
            {
                typeNameColumn.FillWeight = 22;
            }

            DataGridViewColumn? dailyRateColumn = carTypesGrid.Columns["DailyRate"];
            if (dailyRateColumn != null)
            {
                dailyRateColumn.FillWeight = 18;
                dailyRateColumn.DefaultCellStyle.Format = "N2";
            }

            DataGridViewColumn? descriptionColumn = carTypesGrid.Columns["Description"];
            if (descriptionColumn != null)
            {
                descriptionColumn.FillWeight = 48;
                descriptionColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            DataGridViewColumn? statusColumn = carTypesGrid.Columns["Status"];
            if (statusColumn != null)
            {
                statusColumn.FillWeight = 12;
                statusColumn.MinimumWidth = 90;
                DataGridViewHelper.MoveColumnToEnd(carTypesGrid, "Status");
            }

            carTypesGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private bool ValidateInputs(out string typeName, out decimal dailyRate, out string description)
        {
            typeName = ValidationHelper.CollapseSpaces(typeNameTextBox.Text);
            description = ValidationHelper.CollapseSpaces(descriptionTextBox.Text);
            dailyRate = 0;

            if (string.IsNullOrWhiteSpace(typeName) ||
                string.IsNullOrWhiteSpace(dailyRateTextBox.Text))
            {
                MessageBox.Show("Please enter the car type name and daily rate.", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationHelper.IsValidCarTypeName(typeName))
            {
                MessageBox.Show("Car type name must be 2-50 characters and may contain letters, numbers, spaces, /, &, or hyphens.", "Invalid Type Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(dailyRateTextBox.Text, out dailyRate))
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

            if (description.Length > 200)
            {
                MessageBox.Show("Description cannot be longer than 200 characters.", "Invalid Description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private static bool IsDuplicateTypeName(AppDbContext db, string typeName, int? excludedCarTypeId = null)
        {
            return db.CarTypes.Any(ct =>
                ct.TypeName == typeName &&
                (!excludedCarTypeId.HasValue || ct.CarTypeId != excludedCarTypeId.Value));
        }

        private void ClearFormAfterGridLoad()
        {
            ClearForm();

            if (!carTypesGrid.IsHandleCreated)
            {
                isLoadingCarTypes = false;
                return;
            }

            BeginInvoke(new Action(() =>
            {
                ClearForm();
                isLoadingCarTypes = false;
            }));
        }

        private static void ShowDuplicateTypeMessage(string typeName)
        {
            MessageBox.Show(
                $"A car type named '{typeName}' already exists.",
                "Duplicate Car Type",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void ClearForm()
        {
            selectedCarTypeId = null;
            typeNameTextBox.Clear();
            dailyRateTextBox.Clear();
            descriptionTextBox.Clear();
            isActiveCheckBox.Checked = true;
            carTypesGrid.ClearSelection();
            carTypesGrid.CurrentCell = null;
        }

        private sealed class CarTypeGridRow
        {
            public int CarTypeId { get; set; }
            public string TypeName { get; set; } = "";
            public decimal DailyRate { get; set; }
            public string Description { get; set; } = "";
            public string Status { get; set; } = "";
        }
    }
}
