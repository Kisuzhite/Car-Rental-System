using Car_Rental_System.Models;
using Car_Rental_System.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(AppConfiguration.GetConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string moneyColumnType = $"decimal({AppConstants.MoneyPrecision},{AppConstants.MoneyScale})";

            modelBuilder.Entity<Car>(e =>
            {
                e.HasIndex(c => c.PlateNumber).IsUnique();
                e.Property(c => c.PlateNumber).HasMaxLength(20).IsRequired();
                e.Property(c => c.Brand).HasMaxLength(50).IsRequired();
                e.Property(c => c.Model).HasMaxLength(50).IsRequired();
                e.Property(c => c.Color).HasMaxLength(30).IsRequired();
                e.Property(c => c.DailyRate).HasColumnType(moneyColumnType);
                e.Property(c => c.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
                e.Property(c => c.ImagePath).HasMaxLength(500);
                e.HasOne(c => c.CarType)
                    .WithMany(ct => ct.Cars)
                    .HasForeignKey(c => c.CarTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CarType>(e =>
            {
                e.HasIndex(ct => ct.TypeName).IsUnique();
                e.Property(ct => ct.TypeName).HasMaxLength(50).IsRequired();
                e.Property(ct => ct.DailyRate).HasColumnType(moneyColumnType);
                e.Property(ct => ct.Description).HasMaxLength(200).IsRequired();
                e.HasData(
                    new CarType { CarTypeId = 1, TypeName = "Economy", DailyRate = 1500, Description = "Small fuel-efficient cars for budget rentals.", IsActive = true },
                    new CarType { CarTypeId = 2, TypeName = "Sedan", DailyRate = 2000, Description = "Comfortable standard cars for daily use.", IsActive = true },
                    new CarType { CarTypeId = 3, TypeName = "SUV", DailyRate = 3500, Description = "Higher-capacity vehicles for family or group trips.", IsActive = true },
                    new CarType { CarTypeId = 4, TypeName = "Van", DailyRate = 4000, Description = "Large passenger vehicles for groups and luggage.", IsActive = true },
                    new CarType { CarTypeId = 5, TypeName = "Luxury", DailyRate = 6000, Description = "Premium vehicles with higher comfort and style.", IsActive = true });
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.HasIndex(c => c.LicenseNumber)
                    .IsUnique()
                    .HasFilter($"[LicenseNumber] <> N'{AppConstants.NotProvided}'");
                e.Property(c => c.FullName).HasMaxLength(100).IsRequired();
                e.Property(c => c.ContactNumber).HasMaxLength(30).IsRequired();
                e.Property(c => c.LicenseNumber).HasMaxLength(50).IsRequired();
                e.Property(c => c.Address).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<Rental>(e =>
            {
                e.Property(r => r.DailyRate).HasColumnType(moneyColumnType);
                e.Property(r => r.TotalAmount).HasColumnType(moneyColumnType);
                e.Property(r => r.AmountPaid).HasColumnType(moneyColumnType);
                e.Property(r => r.PaymentStatus)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
                e.Property(r => r.PaymentMethod)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();
                e.Property(r => r.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
                e.HasOne(r => r.Customer)
                    .WithMany(c => c.Rentals)
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.Car)
                    .WithMany(c => c.Rentals)
                    .HasForeignKey(r => r.CarId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
