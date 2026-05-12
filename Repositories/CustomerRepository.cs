using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Repositories
{
    internal sealed class CustomerRepository
    {
        private readonly AppDbContext db;

        public CustomerRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<List<Customer>> SearchActiveAsync(string search = "")
        {
            string trimmed = search.Trim();
            IQueryable<Customer> query = db.Customers.Where(c => !c.IsArchived);

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                query = query.Where(c =>
                    c.FullName.Contains(trimmed) ||
                    c.ContactNumber.Contains(trimmed) ||
                    c.LicenseNumber.Contains(trimmed));
            }

            return query
                .OrderByDescending(c => c.CustomerId)
                .ToListAsync();
        }

        public Task<List<Customer>> GetActiveForLookupAsync()
        {
            return db.Customers
                .Where(c => !c.IsArchived)
                .OrderBy(c => c.FullName)
                .ToListAsync();
        }

        public Task<Customer?> GetByIdAsync(int customerId)
        {
            return db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public Task<Customer?> GetByIdWithRentalsAsync(int customerId)
        {
            return db.Customers
                .Include(c => c.Rentals)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public Task<bool> LicenseNumberExistsAsync(string licenseNumber, int? excludedCustomerId = null)
        {
            licenseNumber = ValidationHelper.NormalizeLicenseNumber(licenseNumber);

            if (string.Equals(licenseNumber, AppConstants.NotProvided, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(false);
            }

            return db.Customers.AnyAsync(c =>
                c.LicenseNumber == licenseNumber &&
                (!excludedCustomerId.HasValue || c.CustomerId != excludedCustomerId.Value));
        }

        public Task<int?> GetActiveRentalIdAsync(int customerId)
        {
            return db.Rentals
                .Where(r => r.CustomerId == customerId && r.Status == RentalStatus.Active)
                .OrderByDescending(r => r.RentalId)
                .Select(r => (int?)r.RentalId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetOrCreateByNameAsync(string customerName)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);

            Customer? existing = await db.Customers
                .Where(c => !c.IsArchived && c.FullName == customerName)
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                return existing.CustomerId;
            }

            var newCustomer = new Customer
            {
                FullName = customerName,
                ContactNumber = AppConstants.NotProvided,
                LicenseNumber = AppConstants.NotProvided,
                Address = AppConstants.NotProvided
            };

            db.Customers.Add(newCustomer);
            await db.SaveChangesAsync();
            return newCustomer.CustomerId;
        }

        public async Task<int> GetOrCreateByNameAndContactAsync(string customerName, string contactNumber)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);
            contactNumber = ValidationHelper.CollapseSpaces(contactNumber);

            Customer? existing = await db.Customers
                .Where(c => !c.IsArchived && c.FullName == customerName)
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                if (string.Equals(existing.ContactNumber, AppConstants.NotProvided, StringComparison.OrdinalIgnoreCase))
                {
                    existing.ContactNumber = contactNumber;
                    await db.SaveChangesAsync();
                }

                return existing.CustomerId;
            }

            var newCustomer = new Customer
            {
                FullName = customerName,
                ContactNumber = contactNumber,
                LicenseNumber = AppConstants.NotProvided,
                Address = AppConstants.NotProvided
            };

            db.Customers.Add(newCustomer);
            await db.SaveChangesAsync();
            return newCustomer.CustomerId;
        }

        public void Add(Customer customer)
        {
            db.Customers.Add(customer);
        }

        public void Remove(Customer customer)
        {
            db.Customers.Remove(customer);
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
