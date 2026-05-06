using System.Drawing.Drawing2D;

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
            ["LateDays"] = "Late Days",
            ["LicenseNumber"] = "License Number",
            ["Model"] = "Model",
            ["Penalty"] = "Penalty",
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
            ApplyProfessionalGridStyle(grid);
            LockHeaderLayout(grid);

            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (FriendlyHeaders.TryGetValue(column.Name, out string? headerText))
                {
                    column.HeaderText = headerText;
                }

                ApplyColumnSizing(column);
            }
        }

        private static void ApplyProfessionalGridStyle(DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.GridColor = Color.FromArgb(229, 231, 235);
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeight = 44;
            grid.RowTemplate.Height = 46;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 24, 39);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(17, 24, 39);
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 8, 0);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Inter SemiBold", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.DefaultCellStyle.Padding = new Padding(12, 0, 8, 0);
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            grid.AlternatingRowsDefaultCellStyle.Padding = new Padding(12, 0, 8, 0);
        }

        private static void ApplyColumnSizing(DataGridViewColumn column)
        {
            column.MinimumWidth = Math.Max(column.MinimumWidth, GetMinimumColumnWidth(column.Name));
            column.FillWeight = GetColumnFillWeight(column.Name);
        }

        private static int GetMinimumColumnWidth(string columnName)
        {
            return columnName switch
            {
                "Status" => 116,
                "RentalId" or "RentalID" or "CarId" or "CustomerId" or "CarTypeId" => 82,
                "RentalDate" or "ReturnDate" or "ExpectedReturn" or "ActualReturn" => 120,
                "DailyRate" or "RatePerDay" or "BaseAmount" or "TotalAmount" or "Penalty" or "Amount" => 125,
                "LateDays" => 92,
                "PlateNo" or "PlateNumber" => 115,
                "CarType" or "CarTypeName" or "Type" or "TypeName" => 130,
                "Customer" or "FullName" => 170,
                "Car" or "Description" or "Address" => 240,
                _ => 100
            };
        }

        private static float GetColumnFillWeight(string columnName)
        {
            return columnName switch
            {
                "Status" => 78,
                "RentalId" or "RentalID" or "CarId" or "CustomerId" or "CarTypeId" => 55,
                "LateDays" => 62,
                "RentalDate" or "ReturnDate" or "ExpectedReturn" or "ActualReturn" => 85,
                "DailyRate" or "RatePerDay" or "BaseAmount" or "TotalAmount" or "Penalty" or "Amount" => 90,
                "Car" or "Description" or "Address" => 175,
                "Customer" or "FullName" => 125,
                _ => 100
            };
        }

        private static void LockHeaderLayout(DataGridView grid)
        {
            grid.AllowUserToResizeColumns = false;
            grid.AllowUserToResizeRows = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.Resizable = DataGridViewTriState.False;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            grid.CellPainting -= StatusCellPainting;
            grid.CellPainting += StatusCellPainting;

            DataGridViewColumn? statusColumn = grid.Columns["Status"];
            if (statusColumn != null)
            {
                statusColumn.MinimumWidth = Math.Max(statusColumn.MinimumWidth, 116);
                statusColumn.FillWeight = 78;
            }
        }

        private static void StatusCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (sender is not DataGridView grid ||
                e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                !string.Equals(grid.Columns[e.ColumnIndex].Name, "Status", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

            string status = e.FormattedValue?.ToString() ?? "";
            (Color backColor, Color borderColor, Color foreColor) = GetStatusChipColors(status);
            Font chipFont = e.CellStyle?.Font ?? Control.DefaultFont;
            Graphics? graphics = e.Graphics;
            if (graphics == null)
                return;

            Size textSize = TextRenderer.MeasureText(status, chipFont);
            int chipWidth = Math.Min(Math.Max(textSize.Width + 28, 78), e.CellBounds.Width - 24);
            int chipHeight = Math.Min(26, e.CellBounds.Height - 12);
            int chipX = e.CellBounds.Left + 12;
            int chipY = e.CellBounds.Top + (e.CellBounds.Height - chipHeight) / 2;
            Rectangle chipBounds = new(chipX, chipY, chipWidth, chipHeight);

            using GraphicsPath chipPath = CreateRoundedRectanglePath(chipBounds, 13);
            using var chipBrush = new SolidBrush(backColor);
            using var chipPen = new Pen(borderColor);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(chipBrush, chipPath);
            graphics.DrawPath(chipPen, chipPath);

            TextRenderer.DrawText(
                graphics,
                status,
                chipFont,
                chipBounds,
                foreColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            e.Handled = true;
        }

        private static (Color BackColor, Color BorderColor, Color ForeColor) GetStatusChipColors(string status)
        {
            Color backColor = status switch
            {
                AppConstants.AvailableStatus => Color.FromArgb(220, 252, 231),
                AppConstants.RentedStatus => Color.FromArgb(254, 243, 199),
                AppConstants.MaintenanceStatus => Color.FromArgb(243, 244, 246),
                AppConstants.ActiveRentalStatus => Color.FromArgb(219, 234, 254),
                AppConstants.OverdueRentalStatus => Color.FromArgb(254, 226, 226),
                AppConstants.ReturnedRentalStatus => Color.FromArgb(220, 252, 231),
                _ => Color.White
            };

            Color borderColor = status switch
            {
                AppConstants.AvailableStatus => Color.FromArgb(134, 239, 172),
                AppConstants.RentedStatus => Color.FromArgb(252, 211, 77),
                AppConstants.MaintenanceStatus => Color.FromArgb(209, 213, 219),
                AppConstants.ActiveRentalStatus => Color.FromArgb(147, 197, 253),
                AppConstants.OverdueRentalStatus => Color.FromArgb(252, 165, 165),
                AppConstants.ReturnedRentalStatus => Color.FromArgb(134, 239, 172),
                _ => Color.FromArgb(229, 231, 235)
            };

            Color foreColor = status switch
            {
                AppConstants.AvailableStatus => Color.FromArgb(22, 101, 52),
                AppConstants.RentedStatus => Color.FromArgb(146, 64, 14),
                AppConstants.MaintenanceStatus => Color.FromArgb(75, 85, 99),
                AppConstants.ActiveRentalStatus => Color.FromArgb(30, 64, 175),
                AppConstants.OverdueRentalStatus => Color.FromArgb(153, 27, 27),
                AppConstants.ReturnedRentalStatus => Color.FromArgb(22, 101, 52),
                _ => Color.FromArgb(31, 41, 55)
            };

            return (backColor, borderColor, foreColor);
        }

        private static GraphicsPath CreateRoundedRectanglePath(Rectangle rectangle, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();

            path.AddArc(rectangle.Left, rectangle.Top, diameter, diameter, 180, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Top, diameter, diameter, 270, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rectangle.Left, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
