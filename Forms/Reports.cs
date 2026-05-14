using Car_Rental_System.Helpers;
using Car_Rental_System.Presenters;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System
{
    public partial class Reports : Form, IReportsView
    {
        private readonly ReportsPresenter presenter;

        public Reports()
        {
            InitializeComponent();
            presenter = new ReportsPresenter(this);
        }

        public string SearchText => searchTextBox.Text;

        void IReportsView.BindReports(ReportsData reports)
        {
            totalCarsValueLabel.Text = reports.TotalCars.ToString();
            availableCarsValueLabel.Text = reports.AvailableCars.ToString();
            activeRentalsValueLabel.Text = $"{reports.ActiveRentals} / {reports.OverdueRentals}";
            incomeValueLabel.Text = RentalCalculator.FormatCurrency(reports.TotalCollected);

            reportGrid.DataSource = reports.RentalHistory;

            DataGridViewHelper.ApplyFriendlyHeaders(reportGrid);
            DataGridViewHelper.HideColumn(reportGrid, "RentalId");
            DataGridViewHelper.ApplyStatusFormatting(reportGrid);
            DataGridViewHelper.ClearSelectionWhenReady(reportGrid);
        }

        private async void Reports_Load(object? sender, EventArgs e)
        {
            await LoadReportsAsync();
        }

        private async void RefreshButton_Click(object? sender, EventArgs e)
        {
            await LoadReportsAsync();
        }

        private async void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            await LoadReportsAsync();
        }

        private async Task LoadReportsAsync()
        {
            try
            {
                await presenter.LoadReportsAsync();
            }
            catch (Exception ex)
            {
                AppMessageBox.ShowLocalDbWarning(ex);
            }
        }
    }
}

