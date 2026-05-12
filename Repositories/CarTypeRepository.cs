using Car_Rental_System.Data;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Repositories
{
    internal sealed class CarTypeRepository
    {
        private readonly AppDbContext db;

        public CarTypeRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<List<CarType>> GetActiveCarTypesAsync(int? includeCarTypeId = null)
        {
            return db.CarTypes
                .Where(ct => ct.IsActive || (includeCarTypeId.HasValue && ct.CarTypeId == includeCarTypeId.Value))
                .OrderBy(ct => ct.TypeName)
                .ToListAsync();
        }

        public Task<List<CarType>> SearchAsync(string search = "")
        {
            string trimmed = search.Trim();
            IQueryable<CarType> query = db.CarTypes;

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                query = query.Where(ct =>
                    ct.TypeName.Contains(trimmed) ||
                    ct.Description.Contains(trimmed));
            }

            return query
                .OrderByDescending(ct => ct.IsActive)
                .ThenBy(ct => ct.TypeName)
                .ToListAsync();
        }

        public Task<CarType?> GetByIdAsync(int carTypeId)
        {
            return db.CarTypes.FirstOrDefaultAsync(ct => ct.CarTypeId == carTypeId);
        }

        public Task<CarType?> GetByIdWithCarsAsync(int carTypeId)
        {
            return db.CarTypes
                .Include(ct => ct.Cars)
                .FirstOrDefaultAsync(ct => ct.CarTypeId == carTypeId);
        }

        public Task<bool> TypeNameExistsAsync(string typeName, int? excludedCarTypeId = null)
        {
            return db.CarTypes.AnyAsync(ct =>
                ct.TypeName == typeName &&
                (!excludedCarTypeId.HasValue || ct.CarTypeId != excludedCarTypeId.Value));
        }

        public Task<List<Car>> GetCarsByTypeAsync(int carTypeId)
        {
            return db.Cars
                .Where(c => c.CarTypeId == carTypeId)
                .ToListAsync();
        }

        public void Add(CarType carType)
        {
            db.CarTypes.Add(carType);
        }

        public void Remove(CarType carType)
        {
            db.CarTypes.Remove(carType);
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
