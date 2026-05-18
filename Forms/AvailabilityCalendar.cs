using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System
{
    public partial class AvailabilityCalendar : Form
    {
        private const int LeftColumnWidth = 300;
        private const int HeaderHeight = 64;
        private const int RowHeight = 102;
        private const int BarHeight = 30;
        private const int TargetDayColumnWidth = 142;
        private static readonly Color PageBackColor = Color.FromArgb(243, 244, 246);
        private static readonly Color BoardBackColor = Color.White;
        private static readonly Color HeaderCellBackColor = Color.FromArgb(248, 250, 252);
        private static readonly Color CarCellBackColor = Color.White;
        private static readonly Color GridLineColor = Color.FromArgb(226, 232, 240);
        private static readonly Color PrimaryTextColor = Color.FromArgb(17, 24, 39);
        private static readonly Color MutedTextColor = Color.FromArgb(75, 85, 99);
        private static readonly Color RequestedBackColor = Color.FromArgb(219, 234, 254);

        private readonly DateOnly requestedPickupDate;
        private readonly DateOnly requestedReturnDate;
        private readonly int? selectedCarId;
        private DateOnly visibleStartDate;
        private List<CalendarCarRow> calendarRows = [];

        public AvailabilityCalendar()
            : this(RentalCalculator.ToDateOnly(DateTime.Today), RentalCalculator.ToDateOnly(DateTime.Today.AddDays(1)))
        {
        }

        public AvailabilityCalendar(DateOnly pickupDate, DateOnly returnDate, int? selectedCarId = null)
        {
            InitializeComponent();
            requestedPickupDate = pickupDate;
            requestedReturnDate = returnDate;
            this.selectedCarId = selectedCarId;
            visibleStartDate = GetMonthStart(pickupDate);
        }

        private async void AvailabilityCalendar_Load(object? sender, EventArgs e)
        {
            FitWindowToCalendar();
            await LoadCalendarAsync();
        }

        private async void PreviousButton_Click(object? sender, EventArgs e)
        {
            visibleStartDate = visibleStartDate.AddMonths(-1);
            await LoadCalendarAsync();
        }

        private async void NextButton_Click(object? sender, EventArgs e)
        {
            visibleStartDate = visibleStartDate.AddMonths(1);
            await LoadCalendarAsync();
        }

        private void TimelineScrollPanel_Resize(object? sender, EventArgs e)
        {
            if (calendarRows.Count > 0)
            {
                BindCalendar(calendarRows, GetVisibleEndDate());
            }
        }

        private async Task LoadCalendarAsync()
        {
            try
            {
                DateOnly visibleEndDate = GetVisibleEndDate();
                calendarRows = await LoadCalendarRowsAsync(visibleStartDate, visibleEndDate);
                BindCalendar(calendarRows, visibleEndDate);
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private static async Task<List<CalendarCarRow>> LoadCalendarRowsAsync(DateOnly visibleStartDate, DateOnly visibleEndDate)
        {
            using var db = new AppDbContext();

            List<Car> cars = await db.Cars
                .Include(c => c.CarType)
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Model)
                .ThenBy(c => c.PlateNumber)
                .ToListAsync();

            List<Rental> rentals = await db.Rentals
                .Include(r => r.Customer)
                .Where(r =>
                    r.Status == RentalStatus.Active &&
                    visibleStartDate <= r.ExpectedReturnDate &&
                    r.RentalDate <= visibleEndDate)
                .ToListAsync();

            List<Reservation> reservations = await db.Reservations
                .Where(r =>
                    (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed) &&
                    visibleStartDate <= r.ExpectedReturnDate &&
                    r.PickupDate <= visibleEndDate)
                .ToListAsync();

            return cars
                .Select(car =>
                {
                    List<CalendarBookingBlock> blocks = new();

                    blocks.AddRange(reservations
                        .Where(r => r.CarId == car.CarId)
                        .Select(r => new CalendarBookingBlock(
                            car.CarId,
                            r.PickupDate,
                            r.ExpectedReturnDate,
                            $"Reserved - {r.CustomerName}",
                            CalendarBlockType.Reservation)));

                    blocks.AddRange(rentals
                        .Where(r => r.CarId == car.CarId)
                        .Select(r => new CalendarBookingBlock(
                            car.CarId,
                            r.RentalDate,
                            r.ExpectedReturnDate,
                            $"Rented - {r.Customer.FullName}",
                            CalendarBlockType.Rental)));

                    return new CalendarCarRow(
                        car.CarId,
                        car.PlateNumber,
                        $"{car.Brand} {car.Model}",
                        car.CarType?.TypeName ?? AppConstants.NotProvided,
                        car.CarType?.DailyRate ?? car.DailyRate,
                        car.Status,
                        blocks
                            .OrderBy(b => b.StartDate)
                            .ThenBy(b => b.Type)
                            .ToList());
                })
                .ToList();
        }

        private void BindCalendar(List<CalendarCarRow> rows, DateOnly visibleEndDate)
        {
            UpdateHeader(visibleEndDate);
            timelineScrollPanel.SuspendLayout();
            timelineScrollPanel.AutoScrollPosition = Point.Empty;
            timelineScrollPanel.Controls.Clear();

            int visibleDays = GetVisibleDayCount();
            int contentWidth = GetTimelineContentWidth();
            int dayColumnWidth = GetDayColumnWidth(contentWidth, visibleDays);
            int contentHeight = HeaderHeight + rows.Count * RowHeight + 24;

            var content = new BufferedPanel
            {
                BackColor = BoardBackColor,
                Location = new Point(0, 0),
                Size = new Size(contentWidth, contentHeight)
            };

            BuildDateHeader(content, visibleDays, dayColumnWidth);
            BuildCarRows(content, rows, visibleDays, dayColumnWidth);
            timelineScrollPanel.Controls.Add(content);
            timelineScrollPanel.AutoScrollMinSize = new Size(contentWidth, contentHeight);
            content.Location = Point.Empty;
            timelineScrollPanel.ResumeLayout();
            BeginInvoke(new Action(() => ScrollToRelevantDate(dayColumnWidth, contentWidth)));
        }

        private void BuildDateHeader(Control content, int visibleDays, int dayColumnWidth)
        {
            var carHeader = new Label
            {
                BackColor = HeaderCellBackColor,
                Font = new Font("Inter SemiBold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(0, 0),
                Size = new Size(LeftColumnWidth, HeaderHeight),
                Text = "CAR / RATE",
                TextAlign = ContentAlignment.MiddleCenter
            };
            carHeader.Paint += (_, e) => DrawCellBorder(e.Graphics, carHeader.ClientRectangle);
            content.Controls.Add(carHeader);

            for (int dayIndex = 0; dayIndex < visibleDays; dayIndex++)
            {
                DateOnly date = visibleStartDate.AddDays(dayIndex);
                string dayName = date.ToString("dddd");

                var dateHeader = new Label
                {
                    BackColor = IsRequestedDate(date) ? RequestedBackColor : HeaderCellBackColor,
                    Font = new Font("Inter SemiBold", 8.8F, FontStyle.Bold),
                    ForeColor = PrimaryTextColor,
                    Location = new Point(LeftColumnWidth + dayIndex * dayColumnWidth, 0),
                    Size = new Size(dayColumnWidth, HeaderHeight),
                    Text = $"{dayName}\n{date.Day}",
                    TextAlign = ContentAlignment.MiddleCenter
                };
                dateHeader.Paint += (_, e) => DrawCellBorder(e.Graphics, dateHeader.ClientRectangle);
                content.Controls.Add(dateHeader);
            }
        }

        private void BuildCarRows(Control content, List<CalendarCarRow> rows, int visibleDays, int dayColumnWidth)
        {
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                CalendarCarRow row = rows[rowIndex];
                int rowTop = HeaderHeight + rowIndex * RowHeight;

                content.Controls.Add(BuildCarInfoPanel(row, rowTop));
                content.Controls.Add(BuildGridRowPanel(rowTop, visibleDays, dayColumnWidth));

                foreach (CalendarBookingBlock block in row.Blocks)
                {
                    AddBookingBlock(content, block, rowTop, dayColumnWidth);
                }
            }
        }

        private Control BuildCarInfoPanel(CalendarCarRow row, int rowTop)
        {
            var panel = new Guna2Panel
            {
                BackColor = Color.Transparent,
                BorderColor = row.CarId == selectedCarId ? Color.FromArgb(37, 99, 235) : GridLineColor,
                BorderThickness = row.CarId == selectedCarId ? 2 : 1,
                FillColor = row.CarId == selectedCarId ? Color.FromArgb(239, 246, 255) : CarCellBackColor,
                Location = new Point(0, rowTop),
                Size = new Size(LeftColumnWidth, RowHeight)
            };

            panel.Controls.Add(new Label
            {
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Font = new Font("Inter SemiBold", 9.6F, FontStyle.Bold),
                ForeColor = PrimaryTextColor,
                Location = new Point(17, 13),
                Size = new Size(180, 24),
                Text = row.PlateNumber
            });

            panel.Controls.Add(new Label
            {
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Font = new Font("Inter", 9F),
                ForeColor = MutedTextColor,
                Location = new Point(17, 39),
                Size = new Size(260, 22),
                Text = row.CarName
            });

            panel.Controls.Add(new Label
            {
                AutoEllipsis = false,
                BackColor = Color.Transparent,
                Font = new Font("Inter SemiBold", 8.8F, FontStyle.Bold),
                ForeColor = PrimaryTextColor,
                Location = new Point(17, 66),
                Size = new Size(150, 23),
                Text = $"{RentalCalculator.FormatCurrency(row.DailyRate)}/day"
            });

            var typeBadge = new Guna2Panel
            {
                BackColor = Color.Transparent,
                BorderColor = Color.FromArgb(203, 213, 225),
                BorderRadius = 10,
                BorderThickness = 1,
                FillColor = Color.FromArgb(248, 250, 252),
                Location = new Point(210, 13),
                Size = new Size(72, 24)
            };
            typeBadge.Controls.Add(new Label
            {
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                Font = new Font("Inter SemiBold", 7.8F, FontStyle.Bold),
                ForeColor = PrimaryTextColor,
                Text = row.CarType,
                TextAlign = ContentAlignment.MiddleCenter
            });
            panel.Controls.Add(typeBadge);

            panel.Controls.Add(new Label
            {
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Font = new Font("Inter SemiBold", 8F, FontStyle.Bold),
                ForeColor = GetStatusColor(row.Status),
                Location = new Point(175, 66),
                Size = new Size(110, 23),
                Text = row.Status.ToString(),
                TextAlign = ContentAlignment.MiddleLeft
            });

            return panel;
        }

        private Control BuildGridRowPanel(int rowTop, int visibleDays, int dayColumnWidth)
        {
            var panel = new BufferedPanel
            {
                BackColor = BoardBackColor,
                Location = new Point(LeftColumnWidth, rowTop),
                Size = new Size(visibleDays * dayColumnWidth, RowHeight)
            };

            panel.Paint += (_, e) => DrawGridRow(e.Graphics, visibleDays, dayColumnWidth);
            return panel;
        }

        private void DrawGridRow(Graphics graphics, int visibleDays, int dayColumnWidth)
        {
            using var gridPen = new Pen(GridLineColor);
            using var requestedBrush = new SolidBrush(RequestedBackColor);

            for (int dayIndex = 0; dayIndex < visibleDays; dayIndex++)
            {
                DateOnly date = visibleStartDate.AddDays(dayIndex);
                int x = dayIndex * dayColumnWidth;

                if (IsRequestedDate(date))
                {
                    graphics.FillRectangle(requestedBrush, x, 0, dayColumnWidth, RowHeight);
                }

                graphics.DrawLine(gridPen, x, 0, x, RowHeight);
            }

            graphics.DrawLine(gridPen, visibleDays * dayColumnWidth - 1, 0, visibleDays * dayColumnWidth - 1, RowHeight);
            graphics.DrawLine(gridPen, 0, RowHeight - 1, visibleDays * dayColumnWidth, RowHeight - 1);
        }

        private void AddBookingBlock(Control content, CalendarBookingBlock block, int rowTop, int dayColumnWidth)
        {
            DateOnly visibleEndDate = GetVisibleEndDate();
            DateOnly start = block.StartDate < visibleStartDate ? visibleStartDate : block.StartDate;
            DateOnly end = block.EndDate > visibleEndDate ? visibleEndDate : block.EndDate;

            if (start > visibleEndDate || end < visibleStartDate)
            {
                return;
            }

            int offsetDays = start.DayNumber - visibleStartDate.DayNumber;
            int durationDays = Math.Max(1, end.DayNumber - start.DayNumber + 1);
            int x = LeftColumnWidth + offsetDays * dayColumnWidth + 5;
            int width = Math.Max(170, durationDays * dayColumnWidth - 12);
            int y = rowTop + (RowHeight - BarHeight) / 2;

            var blockPanel = new Guna2Panel
            {
                BackColor = Color.Transparent,
                BorderRadius = 8,
                FillColor = GetBlockColor(block.Type),
                Location = new Point(x, y),
                Size = new Size(width, BarHeight)
            };

            blockPanel.Controls.Add(new Label
            {
                AutoEllipsis = true,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                Font = new Font("Inter SemiBold", 8.4F, FontStyle.Bold),
                ForeColor = Color.White,
                Padding = new Padding(10, 0, 8, 0),
                Text = block.Label,
                TextAlign = ContentAlignment.MiddleLeft
            });

            content.Controls.Add(blockPanel);
            blockPanel.BringToFront();
        }

        private void UpdateHeader(DateOnly visibleEndDate)
        {
            rangeLabel.Text = $"{visibleStartDate:MMMM yyyy}";
            helperLabel.Text = $"Requested booking: {requestedPickupDate:MMM d} - {requestedReturnDate:MMM d, yyyy}";
        }

        private int GetVisibleDayCount()
        {
            return DateTime.DaysInMonth(visibleStartDate.Year, visibleStartDate.Month);
        }

        private DateOnly GetVisibleEndDate()
        {
            return visibleStartDate.AddDays(GetVisibleDayCount() - 1);
        }

        private int GetTimelineContentWidth()
        {
            int scrollbarAllowance = SystemInformation.VerticalScrollBarWidth + 8;
            int fittedWidth = timelineScrollPanel.ClientSize.Width - scrollbarAllowance;
            int minimumReadableWidth = LeftColumnWidth + GetVisibleDayCount() * TargetDayColumnWidth;
            return Math.Max(minimumReadableWidth, fittedWidth);
        }

        private static int GetDayColumnWidth(int contentWidth, int visibleDays)
        {
            int dateAreaWidth = Math.Max(visibleDays * TargetDayColumnWidth, contentWidth - LeftColumnWidth);
            return Math.Max(TargetDayColumnWidth, dateAreaWidth / visibleDays);
        }

        private bool IsRequestedDate(DateOnly date)
        {
            return requestedPickupDate <= date && date <= requestedReturnDate;
        }

        private void ScrollToRelevantDate(int dayColumnWidth, int contentWidth)
        {
            DateOnly today = RentalCalculator.ToDateOnly(DateTime.Today);
            DateOnly visibleEndDate = GetVisibleEndDate();
            DateOnly targetDate = today >= visibleStartDate && today <= visibleEndDate
                ? today
                : requestedPickupDate >= visibleStartDate && requestedPickupDate <= visibleEndDate
                    ? requestedPickupDate
                    : visibleStartDate;

            int dayOffset = targetDate.DayNumber - visibleStartDate.DayNumber;
            int targetCenter = LeftColumnWidth + dayOffset * dayColumnWidth + dayColumnWidth / 2;
            int horizontalOffset = Math.Max(0, targetCenter - timelineScrollPanel.ClientSize.Width / 2);
            int maxOffset = Math.Max(0, contentWidth - timelineScrollPanel.ClientSize.Width);
            timelineScrollPanel.AutoScrollPosition = new Point(Math.Min(horizontalOffset, maxOffset), 0);
        }

        private static DateOnly GetMonthStart(DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1);
        }

        private static Color GetBlockColor(CalendarBlockType blockType)
        {
            return blockType switch
            {
                CalendarBlockType.Rental => Color.FromArgb(239, 68, 68),
                _ => Color.FromArgb(245, 158, 11)
            };
        }

        private static Color GetStatusColor(CarStatus status)
        {
            return status switch
            {
                CarStatus.Rented => Color.FromArgb(220, 38, 38),
                _ => Color.FromArgb(22, 163, 74)
            };
        }

        private static void DrawCellBorder(Graphics graphics, Rectangle bounds)
        {
            using var pen = new Pen(GridLineColor);
            graphics.DrawRectangle(pen, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1);
        }

        private void FitWindowToCalendar()
        {
            Rectangle workingArea = Owner != null
                ? Screen.FromControl(Owner).WorkingArea
                : Screen.FromControl(this).WorkingArea;
            int desiredWidth = LeftColumnWidth + GetVisibleDayCount() * TargetDayColumnWidth + 86;
            int desiredHeight = 790;
            int width = Math.Min(desiredWidth, Math.Max(MinimumSize.Width, workingArea.Width - 80));
            int height = Math.Min(desiredHeight, Math.Max(MinimumSize.Height, workingArea.Height - 80));
            Bounds = new Rectangle(
                workingArea.Left + (workingArea.Width - width) / 2,
                workingArea.Top + (workingArea.Height - height) / 2,
                width,
                height);
        }

        private enum CalendarBlockType
        {
            Reservation,
            Rental
        }

        private sealed record CalendarCarRow(
            int CarId,
            string PlateNumber,
            string CarName,
            string CarType,
            decimal DailyRate,
            CarStatus Status,
            List<CalendarBookingBlock> Blocks);

        private sealed record CalendarBookingBlock(
            int CarId,
            DateOnly StartDate,
            DateOnly EndDate,
            string Label,
            CalendarBlockType Type);

        private sealed class BufferedPanel : Panel
        {
            public BufferedPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }
        }
    }
}
