using Car_Rental_System.Models;

namespace Car_Rental_System.ViewModels
{
    internal sealed class CarTypeLookupItem
    {
        public int CarTypeId { get; set; }
        public string TypeName { get; set; } = "";
        public decimal DailyRate { get; set; }
    }

    internal sealed class CarLookupItem
    {
        public int? CarId { get; set; }
        public string CarName { get; set; } = "";
        public decimal? DailyRate { get; set; }
    }

    internal sealed class CustomerLookupItem
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = "";
    }

    internal sealed class RentalCarLookupItem
    {
        public int CarId { get; set; }
        public string CarName { get; set; } = "";
        public decimal DailyRate { get; set; }
    }

    internal sealed class ReservationCarLookupItem
    {
        public int CarId { get; set; }
        public string CarName { get; set; } = "";
        public decimal DailyRate { get; set; }
    }

    internal sealed class CarTypeGridRow
    {
        public int CarTypeId { get; set; }
        public string TypeName { get; set; } = "";
        public decimal DailyRate { get; set; }
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
    }

    internal sealed class CarGridRow
    {
        public int CarId { get; set; }
        public string PlateNumber { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public string Color { get; set; } = "";
        public decimal DailyRate { get; set; }
        public string CarType { get; set; } = "";
        public CarStatus Status { get; set; }
    }

    internal sealed class CustomerGridRow
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string LicenseNumber { get; set; } = "";
        public string Address { get; set; } = "";
    }

    internal sealed class ReservationGridRow
    {
        public int ReservationId { get; set; }
        public int CarId { get; set; }
        public string CustomerName { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string Car { get; set; } = "";
        public DateOnly ReservationDate { get; set; }
        public DateOnly PickupDate { get; set; }
        public DateOnly ExpectedReturnDate { get; set; }
        public decimal DepositAmount { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string PaymentReferenceNumber { get; set; } = "";
        public ReservationStatus Status { get; set; }
    }

    internal sealed class ActiveRentalGridRow
    {
        public int RentalId { get; set; }
        public int CarId { get; set; }
        public string Customer { get; set; } = "";
        public string Car { get; set; } = "";
        public DateOnly RentalDate { get; set; }
        public DateOnly ExpectedReturn { get; set; }
        public decimal DailyRate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; } = "";
        public int LateDays { get; set; }
        public decimal Penalty { get; set; }
        public string Status { get; set; } = "";
    }

    internal sealed class RentalGridRow
    {
        public int RentalId { get; set; }
        public string Customer { get; set; } = "";
        public string Car { get; set; } = "";
        public DateOnly RentalDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; } = "";
        public int LateDays { get; set; }
        public string Status { get; set; } = "";
    }

    internal sealed class DashboardCarGridRow
    {
        public int CarId { get; set; }
        public string PlateNo { get; set; } = "";
        public string Type { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public decimal RatePerDay { get; set; }
        public CarStatus Status { get; set; }
    }

    internal sealed class RecentRentalGridRow
    {
        public int RentalId { get; set; }
        public string Customer { get; set; } = "";
        public string Car { get; set; } = "";
        public DateOnly ReturnDate { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string Status { get; set; } = "";
    }

    internal sealed class ReportRentalHistoryRow
    {
        public int RentalId { get; set; }
        public string Customer { get; set; } = "";
        public string Car { get; set; } = "";
        public string CarType { get; set; } = "";
        public DateOnly RentalDate { get; set; }
        public DateOnly ExpectedReturn { get; set; }
        public DateOnly? ActualReturn { get; set; }
        public decimal DailyRate { get; set; }
        public int LateDays { get; set; }
        public decimal Penalty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; } = "";
        public string Status { get; set; } = "";
    }

    internal sealed record DashboardData(
        int TotalCars,
        int AvailableCars,
        int OverdueRentals,
        decimal MonthlyCollected,
        List<DashboardCarGridRow> AvailableCarsGrid,
        List<RecentRentalGridRow> RecentRentals,
        List<CarLookupItem> AvailableCarLookup);

    internal sealed record ReportsData(
        int TotalCars,
        int AvailableCars,
        int ActiveRentals,
        int OverdueRentals,
        decimal TotalCollected,
        List<ReportRentalHistoryRow> RentalHistory);

    internal sealed record CarFormInput(
        string PlateNumber,
        string Brand,
        string Model,
        int Year,
        string Color,
        int CarTypeId,
        decimal DailyRate,
        CarStatus Status);

    internal sealed record CarTypeFormInput(
        string TypeName,
        decimal DailyRate,
        string Description,
        bool IsActive);

    internal sealed record CustomerFormInput(
        string FullName,
        string ContactNumber,
        string LicenseNumber,
        string Address);

    internal sealed record ReservationFormInput(
        int CarId,
        string CustomerName,
        string ContactNumber,
        DateOnly PickupDate,
        DateOnly ExpectedReturnDate,
        decimal DepositAmount,
        PaymentMethod PaymentMethod,
        string PaymentReferenceNumber);

    internal sealed record QuickRentalFormInput(
        string CustomerName,
        int CarId,
        DateTime RentalDate,
        DateTime ExpectedReturnDate,
        decimal AmountPaid,
        PaymentMethod PaymentMethod);

    internal sealed record RentalFormInput(
        int CustomerId,
        int CarId,
        DateTime RentalDate,
        DateTime ExpectedReturnDate,
        decimal AmountPaid,
        PaymentMethod PaymentMethod);

    internal sealed record ReturnFormInput(
        int RentalId,
        DateTime ActualReturnDate,
        decimal AdditionalPayment,
        PaymentMethod PaymentMethod);
}
