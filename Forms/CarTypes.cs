using Car_Rental_System.Helpers;
using Car_Rental_System.Presenters;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;
using Guna.UI2.WinForms;

namespace Car_Rental_System
{
    public partial class CarTypes : Form, ICarTypesView
    {
        private readonly CarTypesPresenter presenter;
        private int? selectedCarTypeId;
        private bool isLoadingCarTypes;

        public CarTypes()
        {
            InitializeComponent();
            presenter = new CarTypesPresenter(this);
            SetCarTypeEditMode(isExistingType: false);
        }

        public string SearchText => string.Empty;

        void ICarTypesView.BindCarTypes(List<CarTypeGridRow> carTypes)
        {
            carTypesGrid.DataSource = carTypes;

            DataGridViewHelper.ApplyFriendlyHeaders(carTypesGrid);
            DataGridViewHelper.HideColumn(carTypesGrid, "CarTypeId");
            FormatCarTypesGridColumns();
            ClearFormAfterGridLoad();
        }

        private async void CarTypes_Load(object? sender, EventArgs e)
        {
            await LoadCarTypesAsync();
        }

        private async Task LoadCarTypesAsync()
        {
            isLoadingCarTypes = true;

            try
            {
                await presenter.LoadCarTypesAsync();
            }
            catch (Exception ex)
            {
                isLoadingCarTypes = false;
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private async void AddButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out CarTypeFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.AddCarTypeAsync(input);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    await LoadCarTypesAsync();
                }
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to add car type.", ex);
            }
        }

        private async void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarTypeId == null)
            {
                MessageBox.Show("Please select a car type to update.", "No Car Type Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInputs(out CarTypeFormInput input))
            {
                return;
            }

            try
            {
                PresenterResult result = await presenter.UpdateCarTypeAsync(selectedCarTypeId.Value, input);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    await LoadCarTypesAsync();
                }
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseError("Unable to update car type.", ex);
            }
        }

        private async void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (selectedCarTypeId == null)
            {
                MessageBox.Show("Please select a car type to delete.", "No Car Type Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                CarTypeDeletePreview preview = await presenter.PreviewDeleteAsync(selectedCarTypeId.Value);
                if (!preview.CanContinue)
                {
                    MessageBox.Show(preview.Message, preview.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    await LoadCarTypesAsync();
                    return;
                }

                DialogResult confirmation = MessageBox.Show(
                    preview.Message,
                    preview.Title,
                    MessageBoxButtons.YesNo,
                    preview.HasCars ? MessageBoxIcon.Information : MessageBoxIcon.Question);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                PresenterResult result = await presenter.DeleteCarTypeAsync(selectedCarTypeId.Value);
                AppMessageBox.ShowPresenterResult(result);

                if (result.Success)
                {
                    await LoadCarTypesAsync();
                }
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

        private void CarTypesGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (isLoadingCarTypes)
            {
                return;
            }

            if (carTypesGrid.CurrentRow?.DataBoundItem is not CarTypeGridRow row ||
                !carTypesGrid.CurrentRow.Selected)
            {
                selectedCarTypeId = null;
                SetCarTypeEditMode(isExistingType: false);
                return;
            }

            selectedCarTypeId = row.CarTypeId;
            SetCarTypeEditMode(isExistingType: true);
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
                statusColumn.FillWeight = 16;
                statusColumn.MinimumWidth = Math.Max(statusColumn.MinimumWidth, 116);
                DataGridViewHelper.MoveColumnToEnd(carTypesGrid, "Status");
            }

            carTypesGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private bool ValidateInputs(out CarTypeFormInput input)
        {
            string typeName = ValidationHelper.CollapseSpaces(typeNameTextBox.Text);
            string description = ValidationHelper.CollapseSpaces(descriptionTextBox.Text);
            input = new CarTypeFormInput(typeName, 0, description, isActiveCheckBox.Checked);

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

            if (!decimal.TryParse(dailyRateTextBox.Text, out decimal dailyRate))
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

            input = new CarTypeFormInput(typeName, dailyRate, description, isActiveCheckBox.Checked);
            return true;
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

        private void ClearForm()
        {
            selectedCarTypeId = null;
            typeNameTextBox.Clear();
            dailyRateTextBox.Clear();
            descriptionTextBox.Clear();
            isActiveCheckBox.Checked = true;
            carTypesGrid.ClearSelection();
            carTypesGrid.CurrentCell = null;
            SetCarTypeEditMode(isExistingType: false);
        }

        private void SetCarTypeEditMode(bool isExistingType)
        {
            sectionLabel.Text = isExistingType ? "Selected Rate" : "Rate Details";

            addButton.Enabled = !isExistingType;
            updateButton.Enabled = isExistingType;
            deleteButton.Enabled = isExistingType;

            SetTypeNameState(typeNameTextBox, isExistingType);
        }

        private static void SetTypeNameState(Guna2TextBox textBox, bool isLocked)
        {
            textBox.ReadOnly = isLocked;
            textBox.FillColor = isLocked
                ? Color.FromArgb(243, 244, 246)
                : Color.White;
            textBox.Cursor = isLocked
                ? Cursors.Default
                : Cursors.IBeam;
            textBox.FocusedState.BorderColor = isLocked
                ? Color.FromArgb(209, 213, 219)
                : Color.FromArgb(37, 99, 235);
            textBox.HoverState.BorderColor = isLocked
                ? Color.FromArgb(209, 213, 219)
                : Color.FromArgb(37, 99, 235);
        }
    }
}

