namespace Car_Rental_System
{
    internal static class DataGridViewHelper
    {
        public static void HideColumn(DataGridView grid, string columnName)
        {
            DataGridViewColumn? column = grid.Columns[columnName];
            if (column != null)
            {
                column.Visible = false;
            }
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
