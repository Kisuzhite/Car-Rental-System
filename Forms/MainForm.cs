using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Presenters;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;
using Guna.Charts.WinForms;
using Guna.UI2.WinForms;

namespace Car_Rental_System
{
    public partial class MainForm : Form, IMainFormView
    {
        private readonly Padding dashboardPadding;
        private readonly AppUser currentUser;
        private readonly MainFormPresenter presenter;
        private Form? activeModuleForm;

        public MainForm() : this(new AppUser { UserName = "owner", Role = AppConstants.OwnerRole })
        {
        }

        internal MainForm(AppUser currentUser)
        {
            InitializeComponent();
            AppIcon.ApplyTo(this);
            this.currentUser = currentUser;
            dashboardPadding = contentPanel.Padding;
            presenter = new MainFormPresenter(this);
        }

        void IMainFormView.BindDashboard(DashboardData dashboardData)
        {
            totalCarsValueLabel.Text = RentalCalculator.FormatCurrency(dashboardData.TotalRevenue);
            availableCarsValueLabel.Text = dashboardData.ActiveBookings.ToString();
            rentedCarsValueLabel.Text = dashboardData.RentedCars.ToString();
            incomeValueLabel.Text = dashboardData.AvailableCars.ToString();
            overdueRentalsValueLabel.Text = dashboardData.OverdueRentals.ToString();
            rentedCarsSubLabel.Text = dashboardData.RentedCars == 0 ? "No cars out" : "Out on rental";
            incomeSubLabel.Text = dashboardData.AvailableCars == 0 ? "None ready" : "Ready to rent";
            overdueRentalsSubLabel.Text = dashboardData.OverdueRentals == 0 ? "No overdue today" : "Needs follow-up";

            BindDashboardSchedule(dashboardData);
            BindDashboardCharts(dashboardData);
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToString("MMMM d, yyyy");
            staffBadgeLabel.Text = $"{currentUser.Role} Account";
            SetActiveNavigationButton(dashboardButton);

            if (await TryPrepareDatabaseAsync())
            {
                await LoadDashboardDataAsync();
            }
        }

        private static async Task<bool> TryPrepareDatabaseAsync()
        {
            try
            {
                await MainFormPresenter.PrepareDatabaseAsync();
                return true;
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowDatabaseStartupError(ex);
                return false;
            }
        }

        private async Task LoadDashboardDataAsync()
        {
            try
            {
                await presenter.LoadDashboardDataAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }

        private async void NavigationButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Guna2Button selectedButton) return;

            ResetNavigationButtons();
            SetActiveNavigationButton(selectedButton);
            pageTitleLabel.Text = selectedButton.Text;

            if (selectedButton == dashboardButton)
            {
                ShowDashboard();
                await LoadDashboardDataAsync();
                return;
            }

            Form? moduleForm = selectedButton == carsButton ? new Cars()
                : selectedButton == ratesButton ? new CarTypes()
                : selectedButton == customersButton ? new Customers()
                : selectedButton == rentalsButton ? new Rentals()
                : selectedButton == reservationsButton ? new Reservations()
                : selectedButton == returnsButton ? new ReturnCar()
                : selectedButton == reportsButton ? new Reports()
                : null;

            if (moduleForm == null) return;

            ShowModule(moduleForm);
        }

        private void ResetNavigationButtons()
        {
            Guna2Button[] buttons = { dashboardButton, carsButton, ratesButton, customersButton, rentalsButton, reservationsButton, returnsButton, reportsButton };

            foreach (Guna2Button button in buttons)
            {
                button.BackColor = Color.Transparent;
                button.FillColor = Color.FromArgb(17, 24, 39);
                button.ForeColor = Color.White;
                button.HoverState.FillColor = Color.FromArgb(31, 41, 55);
            }
        }

        private static void SetActiveNavigationButton(Guna2Button button)
        {
            button.BackColor = Color.Transparent;
            button.FillColor = Color.FromArgb(37, 99, 235);
            button.ForeColor = Color.White;
            button.HoverState.FillColor = Color.FromArgb(29, 78, 216);
        }

        private void ShowDashboard()
        {
            topPanel.Visible = true;
            contentPanel.Padding = dashboardPadding;

            if (activeModuleForm != null)
            {
                contentPanel.Controls.Remove(activeModuleForm);
                activeModuleForm.Dispose();
                activeModuleForm = null;
            }

            foreach (Control control in contentPanel.Controls)
                control.Visible = true;
        }

        private void ShowModule(Form moduleForm)
        {
            topPanel.Visible = false;
            contentPanel.Padding = Padding.Empty;

            if (activeModuleForm != null)
            {
                contentPanel.Controls.Remove(activeModuleForm);
                activeModuleForm.Dispose();
            }

            foreach (Control control in contentPanel.Controls)
                control.Visible = false;

            activeModuleForm = moduleForm;
            moduleForm.TopLevel = false;
            moduleForm.FormBorderStyle = FormBorderStyle.None;
            moduleForm.Dock = DockStyle.Fill;
            moduleForm.ShowInTaskbar = false;

            contentPanel.Controls.Add(moduleForm);
            moduleForm.Show();
        }

        private void LogoutButton_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            Close();
        }

        private void BindDashboardSchedule(DashboardData dashboardData)
        {
            Label[] dayNameLabels =
            {
                scheduleDay1NameLabel,
                scheduleDay2NameLabel,
                scheduleDay3NameLabel,
                scheduleDay4NameLabel,
                scheduleDay5NameLabel,
                scheduleDay6NameLabel,
                scheduleDay7NameLabel
            };

            Label[] dateLabels =
            {
                scheduleDay1DateLabel,
                scheduleDay2DateLabel,
                scheduleDay3DateLabel,
                scheduleDay4DateLabel,
                scheduleDay5DateLabel,
                scheduleDay6DateLabel,
                scheduleDay7DateLabel
            };

            Label[] pickupLabels =
            {
                scheduleDay1PickupsLabel,
                scheduleDay2PickupsLabel,
                scheduleDay3PickupsLabel,
                scheduleDay4PickupsLabel,
                scheduleDay5PickupsLabel,
                scheduleDay6PickupsLabel,
                scheduleDay7PickupsLabel
            };

            Label[] returnLabels =
            {
                scheduleDay1ReturnsLabel,
                scheduleDay2ReturnsLabel,
                scheduleDay3ReturnsLabel,
                scheduleDay4ReturnsLabel,
                scheduleDay5ReturnsLabel,
                scheduleDay6ReturnsLabel,
                scheduleDay7ReturnsLabel
            };

            Guna2Panel[] dayPanels =
            {
                scheduleDay1Panel,
                scheduleDay2Panel,
                scheduleDay3Panel,
                scheduleDay4Panel,
                scheduleDay5Panel,
                scheduleDay6Panel,
                scheduleDay7Panel
            };

            for (int i = 0; i < dayNameLabels.Length && i < dashboardData.WeeklySchedule.Count; i++)
            {
                DashboardScheduleDay scheduleDay = dashboardData.WeeklySchedule[i];
                bool hasActivity = scheduleDay.PickupCount > 0 || scheduleDay.ReturnCount > 0;

                dayNameLabels[i].Text = scheduleDay.Date.ToString("ddd");
                dateLabels[i].Text = scheduleDay.Date.ToString("MMM d");
                pickupLabels[i].Text = FormatScheduleCount(scheduleDay.PickupCount, "pickup", "↗");
                returnLabels[i].Text = FormatScheduleCount(scheduleDay.ReturnCount, "return", "↙");
                pickupLabels[i].ForeColor = scheduleDay.PickupCount > 0
                    ? Color.FromArgb(37, 99, 235)
                    : Color.FromArgb(107, 114, 128);
                returnLabels[i].ForeColor = scheduleDay.ReturnCount > 0
                    ? Color.FromArgb(20, 184, 166)
                    : Color.FromArgb(107, 114, 128);
                SetScheduleDayStyle(dayPanels[i], hasActivity);
            }
        }

        private static string FormatScheduleCount(int count, string label, string icon)
        {
            string suffix = count == 1 ? label : label + "s";
            return $"{icon} {count} {suffix}";
        }

        private void BindDashboardCharts(DashboardData dashboardData)
        {
            BindRevenueSummary(dashboardData.MonthlyRevenue);

            monthlyRevenueChart.Datasets.Clear();
            var revenueDataset = new GunaBarDataset
            {
                Label = "Revenue",
                BorderWidth = 0,
                YFormat = "₱#,##0"
            };

            Color currentMonthColor = Color.FromArgb(37, 99, 235);
            Color previousMonthColor = Color.FromArgb(191, 219, 254);
            int currentMonthIndex = dashboardData.MonthlyRevenue.Count - 1;

            for (int i = 0; i < dashboardData.MonthlyRevenue.Count; i++)
            {
                DashboardRevenuePoint point = dashboardData.MonthlyRevenue[i];
                revenueDataset.FillColors.Add(i == currentMonthIndex ? currentMonthColor : previousMonthColor);
                revenueDataset.DataPoints.Add(new LPoint
                {
                    Label = point.Month,
                    Y = (double)point.Amount
                });
            }

            monthlyRevenueChart.Datasets.Add(revenueDataset);
            monthlyRevenueChart.Update();

            BindPaymentCollectionSummary(dashboardData.PaymentStatus);

            fleetStatusChart.Datasets.Clear();
            var paymentDataset = new GunaDoughnutDataset
            {
                Label = "Payments",
                BorderWidth = 0,
                YFormat = "N0"
            };

            Color[] paymentColors =
            {
                Color.FromArgb(16, 185, 129),
                Color.FromArgb(245, 158, 11)
            };

            for (int i = 0; i < dashboardData.PaymentStatus.Count; i++)
            {
                DashboardPaymentStatusPoint point = dashboardData.PaymentStatus[i];
                paymentDataset.FillColors.Add(paymentColors[i % paymentColors.Length]);
                paymentDataset.DataPoints.Add(new LPoint
                {
                    Label = point.Status,
                    Y = point.Count
                });
            }

            fleetStatusChart.Datasets.Add(paymentDataset);
            fleetStatusChart.Update();
        }

        private void BindRevenueSummary(List<DashboardRevenuePoint> revenuePoints)
        {
            decimal thisMonthRevenue = revenuePoints.Count == 0 ? 0 : revenuePoints[^1].Amount;
            decimal bestMonthRevenue = revenuePoints.Count == 0 ? 0 : revenuePoints.Max(point => point.Amount);
            decimal averageRevenue = revenuePoints.Count == 0 ? 0 : revenuePoints.Average(point => point.Amount);

            thisMonthRevenueValueLabel.Text = RentalCalculator.FormatCurrency(thisMonthRevenue);
            bestMonthRevenueValueLabel.Text = RentalCalculator.FormatCurrency(bestMonthRevenue);
            averageRevenueValueLabel.Text = RentalCalculator.FormatCurrency(averageRevenue);
        }

        private void BindPaymentCollectionSummary(List<DashboardPaymentStatusPoint> paymentStatus)
        {
            int fullyPaid = paymentStatus.FirstOrDefault(point => point.Status == "Fully Paid")?.Count ?? 0;
            int balanceRemaining = paymentStatus.FirstOrDefault(point => point.Status == "Balance Remaining")?.Count ?? 0;
            int total = fullyPaid + balanceRemaining;
            int paidPercent = total == 0
                ? 0
                : (int)Math.Round(fullyPaid * 100m / total, MidpointRounding.AwayFromZero);

            paymentPaidPercentLabel.Text = paidPercent + "%";
            paidStatusValueLabel.Text = fullyPaid.ToString();
            balanceStatusValueLabel.Text = balanceRemaining.ToString();
        }

        private static void SetScheduleDayStyle(Guna2Panel panel, bool hasActivity)
        {
            Color fillColor = hasActivity ? Color.FromArgb(239, 246, 255) : Color.FromArgb(249, 250, 251);
            Color borderColor = hasActivity ? Color.FromArgb(191, 219, 254) : Color.FromArgb(229, 231, 235);

            panel.BackColor = fillColor;
            panel.FillColor = fillColor;
            panel.BorderColor = borderColor;
        }

    }
}
