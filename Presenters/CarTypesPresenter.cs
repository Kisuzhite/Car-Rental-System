using Car_Rental_System.Data;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class CarTypesPresenter
    {
        private readonly ICarTypesView view;

        public CarTypesPresenter(ICarTypesView view)
        {
            this.view = view;
        }

        public async Task LoadCarTypesAsync()
        {
            using var db = new AppDbContext();
            var carTypeRepository = new CarTypeRepository(db);

            List<CarTypeGridRow> carTypes = (await carTypeRepository.SearchAsync(view.SearchText))
                .Select(ct => new CarTypeGridRow
                {
                    CarTypeId = ct.CarTypeId,
                    TypeName = ct.TypeName,
                    DailyRate = ct.DailyRate,
                    Description = ct.Description,
                    Status = ct.IsActive ? "Active" : "Inactive"
                })
                .ToList();

            view.BindCarTypes(carTypes);
        }

        public async Task<PresenterResult> AddCarTypeAsync(CarTypeFormInput input)
        {
            using var db = new AppDbContext();
            var carTypeRepository = new CarTypeRepository(db);

            if (await carTypeRepository.TypeNameExistsAsync(input.TypeName))
            {
                return DuplicateTypeResult(input.TypeName);
            }

            carTypeRepository.Add(new CarType
            {
                TypeName = input.TypeName,
                DailyRate = input.DailyRate,
                Description = input.Description,
                IsActive = input.IsActive
            });

            await carTypeRepository.SaveChangesAsync();
            return PresenterResult.Ok("Saved", "Car type saved successfully.");
        }

        public async Task<PresenterResult> UpdateCarTypeAsync(int carTypeId, CarTypeFormInput input)
        {
            using var db = new AppDbContext();
            var carTypeRepository = new CarTypeRepository(db);

            CarType? carType = await carTypeRepository.GetByIdAsync(carTypeId);
            if (carType == null)
            {
                return PresenterResult.Error("Error", "Car type not found.");
            }

            if (!string.Equals(carType.TypeName, input.TypeName, StringComparison.OrdinalIgnoreCase) &&
                await carTypeRepository.TypeNameExistsAsync(input.TypeName, carTypeId))
            {
                return DuplicateTypeResult(input.TypeName);
            }

            carType.DailyRate = input.DailyRate;
            carType.Description = input.Description;
            carType.IsActive = input.IsActive;

            foreach (Car car in await carTypeRepository.GetCarsByTypeAsync(carType.CarTypeId))
            {
                car.DailyRate = input.DailyRate;
            }

            await carTypeRepository.SaveChangesAsync();
            return PresenterResult.Ok("Updated", "Car type updated successfully.");
        }

        public async Task<CarTypeDeletePreview> PreviewDeleteAsync(int carTypeId)
        {
            using var db = new AppDbContext();
            var carTypeRepository = new CarTypeRepository(db);

            CarType? carType = await carTypeRepository.GetByIdWithCarsAsync(carTypeId);
            if (carType == null)
            {
                return new CarTypeDeletePreview(false, false, "Error", "Car type not found.");
            }

            bool hasCars = carType.Cars.Any();
            return new CarTypeDeletePreview(
                true,
                hasCars,
                hasCars ? "Confirm Deactivate" : "Confirm Delete",
                hasCars
                    ? "This car type is assigned to cars. Deactivate it instead of deleting it?"
                    : "Are you sure you want to delete this car type?");
        }

        public async Task<PresenterResult> DeleteCarTypeAsync(int carTypeId)
        {
            using var db = new AppDbContext();
            var carTypeRepository = new CarTypeRepository(db);

            CarType? carType = await carTypeRepository.GetByIdWithCarsAsync(carTypeId);
            if (carType == null)
            {
                return PresenterResult.Error("Error", "Car type not found.");
            }

            bool hasCars = carType.Cars.Any();
            if (hasCars)
            {
                carType.IsActive = false;
            }
            else
            {
                carTypeRepository.Remove(carType);
            }

            await carTypeRepository.SaveChangesAsync();
            return PresenterResult.Ok(
                hasCars ? "Deactivated" : "Deleted",
                hasCars ? "Car type deactivated successfully." : "Car type deleted successfully.");
        }

        private static PresenterResult DuplicateTypeResult(string typeName)
        {
            return PresenterResult.Warning(
                "Duplicate Car Type",
                $"A car type named '{typeName}' already exists.");
        }
    }
}
