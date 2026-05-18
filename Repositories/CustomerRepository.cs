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

        public Task<Customer?> GetActiveByNameAsync(string customerName)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);

            return db.Customers
                .Where(c => !c.IsArchived && c.FullName == customerName)
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefaultAsync();
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

        public async Task<Customer> GetOrCreateCustomerByNameAndContactAsync(string customerName, string contactNumber)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);
            contactNumber = ValidationHelper.CollapseSpaces(contactNumber);

            Customer? existing = await db.Customers
                .Where(c => !c.IsArchived && c.FullName == customerName)
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                if (!IsMissingValue(contactNumber) &&
                    !string.Equals(existing.ContactNumber, contactNumber, StringComparison.OrdinalIgnoreCase))
                {
                    existing.ContactNumber = contactNumber;
                    await db.SaveChangesAsync();
                }

                return existing;
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
            return newCustomer;
        }

        public async Task<Customer> GetOrCreateCustomerByBookingDetailsAsync(
            string customerName,
            string contactNumber,
            string licenseNumber,
            string address)
        {
            customerName = ValidationHelper.CollapseSpaces(customerName);
            contactNumber = ValidationHelper.CollapseSpaces(contactNumber);
            licenseNumber = string.IsNullOrWhiteSpace(licenseNumber)
                ? AppConstants.NotProvided
                : ValidationHelper.NormalizeLicenseNumber(licenseNumber);
            address = string.IsNullOrWhiteSpace(address)
                ? AppConstants.NotProvided
                : ValidationHelper.CollapseSpaces(address);

            Customer? existing = await db.Customers
                .Where(c => !c.IsArchived && c.FullName == customerName)
                .OrderByDescending(c => c.CustomerId)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                bool changed = false;

                if (ShouldUpdate(existing.ContactNumber, contactNumber))
                {
                    existing.ContactNumber = contactNumber;
                    changed = true;
                }

                if (ShouldUpdate(existing.Address, address))
                {
                    existing.Address = address;
                    changed = true;
                }

                if (changed)
                {
                    await db.SaveChangesAsync();
                }

                return existing;
            }

            var newCustomer = new Customer
            {
                FullName = customerName,
                ContactNumber = contactNumber,
                LicenseNumber = licenseNumber,
                Address = address
            };

            db.Customers.Add(newCustomer);
            await db.SaveChangesAsync();
            return newCustomer;
        }

        public async Task<int> GetOrCreateByNameAndContactAsync(string customerName, string contactNumber)
        {
            Customer customer = await GetOrCreateCustomerByNameAndContactAsync(customerName, contactNumber);
            return customer.CustomerId;
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

        private static bool IsMissingValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ||
                string.Equals(value, AppConstants.NotProvided, StringComparison.OrdinalIgnoreCase);
        }

        private static bool ShouldUpdate(string currentValue, string newValue)
        {
            return !IsMissingValue(newValue) &&
                !string.Equals(currentValue, newValue, StringComparison.OrdinalIgnoreCase);
        }
    }
}
