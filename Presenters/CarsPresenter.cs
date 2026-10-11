using Car_Rental_System.Data;
using Car_Rental_System.Helpers;
using Car_Rental_System.Models;
using Car_Rental_System.Repositories;
using Car_Rental_System.ViewModels;
using Car_Rental_System.Views;

namespace Car_Rental_System.Presenters
{
    internal sealed class CarsPresenter
    {
        private readonly ICarsView view;

        public CarsPresenter(ICarsView view)
        {
            this.view = view;
        }

        public async Task LoadCarTypesAsync(int? includeCarTypeId = null)
        {
            using var db = new AppDbContext();
            var carTypeRepository = new CarTypeRepository(db);

            List<CarTypeLookupItem> carTypes = (await carTypeRepository.GetActiveCarTypesAsync(includeCarTypeId))
                .Select(ct => new CarTypeLookupItem
                {
                    CarTypeId = ct.CarTypeId,
                    TypeName = ct.TypeName,
                    DailyRate = ct.DailyRate
                })
                .ToList();

            view.BindCarTypes(carTypes);
        }

        public async Task LoadCarsAsync()
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);

            List<CarGridRow> cars = (await carRepository.GetManageableCarsAsync(view.SearchText))
                .Select(c => new CarGridRow
                {
                    CarId = c.CarId,
                    CarTypeId = c.CarTypeId,
                    PlateNumber = c.PlateNumber,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    Color = c.Color,
                    DailyRate = c.CarType != null ? c.CarType.DailyRate : c.DailyRate,
                    CarType = c.CarType?.TypeName ?? AppConstants.NotProvided,
                    Status = c.Status
                })
                .ToList();

            view.BindCars(cars);
        }

        public async Task<PresenterResult> AddCarAsync(CarFormInput input)
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);
            string plateNumber = ValidationHelper.NormalizePlateNumber(input.PlateNumber);

            if (await carRepository.PlateNumberExistsAsync(plateNumber))
            {
                return DuplicatePlateResult(plateNumber);
            }

            carRepository.Add(new Car
            {
                PlateNumber = plateNumber,
                Brand = ValidationHelper.CollapseSpaces(input.Brand),
                Model = ValidationHelper.CollapseSpaces(input.Model),
                Year = input.Year,
                Color = ValidationHelper.CollapseSpaces(input.Color),
                DailyRate = input.DailyRate,
                CarTypeId = input.CarTypeId,
                Status = input.Status,
                ImagePath = null
            });

            await carRepository.SaveChangesAsync();
            return PresenterResult.Ok("Saved", "Car added successfully.");
        }

        public async Task<PresenterResult> UpdateCarAsync(int carId, CarFormInput input)
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);

            Car? car = await carRepository.GetByIdAsync(carId);
            if (car == null)
            {
                return PresenterResult.Error("Error", "Car not found.");
            }

            string submittedPlateNumber = ValidationHelper.NormalizePlateNumber(input.PlateNumber);
            if (!string.Equals(car.PlateNumber, submittedPlateNumber, StringComparison.OrdinalIgnoreCase) &&
                await carRepository.PlateNumberExistsAsync(submittedPlateNumber, carId))
            {
                return DuplicatePlateResult(submittedPlateNumber);
            }

            car.Color = ValidationHelper.CollapseSpaces(input.Color);
            if (car.Status != CarStatus.Rented)
            {
                car.Status = input.Status;
            }

            await carRepository.SaveChangesAsync();
            return PresenterResult.Ok("Updated", "Car updated successfully.");
        }

        public async Task<PresenterResult> CanDeleteCarAsync(int carId)
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);

            if (await carRepository.HasActiveRentalAsync(carId))
            {
                return PresenterResult.Info(
                    "Car Is Currently Rented",
                    "This car has an active rental. Process the return before deleting it.");
            }

            if (await carRepository.HasRentalHistoryAsync(carId))
            {
                return PresenterResult.Info(
                    "Car Has Rental History",
                    "This car has rental history, so it cannot be deleted without breaking reports. Change its status to Maintenance instead.");
            }

            return PresenterResult.Ok("Can Delete", "");
        }

        public async Task<PresenterResult> DeleteCarAsync(int carId)
        {
            using var db = new AppDbContext();
            var carRepository = new CarRepository(db);
            Car? car = await carRepository.GetByIdAsync(carId);

            if (car == null)
            {
                return PresenterResult.Error("Error", "Car not found.");
            }

            carRepository.Remove(car);
            await carRepository.SaveChangesAsync();
            return PresenterResult.Ok("Deleted", "Car deleted successfully.");
        }

        private static PresenterResult DuplicatePlateResult(string plateNumber)
        {
            return PresenterResult.Warning(
                "Duplicate Plate Number",
                $"A car with plate number '{plateNumber}' already exists.");
        }
    }
}
