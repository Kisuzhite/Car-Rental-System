using Car_Rental_System.Data;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Repositories
{
    internal sealed class CarRepository
    {
        private readonly AppDbContext db;

        public CarRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<int> CountAsync()
        {
            return db.Cars.CountAsync();
        }

        public Task<int> CountByStatusAsync(CarStatus status)
        {
            return db.Cars.CountAsync(c => c.Status == status);
        }

        public Task<List<Car>> GetManageableCarsAsync(string search = "")
        {
            string trimmed = search.Trim();

            IQueryable<Car> query = db.Cars
                .Include(c => c.CarType)
                .Where(c => c.Status != CarStatus.Rented);

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                query = query.Where(c =>
                    c.PlateNumber.Contains(trimmed) ||
                    c.Brand.Contains(trimmed) ||
                    c.Model.Contains(trimmed) ||
                    (c.CarType != null && c.CarType.TypeName.Contains(trimmed)));
            }

            return query
                .OrderByDescending(c => c.CarId)
                .ToListAsync();
        }

        public Task<List<Car>> GetAvailableCarsAsync(string search = "")
        {
            string trimmed = search.Trim();

            IQueryable<Car> query = db.Cars
                .Include(c => c.CarType)
                .Where(c => c.Status == CarStatus.Available);

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                query = query.Where(c =>
                    c.PlateNumber.Contains(trimmed) ||
                    c.Brand.Contains(trimmed) ||
                    c.Model.Contains(trimmed) ||
                    (c.CarType != null && c.CarType.TypeName.Contains(trimmed)));
            }

            return query
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Model)
                .ToListAsync();
        }

        public Task<Car?> GetByIdAsync(int carId)
        {
            return db.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
        }

        public Task<Car?> GetAvailableByIdWithTypeAsync(int carId)
        {
            return db.Cars
                .Include(c => c.CarType)
                .FirstOrDefaultAsync(c =>
                    c.CarId == carId &&
                    c.Status == CarStatus.Available);
        }

        public Task<bool> PlateNumberExistsAsync(string plateNumber, int? excludedCarId = null)
        {
            return db.Cars.AnyAsync(c =>
                c.PlateNumber == plateNumber &&
                (!excludedCarId.HasValue || c.CarId != excludedCarId.Value));
        }

        public Task<bool> HasActiveRentalAsync(int carId)
        {
            return db.Rentals.AnyAsync(r =>
                r.CarId == carId &&
                r.Status == RentalStatus.Active);
        }

        public Task<bool> HasRentalHistoryAsync(int carId)
        {
            return db.Rentals.AnyAsync(r => r.CarId == carId);
        }

        public void Add(Car car)
        {
            db.Cars.Add(car);
        }

        public void Remove(Car car)
        {
            db.Cars.Remove(car);
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
