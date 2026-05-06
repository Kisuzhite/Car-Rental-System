namespace Car_Rental_System.Helpers
{
    internal static class DataGridViewHelper
    {
        private static readonly Dictionary<string, string> FriendlyHeaders = new(StringComparer.OrdinalIgnoreCase)
        {
            ["ActiveRentals"] = "Active Rentals",
            ["ActualReturn"] = "Actual Return",
            ["Address"] = "Address",
            ["Amount"] = "Amount",
            ["BaseAmount"] = "Base Amount",
            ["Brand"] = "Brand",
            ["Car"] = "Car",
            ["CarId"] = "Car ID",
            ["CarType"] = "Car Type",
            ["CarTypeId"] = "Car Type ID",
            ["Color"] = "Color",
            ["ContactNumber"] = "Contact Number",
            ["Customer"] = "Customer",
            ["CustomerId"] = "Customer ID",
            ["DailyRate"] = "Daily Rate",
            ["Description"] = "Description",
            ["ExpectedReturn"] = "Expected Return",
            ["FullName"] = "Full Name",
            ["ImagePath"] = "Image Path",
            ["LicenseNumber"] = "License Number",
            ["Model"] = "Model",
            ["PlateNo"] = "Plate No",
            ["PlateNumber"] = "Plate Number",
            ["RatePerDay"] = "Rate Per Day",
            ["RentalDate"] = "Rental Date",
            ["RentalId"] = "Rental ID",
            ["RentalID"] = "Rental ID",
            ["ReturnDate"] = "Return Date",
            ["Status"] = "Status",
            ["TotalAmount"] = "Total Amount",
            ["Type"] = "Type",
            ["TypeName"] = "Type Name",
            ["Year"] = "Year",
            ["CarTypeName"] = "Car Type"
        };

        public static void HideColumn(DataGridView grid, string columnName)
        {
            DataGridViewColumn? column = grid.Columns[columnName];
            if (column != null)
            {
                column.Visible = false;
            }
        }

        public static void ApplyFriendlyHeaders(DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (FriendlyHeaders.TryGetValue(column.Name, out string? headerText))
                {
                    column.HeaderText = headerText;
                }
            }
        }

        public static void MoveColumnToEnd(DataGridView grid, string columnName)
        {
            DataGridViewColumn? column = grid.Columns[columnName];
            if (column == null)
            {
                return;
            }

            int lastVisibleIndex = grid.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible && c != column)
                .Select(c => c.DisplayIndex)
                .DefaultIfEmpty(0)
                .Max();

            column.DisplayIndex = Math.Min(lastVisibleIndex + 1, grid.Columns.Count - 1);
        }

        public static void ClearSelectionWhenReady(DataGridView grid)
        {
            if (!grid.IsHandleCreated)
            {
                grid.ClearSelection();
                return;
            }

            grid.BeginInvoke(new Action(grid.ClearSelection));
        }

        public static void ApplyStatusFormatting(DataGridView grid)
        {
            grid.CellFormatting -= StatusCellFormatting;
            grid.CellFormatting += StatusCellFormatting;
        }

        private static void StatusCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is not DataGridView grid ||
                e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                !string.Equals(grid.Columns[e.ColumnIndex].Name, "Status", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string status = e.Value?.ToString() ?? "";
            Color backColor = status switch
            {
                AppConstants.AvailableStatus => Color.FromArgb(220, 252, 231),
                AppConstants.RentedStatus => Color.FromArgb(254, 243, 199),
                AppConstants.MaintenanceStatus => Color.FromArgb(243, 244, 246),
                AppConstants.ActiveRentalStatus => Color.FromArgb(219, 234, 254),
                AppConstants.ReturnedRentalStatus => Color.FromArgb(220, 252, 231),
                _ => Color.White
            };

            Color foreColor = status switch
            {
                AppConstants.AvailableStatus => Color.FromArgb(22, 101, 52),
                AppConstants.RentedStatus => Color.FromArgb(146, 64, 14),
                AppConstants.MaintenanceStatus => Color.FromArgb(75, 85, 99),
                AppConstants.ActiveRentalStatus => Color.FromArgb(30, 64, 175),
                AppConstants.ReturnedRentalStatus => Color.FromArgb(22, 101, 52),
                _ => Color.FromArgb(31, 41, 55)
            };

            e.CellStyle.BackColor = backColor;
            e.CellStyle.ForeColor = foreColor;
            e.CellStyle.SelectionBackColor = backColor;
            e.CellStyle.SelectionForeColor = foreColor;
        }
    }
}
