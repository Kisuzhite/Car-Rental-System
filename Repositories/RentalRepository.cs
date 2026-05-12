using Car_Rental_System.Data;
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

        public async Task<decimal> GetMonthlyCollectedAsync(DateOnly firstOfMonth, DateOnly firstOfNextMonth)
        {
            return await db.Rentals
                .Where(r => r.ActualReturnDate >= firstOfMonth &&
                    r.ActualReturnDate < firstOfNextMonth)
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
                .Include(r => r.Customer)
                .Include(r => r.Car).ThenInclude(c => c.CarType)
                .OrderByDescending(r => r.RentalId)
                .Take(take)
                .ToListAsync();
        }

        public Task<List<Rental>> GetRentalHistoryAsync(string search = "")
        {
            return BuildRentalSearchQuery(search)
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
                .FirstOrDefaultAsync(r => r.RentalId == rentalId);
        }

        public Task<bool> CustomerHasActiveRentalAsync(int customerId)
        {
            return db.Rentals.AnyAsync(r =>
                r.CustomerId == customerId &&
                r.Status == RentalStatus.Active);
        }

        public void Add(Rental rental)
        {
            db.Rentals.Add(rental);
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        private IQueryable<Rental> BuildRentalSearchQuery(string search)
        {
            string trimmed = search.Trim();

            IQueryable<Rental> query = db.Rentals
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
    }
}
