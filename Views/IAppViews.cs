using Car_Rental_System.Models;
using Car_Rental_System.ViewModels;

namespace Car_Rental_System.Views
{
    internal interface ICarsView
    {
        string SearchText { get; }
        void BindCarTypes(List<CarTypeLookupItem> carTypes);
        void BindCars(List<Car> cars);
    }

    internal interface ICarTypesView
    {
        string SearchText { get; }
        void BindCarTypes(List<CarTypeGridRow> carTypes);
    }

    internal interface ICustomersView
    {
        string SearchText { get; }
        void BindCustomers(List<CustomerGridRow> customers);
    }

    internal interface IRentalsView
    {
        string SearchText { get; }
        void BindCustomers(List<CustomerLookupItem> customers);
        void BindAvailableCars(List<RentalCarLookupItem> cars);
        void BindRentals(List<RentalGridRow> rentals);
    }

    internal interface IReservationsView
    {
        string SearchText { get; }
        void BindAvailableCars(List<ReservationCarLookupItem> cars);
        void BindReservations(List<ReservationGridRow> reservations);
    }

    internal interface IReturnCarView
    {
        string SearchText { get; }
        void BindActiveRentals(List<ActiveRentalGridRow> rentals);
    }

    internal interface IReportsView
    {
        string SearchText { get; }
        void BindReports(ReportsData reports);
    }

    internal interface IMainFormView
    {
        string DashboardSearchText { get; }
        void BindDashboard(DashboardData dashboardData);
        void RefreshAvailableCarSelection(List<CarLookupItem> availableCars);
    }

    internal interface ILoginView
    {
        string UserName { get; }
        string Password { get; }
        void ShowLoginStatus(string message);
        void CompleteLogin(AppUser user);
    }
}
