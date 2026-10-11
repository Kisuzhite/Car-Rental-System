using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Repositories
{
    internal sealed class ReservationRepository
    {
        private readonly AppDbContext db;

        public ReservationRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<List<Reservation>> SearchAsync(string search = "")
        {
            string trimmed = search.Trim();

            IQueryable<Reservation> query = db.Reservations
                .AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .Where(r => r.Status == ReservationStatus.Pending ||
                    r.Status == ReservationStatus.Confirmed);

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                query = query.Where(r =>
                    r.CustomerName.Contains(trimmed) ||
                    r.ContactNumber.Contains(trimmed) ||
                    (r.Customer != null && (
                        r.Customer.FullName.Contains(trimmed) ||
                        r.Customer.ContactNumber.Contains(trimmed))) ||
                    r.PaymentReferenceNumber.Contains(trimmed) ||
                    r.Car.PlateNumber.Contains(trimmed) ||
                    r.Car.Brand.Contains(trimmed) ||
                    r.Car.Model.Contains(trimmed) ||
                    (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(trimmed)));
            }

            return query
                .OrderBy(r => r.PickupDate)
                .ToListAsync();
        }

        public Task<Reservation?> GetByIdAsync(int reservationId)
        {
            return db.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }

        public Task<Reservation?> GetByIdWithCarAsync(int reservationId)
        {
            return db.Reservations
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }

        public Task<int> CountOpenAsync()
        {
            return db.Reservations.CountAsync(r =>
                r.Status == ReservationStatus.Pending ||
                r.Status == ReservationStatus.Confirmed);
        }

        public Task<int> CountOpenPickupsForDateAsync(DateOnly pickupDate)
        {
            return db.Reservations.CountAsync(r =>
                (r.Status == ReservationStatus.Pending ||
                    r.Status == ReservationStatus.Confirmed) &&
                r.PickupDate == pickupDate);
        }

        public async Task<List<(decimal TotalAmount, decimal AmountPaid)>> GetOpenPaymentAmountsAsync()
        {
            List<Reservation> reservations = await db.Reservations
                .AsNoTracking()
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .Where(r =>
                    r.Status == ReservationStatus.Pending ||
                    r.Status == ReservationStatus.Confirmed)
                .ToListAsync();

            return reservations
                .Select(r =>
                {
                    decimal dailyRate = r.Car.CarType?.DailyRate ?? r.Car.DailyRate;
                    decimal totalAmount = RentalCalculator.CalculateBaseAmount(
                        r.PickupDate.ToDateTime(TimeOnly.MinValue),
                        r.ExpectedReturnDate.ToDateTime(TimeOnly.MinValue),
                        dailyRate);

                    return (totalAmount, r.DepositAmount);
                })
                .ToList();
        }

        public async Task<decimal> GetOpenDepositCollectedAsync()
        {
            return await db.Reservations
                .Where(r => r.Status == ReservationStatus.Pending ||
                    r.Status == ReservationStatus.Confirmed)
                .SumAsync(r => (decimal?)r.DepositAmount) ?? 0;
        }

        public async Task<decimal> GetOpenDepositCollectedInRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await db.Reservations
                .Where(r =>
                    (r.Status == ReservationStatus.Pending ||
                        r.Status == ReservationStatus.Confirmed) &&
                    r.ReservationDate >= startDate &&
                    r.ReservationDate < endDate)
                .SumAsync(r => (decimal?)r.DepositAmount) ?? 0;
        }

        public async Task<bool> HasCarScheduleConflictAsync(
            int carId,
            DateOnly pickupDate,
            DateOnly expectedReturnDate,
            int? excludedReservationId = null)
        {
            if (await HasRentalConflictAsync(carId, pickupDate, expectedReturnDate))
            {
                return true;
            }

            return await db.Reservations.AnyAsync(r =>
                r.CarId == carId &&
                (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed) &&
                (!excludedReservationId.HasValue || r.ReservationId != excludedReservationId.Value) &&
                pickupDate <= r.ExpectedReturnDate &&
                r.PickupDate <= expectedReturnDate);
        }

        public async Task<bool> HasCustomerScheduleConflictAsync(
            int? customerId,
            string customerName,
            DateOnly pickupDate,
            DateOnly expectedReturnDate,
            int? excludedReservationId = null)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);

            if (customerId.HasValue &&
                await db.Rentals.AnyAsync(r =>
                    r.CustomerId == customerId.Value &&
                    r.Status == RentalStatus.Active &&
                    pickupDate <= r.ExpectedReturnDate &&
                    r.RentalDate <= expectedReturnDate))
            {
                return true;
            }

            return await db.Reservations.AnyAsync(r =>
                (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed) &&
                (!excludedReservationId.HasValue || r.ReservationId != excludedReservationId.Value) &&
                pickupDate <= r.ExpectedReturnDate &&
                r.PickupDate <= expectedReturnDate &&
                ((customerId.HasValue && r.CustomerId == customerId.Value) ||
                    r.CustomerName == customerName));
        }

        public void Add(Reservation reservation)
        {
            db.Reservations.Add(reservation);
        }

        public async Task SyncCustomerSnapshotAsync(Customer customer)
        {
            List<Reservation> reservations = await db.Reservations
                .Where(r => r.CustomerId == customer.CustomerId)
                .ToListAsync();

            foreach (Reservation reservation in reservations)
            {
                reservation.CustomerName = customer.FullName;
                reservation.ContactNumber = string.IsNullOrWhiteSpace(customer.ContactNumber)
                    ? AppConstants.NotProvided
                    : customer.ContactNumber;
            }
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        private Task<bool> HasRentalConflictAsync(
            int carId,
            DateOnly pickupDate,
            DateOnly expectedReturnDate)
        {
            return db.Rentals.AnyAsync(r =>
                r.CarId == carId &&
                r.Status == RentalStatus.Active &&
                pickupDate <= r.ExpectedReturnDate &&
                r.RentalDate <= expectedReturnDate);
        }
    }
}
