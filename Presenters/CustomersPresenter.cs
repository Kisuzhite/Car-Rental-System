using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class CustomersPresenter
    {
        private readonly ICustomersView view;

        public CustomersPresenter(ICustomersView view)
        {
            this.view = view;
        }

        public async Task LoadCustomersAsync()
        {
            using var db = new AppDbContext();
            var customerRepository = new CustomerRepository(db);

            List<CustomerGridRow> customers = (await customerRepository.SearchActiveAsync(view.SearchText))
                .Select(c => new CustomerGridRow
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    ContactNumber = c.ContactNumber,
                    LicenseNumber = c.LicenseNumber,
                    Address = c.Address
                })
                .ToList();

            view.BindCustomers(customers);
        }

        public async Task<PresenterResult> AddCustomerAsync(CustomerFormInput input)
        {
            using var db = new AppDbContext();
            var customerRepository = new CustomerRepository(db);
            string licenseNumber = ValidationHelper.NormalizeLicenseNumber(input.LicenseNumber);

            if (await customerRepository.LicenseNumberExistsAsync(licenseNumber))
            {
                return DuplicateLicenseResult(licenseNumber);
            }

            customerRepository.Add(new Customer
            {
                FullName = ValidationHelper.CollapseSpaces(input.FullName),
                ContactNumber = ValidationHelper.CollapseSpaces(input.ContactNumber),
                LicenseNumber = licenseNumber,
                Address = ValidationHelper.CollapseSpaces(input.Address)
            });

            await customerRepository.SaveChangesAsync();
            return PresenterResult.Ok("Saved", "Customer added successfully.");
        }

        public async Task<PresenterResult> UpdateCustomerAsync(int customerId, CustomerFormInput input)
        {
            using var db = new AppDbContext();
            var customerRepository = new CustomerRepository(db);

            Customer? customer = await customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                return PresenterResult.Error("Error", "Customer not found.");
            }

            string licenseNumber = ValidationHelper.NormalizeLicenseNumber(input.LicenseNumber);
            if (await customerRepository.LicenseNumberExistsAsync(licenseNumber, customerId))
            {
                return DuplicateLicenseResult(licenseNumber);
            }

            customer.FullName = ValidationHelper.CollapseSpaces(input.FullName);
            customer.ContactNumber = ValidationHelper.CollapseSpaces(input.ContactNumber);
            customer.LicenseNumber = licenseNumber;
            customer.Address = ValidationHelper.CollapseSpaces(input.Address);

            await customerRepository.SaveChangesAsync();
            int? activeRentalId = await customerRepository.GetActiveRentalIdAsync(customer.CustomerId);

            return PresenterResult.Ok("Updated", "Customer updated successfully.", activeRentalId);
        }

        public async Task<CustomerDeletePreview> PreviewDeleteAsync(int customerId)
        {
            using var db = new AppDbContext();
            var customerRepository = new CustomerRepository(db);
            Customer? customer = await customerRepository.GetByIdWithRentalsAsync(customerId);

            if (customer == null)
            {
                return new CustomerDeletePreview(false, false, "Error", "Customer not found.");
            }

            bool hasActiveRentals = customer.Rentals.Any(r => r.Status == RentalStatus.Active);
            if (hasActiveRentals)
            {
                return new CustomerDeletePreview(
                    false,
                    true,
                    "Active Rental Found",
                    "This customer still has an active rental. Process the return before deleting the customer.");
            }

            bool hasRentalHistory = customer.Rentals.Any();
            return new CustomerDeletePreview(
                true,
                hasRentalHistory,
                hasRentalHistory ? "Confirm Archive" : "Confirm Delete",
                hasRentalHistory
                    ? "This customer has rental history. Archive the customer instead of deleting their records?"
                    : "Are you sure you want to delete this customer?");
        }

        public async Task<PresenterResult> DeleteCustomerAsync(int customerId)
        {
            using var db = new AppDbContext();
            var customerRepository = new CustomerRepository(db);
            Customer? customer = await customerRepository.GetByIdWithRentalsAsync(customerId);

            if (customer == null)
            {
                return PresenterResult.Error("Error", "Customer not found.");
            }

            bool hasRentalHistory = customer.Rentals.Any();
            if (hasRentalHistory)
            {
                customer.IsArchived = true;
            }
            else
            {
                customerRepository.Remove(customer);
            }

            await customerRepository.SaveChangesAsync();
            return PresenterResult.Ok(
                hasRentalHistory ? "Archived" : "Deleted",
                hasRentalHistory
                    ? "Customer archived successfully. Rental history was preserved."
                    : "Customer deleted successfully.");
        }

        private static PresenterResult DuplicateLicenseResult(string licenseNumber)
        {
            return PresenterResult.Warning(
                "Duplicate License Number",
                $"A customer with license number '{licenseNumber}' already exists.");
        }
    }
}
