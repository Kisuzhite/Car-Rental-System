using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Repositories
{
    internal sealed class RentalRepository
    {
        private readonly AppDbContext db;

        public RentalRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<int> CountActiveAsync()
        {
            return db.Rentals.CountAsync(r => r.Status == RentalStatus.Active);
        }

        public Task<int> CountOverdueAsync(DateOnly today)
        {
            return db.Rentals.CountAsync(r =>
                r.Status == RentalStatus.Active &&
                r.ExpectedReturnDate < today);
        }

        public Task<int> CountReturnsDueAsync(DateOnly returnDate)
        {
            return db.Rentals.CountAsync(r =>
                r.Status == RentalStatus.Active &&
                r.ExpectedReturnDate == returnDate);
        }

        public async Task<List<(decimal TotalAmount, decimal AmountPaid)>> GetActivePaymentAmountsAsync()
        {
            var amounts = await db.Rentals
                .AsNoTracking()
                .Where(r => r.Status == RentalStatus.Active)
                .Select(r => new
                {
                    r.TotalAmount,
                    r.AmountPaid
                })
                .ToListAsync();

            return amounts
                .Select(r => (r.TotalAmount, r.AmountPaid))
                .ToList();
        }

        public async Task<decimal> GetMonthlyCollectedAsync(DateOnly firstOfMonth, DateOnly firstOfNextMonth)
        {
            return await db.Rentals
                .Where(r => r.ActualReturnDate >= firstOfMonth &&
                    r.ActualReturnDate < firstOfNextMonth)
                .SumAsync(r => (decimal?)r.AmountPaid) ?? 0;
        }

        public async Task<decimal> GetCollectedForStartsInRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await db.Rentals
                .Where(r => r.RentalDate >= startDate &&
                    r.RentalDate < endDate)
                .SumAsync(r => (decimal?)r.AmountPaid) ?? 0;
        }

        public async Task<decimal> GetTotalCollectedAsync()
        {
            return await db.Rentals
                .SumAsync(r => (decimal?)r.AmountPaid) ?? 0;
        }

        public Task<List<Rental>> GetActiveRentalsAsync(string search = "")
        {
            return BuildRentalSearchQuery(search)
                .Where(r => r.Status == RentalStatus.Active)
                .OrderByDescending(r => r.RentalId)
                .ToListAsync();
        }

        public Task<List<Rental>> GetActiveRentalsForReturnAsync(string search = "")
        {
            return BuildRentalSearchQuery(search)
                .Where(r => r.Status == RentalStatus.Active)
                .OrderBy(r => r.ExpectedReturnDate)
                .ToListAsync();
        }

        public Task<List<Rental>> GetRecentRentalsAsync(int take)
        {
            return db.Rentals
                .AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .OrderByDescending(r => r.RentalId)
                .Take(take)
                .ToListAsync();
        }

        public Task<List<Rental>> GetRentalHistoryAsync(string search = "", DateOnly? today = null)
        {
            IQueryable<Rental> query = db.Rentals
                .AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType);

            query = ApplyRentalHistorySearchFilter(
                query,
                search,
                today ?? DateOnly.FromDateTime(DateTime.Today));

            return query
                .OrderByDescending(r => r.RentalId)
                .ToListAsync();
        }

        public Task<List<Rental>> GetRentalHistoryInDateRangeAsync(
            DateOnly startDate,
            DateOnly endDate)
        {
            IQueryable<Rental> query = db.Rentals
                .AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .Where(r => r.RentalDate >= startDate && r.RentalDate <= endDate);

            return query
                .OrderByDescending(r => r.RentalId)
                .ToListAsync();
        }

        public Task<Rental?> GetActiveByIdWithCarAsync(int rentalId)
        {
            return db.Rentals
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r =>
                    r.RentalId == rentalId &&
                    r.Status == RentalStatus.Active);
        }

        public Task<Rental?> GetForDocumentAsync(int rentalId)
        {
            return db.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r => r.RentalId == rentalId);
        }

        public Task<bool> CustomerHasActiveRentalAsync(int customerId)
        {
            return db.Rentals.AnyAsync(r =>
                r.CustomerId == customerId &&
                r.Status == RentalStatus.Active);
        }

        public async Task<bool> HasCarScheduleConflictAsync(
            int carId,
            DateOnly rentalDate,
            DateOnly expectedReturnDate)
        {
            bool hasActiveRentalConflict = await db.Rentals.AnyAsync(r =>
                r.CarId == carId &&
                r.Status == RentalStatus.Active &&
                rentalDate <= r.ExpectedReturnDate &&
                r.RentalDate <= expectedReturnDate);

            if (hasActiveRentalConflict)
            {
                return true;
            }

            return await db.Reservations.AnyAsync(r =>
                r.CarId == carId &&
                (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed) &&
                rentalDate <= r.ExpectedReturnDate &&
                r.PickupDate <= expectedReturnDate);
        }

        public void Add(Rental rental)
        {
            db.Rentals.Add(rental);
        }

        public void AddPayment(RentalPayment payment)
        {
            db.RentalPayments.Add(payment);
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        private IQueryable<Rental> BuildRentalSearchQuery(string search)
        {
            string trimmed = search.Trim();

            IQueryable<Rental> query = db.Rentals
                .AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType);

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                query = query.Where(r =>
                    r.Customer.FullName.Contains(trimmed) ||
                    r.Car.PlateNumber.Contains(trimmed) ||
                    r.Car.Brand.Contains(trimmed) ||
                    r.Car.Model.Contains(trimmed) ||
                    (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(trimmed)));
            }

            return query;
        }

        private static IQueryable<Rental> ApplyRentalHistorySearchFilter(
            IQueryable<Rental> query,
            string search,
            DateOnly today)
        {
            string trimmed = search.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return query;
            }

            List<RentalStatus> matchingStatuses = Enum.GetValues<RentalStatus>()
                .Where(status => status.ToString().Contains(trimmed, StringComparison.OrdinalIgnoreCase))
                .ToList();
            List<PaymentStatus> matchingPaymentStatuses = Enum.GetValues<PaymentStatus>()
                .Where(status => status.ToString().Contains(trimmed, StringComparison.OrdinalIgnoreCase))
                .ToList();
            List<PaymentMethod> matchingPaymentMethods = Enum.GetValues<PaymentMethod>()
                .Where(method => PaymentHelper.FormatPaymentMethod(method).Contains(trimmed, StringComparison.OrdinalIgnoreCase) ||
                    method.ToString().Contains(trimmed, StringComparison.OrdinalIgnoreCase))
                .ToList();
            bool matchesOverdueStatus = AppConstants.OverdueRentalStatus
                .Contains(trimmed, StringComparison.OrdinalIgnoreCase);

            return query.Where(r =>
                r.Customer.FullName.Contains(trimmed) ||
                r.Car.PlateNumber.Contains(trimmed) ||
                r.Car.Brand.Contains(trimmed) ||
                r.Car.Model.Contains(trimmed) ||
                (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(trimmed)) ||
                matchingStatuses.Contains(r.Status) ||
                matchingPaymentStatuses.Contains(r.PaymentStatus) ||
                matchingPaymentMethods.Contains(r.PaymentMethod) ||
                (matchesOverdueStatus &&
                    r.Status == RentalStatus.Active &&
                    r.ExpectedReturnDate < today));
        }
    }
}
