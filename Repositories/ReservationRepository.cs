using Car_Rental_System.Data;
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
                .Include(r => r.Car).ThenInclude(c => c.CarType);

            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                query = query.Where(r =>
                    r.CustomerName.Contains(trimmed) ||
                    r.ContactNumber.Contains(trimmed) ||
                    r.PaymentReferenceNumber.Contains(trimmed) ||
                    r.Car.PlateNumber.Contains(trimmed) ||
                    r.Car.Brand.Contains(trimmed) ||
                    r.Car.Model.Contains(trimmed) ||
                    (r.Car.CarType != null && r.Car.CarType.TypeName.Contains(trimmed)));
            }

            return query
                .OrderBy(r => r.Status == ReservationStatus.Converted ? 2 :
                    r.Status == ReservationStatus.Cancelled ? 1 : 0)
                .ThenBy(r => r.PickupDate)
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

        public void Add(Reservation reservation)
        {
            db.Reservations.Add(reservation);
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
