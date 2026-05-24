using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using Car_Rental_System.Models;

namespace Car_Rental_System.Helpers
{
    internal static class DataGridViewHelper
    {
        private const int DefaultRowHeight = 46;

        private static readonly ConditionalWeakTable<DataGridView, EmptyGridState> EmptyGridStates = new();

        private static readonly Dictionary<string, string> FriendlyHeaders = new(StringComparer.OrdinalIgnoreCase)
        {
            ["ActiveRentals"] = "Active Rentals",
            ["ActualReturn"] = "Actual Return",
            ["Address"] = "Address",
            ["Amount"] = "Amount",
            ["AmountPaid"] = "Amount Paid",
            ["BaseAmount"] = "Base Amount",
            ["Balance"] = "Balance",
            ["Brand"] = "Brand",
            ["Car"] = "Car",
            ["CarId"] = "Car ID",
            ["CarType"] = "Car Type",
            ["CarTypeId"] = "Car Type ID",
            ["Color"] = "Color",
            ["ContactNumber"] = "Contact Number",
            ["Customer"] = "Customer",
            ["CustomerId"] = "Customer ID",
            ["CustomerName"] = "Customer Name",
            ["DailyRate"] = "Daily Rate",
            ["Description"] = "Description",
            ["DepositAmount"] = "Deposit Amount",
            ["ExpectedReturn"] = "Expected Return",
            ["ExpectedReturnDate"] = "Expected Return",
            ["FullName"] = "Full Name",
            ["ImagePath"] = "Image Path",
            ["LateDays"] = "Late Days",
            ["LicenseNumber"] = "License Number",
            ["Model"] = "Model",
            ["PaymentMethod"] = "Payment Method",
            ["PaymentReferenceNumber"] = "Payment Reference",
            ["PaymentStatus"] = "Payment Status",
            ["Penalty"] = "Penalty",
            ["PlateNo"] = "Plate No",
            ["PlateNumber"] = "Plate Number",
            ["PickupDate"] = "Pickup Date",
            ["RatePerDay"] = "Rate Per Day",
            ["ReservationDate"] = "Reservation Date",
            ["ReservationId"] = "Reservation ID",
            ["RentalDate"] = "Rental Date",
            ["RentalId"] = "Rental ID",
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
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grid.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.AdvancedColumnHeadersBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedColumnHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedColumnHeadersBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedRowHeadersBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedRowHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedRowHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            grid.AdvancedRowHeadersBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            grid.GridColor = Color.White;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.ColumnHeadersHeight = 56;
            grid.RowTemplate.Height = DefaultRowHeight;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 24, 39);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(17, 24, 39);
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 8, 0);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Inter SemiBold", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

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

            grid.RowPrePaint -= RemoveCurrentCellFocusBorder;
            grid.RowPrePaint += RemoveCurrentCellFocusBorder;
            grid.CellPainting -= PaintBorderlessCell;
            grid.CellPainting += PaintBorderlessCell;
            grid.RowPostPaint -= CoverGridLinePixels;
            grid.RowPostPaint += CoverGridLinePixels;

            ApplyRowHeights(grid);
            grid.Invalidate();
        }

        private static void ApplyRowHeights(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                row.Height = DefaultRowHeight;
            }
        }

        private static void ApplyColumnSizing(DataGridViewColumn column)
        {
            column.MinimumWidth = Math.Max(column.MinimumWidth, GetMinimumColumnWidth(column.Name));
            column.MinimumWidth = Math.Max(column.MinimumWidth, GetHeaderMinimumWidth(column));
            column.FillWeight = GetColumnFillWeight(column.Name);

            if (IsCurrencyColumn(column.Name))
            {
                column.DefaultCellStyle.Format = RentalCalculator.CurrencyFormat;
            }
        }

        private static int GetHeaderMinimumWidth(DataGridViewColumn column)
        {
            string headerText = string.IsNullOrWhiteSpace(column.HeaderText)
                ? column.Name
                : column.HeaderText;

            Size textSize = TextRenderer.MeasureText(
                headerText,
                new Font("Inter SemiBold", 9F, FontStyle.Bold),
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding);

            return Math.Min(Math.Max(textSize.Width + 34, 82), 220);
        }

        private static int GetMinimumColumnWidth(string columnName)
        {
            return columnName switch
            {
                "Status" => 116,
                "RentalId" or "ReservationId" or "CarId" or "CustomerId" or "CarTypeId" => 82,
                "RentalDate" or "ReturnDate" or "ExpectedReturn" or "ExpectedReturnDate" or "ActualReturn" or "ReservationDate" or "PickupDate" => 120,
                "DailyRate" or "RatePerDay" or "BaseAmount" or "TotalAmount" or "AmountPaid" or "DepositAmount" or "Balance" or "Penalty" or "Amount" => 125,
                "LateDays" => 92,
                "PaymentMethod" or "PaymentStatus" => 125,
                "PaymentReferenceNumber" => 160,
                "PlateNo" or "PlateNumber" => 115,
                "CarType" or "CarTypeName" or "Type" or "TypeName" => 130,
                "Customer" or "FullName" or "CustomerName" => 170,
                "Car" or "Description" or "Address" => 240,
                _ => 100
            };
        }

        private static float GetColumnFillWeight(string columnName)
        {
            return columnName switch
            {
                "Status" => 78,
                "RentalId" or "ReservationId" or "CarId" or "CustomerId" or "CarTypeId" => 55,
                "LateDays" => 62,
                "PaymentMethod" or "PaymentStatus" => 90,
                "RentalDate" or "ReturnDate" or "ExpectedReturn" or "ExpectedReturnDate" or "ActualReturn" or "ReservationDate" or "PickupDate" => 85,
                "DailyRate" or "RatePerDay" or "BaseAmount" or "TotalAmount" or "AmountPaid" or "DepositAmount" or "Balance" or "Penalty" or "Amount" => 90,
                "Car" or "Description" or "Address" => 175,
                "Customer" or "FullName" or "CustomerName" => 125,
                _ => 100
            };
        }

        private static bool IsCurrencyColumn(string columnName)
        {
            return columnName is
                "DailyRate" or
                "RatePerDay" or
                "BaseAmount" or
                "TotalAmount" or
                "AmountPaid" or
                "DepositAmount" or
                "Balance" or
                "Penalty" or
                "Amount";
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

        private static void RemoveCurrentCellFocusBorder(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }

        private static void PaintBorderlessCell(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (sender is not DataGridView grid ||
                e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                e.CellStyle == null ||
                e.Graphics == null ||
                e.AdvancedBorderStyle == null)
            {
                return;
            }

            DataGridViewCellStyle cellStyle = e.CellStyle;
            Graphics graphics = e.Graphics;
            DataGridViewAdvancedBorderStyle borderStyle = e.AdvancedBorderStyle;

            borderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            borderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            borderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            borderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            DataGridViewColumn? column = grid.Columns[e.ColumnIndex];
            if (column is DataGridViewButtonColumn)
            {
                return;
            }

            if (column != null && IsStatusColumn(column.Name))
            {
                PaintCellBackground(e, graphics, cellStyle);
                PaintStatusChip(
                    graphics,
                    e.CellBounds,
                    e.FormattedValue?.ToString() ?? string.Empty,
                    cellStyle.Font ?? grid.Font);
                e.Handled = true;
                return;
            }

            PaintTextCell(e, graphics, cellStyle);
            e.Handled = true;
        }

        private static void PaintTextCell(
            DataGridViewCellPaintingEventArgs e,
            Graphics graphics,
            DataGridViewCellStyle cellStyle)
        {
            PaintCellBackground(e, graphics, cellStyle);

            string text = e.FormattedValue?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Rectangle textBounds = Rectangle.Inflate(e.CellBounds, -12, 0);
            TextRenderer.DrawText(
                graphics,
                text,
                cellStyle.Font ?? Control.DefaultFont,
                textBounds,
                cellStyle.ForeColor,
                GetTextFormatFlags(cellStyle.Alignment));
        }

        private static TextFormatFlags GetTextFormatFlags(DataGridViewContentAlignment alignment)
        {
            TextFormatFlags flags = TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPrefix |
                TextFormatFlags.VerticalCenter;

            flags |= alignment switch
            {
                DataGridViewContentAlignment.MiddleCenter or
                    DataGridViewContentAlignment.TopCenter or
                    DataGridViewContentAlignment.BottomCenter => TextFormatFlags.HorizontalCenter,
                DataGridViewContentAlignment.MiddleRight or
                    DataGridViewContentAlignment.TopRight or
                    DataGridViewContentAlignment.BottomRight => TextFormatFlags.Right,
                _ => TextFormatFlags.Left
            };

            return flags;
        }

        private static void PaintCellBackground(
            DataGridViewCellPaintingEventArgs e,
            Graphics graphics,
            DataGridViewCellStyle cellStyle)
        {
            bool isSelected = (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
            Color backColor = isSelected ? cellStyle.SelectionBackColor : cellStyle.BackColor;
            if (backColor == Color.Empty)
            {
                backColor = Color.White;
            }

            using var backgroundBrush = new SolidBrush(backColor);
            Rectangle bounds = new(e.CellBounds.Left - 1, e.CellBounds.Top - 1, e.CellBounds.Width + 2, e.CellBounds.Height + 2);
            graphics.FillRectangle(backgroundBrush, bounds);
        }

        private static void CoverGridLinePixels(object? sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (sender is not DataGridView grid || e.RowIndex < 0)
            {
                return;
            }

            Color rowBackColor = GetRowBackColor(grid, e.RowIndex, e.State);
            using var rowBrush = new SolidBrush(rowBackColor);

            int left = grid.RowHeadersVisible ? grid.RowHeadersWidth : 0;
            int right = grid.DisplayRectangle.Right;
            if (right <= left)
            {
                return;
            }

            int bottom = e.RowBounds.Bottom - 2;
            if (bottom >= e.RowBounds.Top)
            {
                e.Graphics.FillRectangle(rowBrush, new Rectangle(left, bottom, right - left, 2));
            }

            foreach (DataGridViewColumn column in grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible))
            {
                Rectangle cellBounds = grid.GetCellDisplayRectangle(column.Index, e.RowIndex, false);
                if (cellBounds.Width <= 0 || cellBounds.Height <= 0)
                {
                    continue;
                }

                e.Graphics.FillRectangle(
                    rowBrush,
                    new Rectangle(cellBounds.Right - 2, cellBounds.Top, 2, cellBounds.Height));
            }
        }

        private static Color GetRowBackColor(DataGridView grid, int rowIndex, DataGridViewElementStates state)
        {
            bool isSelected = (state & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
            if (isSelected)
            {
                Color selectedBackColor = grid.DefaultCellStyle.SelectionBackColor;
                return selectedBackColor == Color.Empty ? Color.FromArgb(239, 246, 255) : selectedBackColor;
            }

            DataGridViewRow row = grid.Rows[rowIndex];
            if (row.DefaultCellStyle.BackColor != Color.Empty)
            {
                return row.DefaultCellStyle.BackColor;
            }

            if (rowIndex % 2 == 1 && grid.AlternatingRowsDefaultCellStyle.BackColor != Color.Empty)
            {
                return grid.AlternatingRowsDefaultCellStyle.BackColor;
            }

            Color defaultBackColor = grid.DefaultCellStyle.BackColor;
            return defaultBackColor == Color.Empty ? Color.White : defaultBackColor;
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
                ClearSelectionAndCurrentCell(grid);
                return;
            }

            grid.BeginInvoke(new Action(() => ClearSelectionAndCurrentCell(grid)));
        }

        public static void ApplyEmptyState(DataGridView grid, string message)
        {
            EmptyGridState state = EmptyGridStates.GetOrCreateValue(grid);
            state.Message = message;

            grid.Paint -= PaintEmptyGridMessage;
            grid.Paint += PaintEmptyGridMessage;
            grid.DataBindingComplete -= InvalidateEmptyGridState;
            grid.DataBindingComplete += InvalidateEmptyGridState;
            grid.RowsAdded -= InvalidateEmptyGridState;
            grid.RowsAdded += InvalidateEmptyGridState;
            grid.RowsRemoved -= InvalidateEmptyGridState;
            grid.RowsRemoved += InvalidateEmptyGridState;
            grid.Invalidate();
        }

        private static void InvalidateEmptyGridState(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            InvalidateEmptyGridState(sender);
        }

        private static void InvalidateEmptyGridState(object? sender, DataGridViewRowsAddedEventArgs e)
        {
            InvalidateEmptyGridState(sender);
        }

        private static void InvalidateEmptyGridState(object? sender, DataGridViewRowsRemovedEventArgs e)
        {
            InvalidateEmptyGridState(sender);
        }

        private static void InvalidateEmptyGridState(object? sender)
        {
            if (sender is DataGridView grid && !grid.IsDisposed)
            {
                grid.Invalidate();
            }
        }

        private static void PaintEmptyGridMessage(object? sender, PaintEventArgs e)
        {
            if (sender is not DataGridView grid ||
                grid.IsDisposed ||
                grid.Rows.Count > 0 ||
                !EmptyGridStates.TryGetValue(grid, out EmptyGridState? state) ||
                string.IsNullOrWhiteSpace(state.Message))
            {
                return;
            }

            Rectangle bounds = grid.ClientRectangle;
            bounds.Y += grid.ColumnHeadersVisible ? grid.ColumnHeadersHeight : 0;
            bounds.Height -= grid.ColumnHeadersVisible ? grid.ColumnHeadersHeight : 0;

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            using var brush = new SolidBrush(Color.FromArgb(107, 114, 128));
            using var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            using var font = new Font("Inter", 10F, FontStyle.Regular);

            e.Graphics.DrawString(
                state.Message,
                font,
                brush,
                bounds,
                format);
        }

        public static void KeepDisplayOnly(DataGridView grid)
        {
            grid.TabStop = false;
            grid.SelectionChanged -= ClearDisplayGridSelection;
            grid.SelectionChanged += ClearDisplayGridSelection;
            ClearSelectionWhenReady(grid);
        }

        private static void ClearDisplayGridSelection(object? sender, EventArgs e)
        {
            if (sender is not DataGridView grid)
            {
                return;
            }

            if (!grid.IsHandleCreated)
            {
                ClearSelectionAndCurrentCell(grid);
                return;
            }

            grid.BeginInvoke(new Action(() => ClearSelectionAndCurrentCell(grid)));
        }

        private static void ClearSelectionAndCurrentCell(DataGridView grid)
        {
            if (grid.IsDisposed ||
                (grid.SelectedCells.Count == 0 && grid.SelectedRows.Count == 0 && grid.CurrentCell == null))
            {
                return;
            }

            grid.ClearSelection();
            grid.CurrentCell = null;
        }

        public static void ApplyStatusFormatting(DataGridView grid)
        {
            DataGridViewColumn? statusColumn = grid.Columns["Status"];
            if (statusColumn != null)
            {
                statusColumn.MinimumWidth = Math.Max(statusColumn.MinimumWidth, 116);
                statusColumn.FillWeight = 78;
            }

            DataGridViewColumn? paymentStatusColumn = grid.Columns["PaymentStatus"];
            if (paymentStatusColumn != null)
            {
                paymentStatusColumn.MinimumWidth = Math.Max(paymentStatusColumn.MinimumWidth, 125);
                paymentStatusColumn.FillWeight = 90;
            }
        }

        public static void ApplyFullTextColumn(DataGridView grid, string columnName, int minimumWidth = 320)
        {
            DataGridViewColumn? column = grid.Columns[columnName];
            if (column == null)
            {
                return;
            }

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.MinimumWidth = Math.Max(column.MinimumWidth, minimumWidth);
            column.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
        }

        private static void PaintStatusChip(Graphics graphics, Rectangle cellBounds, string status, Font chipFont)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return;
            }

            (Color backColor, Color borderColor, Color foreColor) = GetStatusChipColors(status);
            Size textSize = TextRenderer.MeasureText(status, chipFont);
            int chipWidth = Math.Min(Math.Max(textSize.Width + 28, 78), Math.Max(0, cellBounds.Width - 24));
            int chipHeight = Math.Min(26, Math.Max(0, cellBounds.Height - 12));
            if (chipWidth == 0 || chipHeight == 0)
            {
                return;
            }

            int chipX = cellBounds.Left + 12;
            int chipY = cellBounds.Top + (cellBounds.Height - chipHeight) / 2;
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
        }

        private static bool IsStatusColumn(string columnName)
        {
            return string.Equals(columnName, "Status", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(columnName, "PaymentStatus", StringComparison.OrdinalIgnoreCase);
        }

        private static (Color BackColor, Color BorderColor, Color ForeColor) GetStatusChipColors(string status)
        {
            Color backColor = status switch
            {
                nameof(CarStatus.Available) => Color.FromArgb(220, 252, 231),
                nameof(CarStatus.Rented) => Color.FromArgb(254, 243, 199),
                nameof(CarStatus.Maintenance) => Color.FromArgb(243, 244, 246),
                nameof(RentalStatus.Active) => Color.FromArgb(219, 234, 254),
                AppConstants.OverdueRentalStatus => Color.FromArgb(254, 226, 226),
                nameof(RentalStatus.Returned) => Color.FromArgb(220, 252, 231),
                nameof(PaymentStatus.Unpaid) => Color.FromArgb(254, 226, 226),
                nameof(PaymentStatus.Partial) => Color.FromArgb(254, 243, 199),
                nameof(PaymentStatus.Paid) => Color.FromArgb(220, 252, 231),
                nameof(ReservationStatus.Pending) => Color.FromArgb(254, 243, 199),
                nameof(ReservationStatus.Confirmed) => Color.FromArgb(219, 234, 254),
                nameof(ReservationStatus.Cancelled) => Color.FromArgb(243, 244, 246),
                nameof(ReservationStatus.Converted) => Color.FromArgb(220, 252, 231),
                _ => Color.White
            };

            Color borderColor = status switch
            {
                nameof(CarStatus.Available) => Color.FromArgb(134, 239, 172),
                nameof(CarStatus.Rented) => Color.FromArgb(252, 211, 77),
                nameof(CarStatus.Maintenance) => Color.FromArgb(209, 213, 219),
                nameof(RentalStatus.Active) => Color.FromArgb(147, 197, 253),
                AppConstants.OverdueRentalStatus => Color.FromArgb(252, 165, 165),
                nameof(RentalStatus.Returned) => Color.FromArgb(134, 239, 172),
                nameof(PaymentStatus.Unpaid) => Color.FromArgb(252, 165, 165),
                nameof(PaymentStatus.Partial) => Color.FromArgb(252, 211, 77),
                nameof(PaymentStatus.Paid) => Color.FromArgb(134, 239, 172),
                nameof(ReservationStatus.Pending) => Color.FromArgb(252, 211, 77),
                nameof(ReservationStatus.Confirmed) => Color.FromArgb(147, 197, 253),
                nameof(ReservationStatus.Cancelled) => Color.FromArgb(209, 213, 219),
                nameof(ReservationStatus.Converted) => Color.FromArgb(134, 239, 172),
                _ => Color.FromArgb(229, 231, 235)
            };

            Color foreColor = status switch
            {
                nameof(CarStatus.Available) => Color.FromArgb(22, 101, 52),
                nameof(CarStatus.Rented) => Color.FromArgb(146, 64, 14),
                nameof(CarStatus.Maintenance) => Color.FromArgb(75, 85, 99),
                nameof(RentalStatus.Active) => Color.FromArgb(30, 64, 175),
                AppConstants.OverdueRentalStatus => Color.FromArgb(153, 27, 27),
                nameof(RentalStatus.Returned) => Color.FromArgb(22, 101, 52),
                nameof(PaymentStatus.Unpaid) => Color.FromArgb(153, 27, 27),
                nameof(PaymentStatus.Partial) => Color.FromArgb(146, 64, 14),
                nameof(PaymentStatus.Paid) => Color.FromArgb(22, 101, 52),
                nameof(ReservationStatus.Pending) => Color.FromArgb(146, 64, 14),
                nameof(ReservationStatus.Confirmed) => Color.FromArgb(30, 64, 175),
                nameof(ReservationStatus.Cancelled) => Color.FromArgb(75, 85, 99),
                nameof(ReservationStatus.Converted) => Color.FromArgb(22, 101, 52),
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

        private sealed class EmptyGridState
        {
            public string Message { get; set; } = "";
        }
    }
}
