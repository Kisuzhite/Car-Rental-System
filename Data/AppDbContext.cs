using Car_Rental_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_System.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CarRentalDB;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(e =>
            {
                e.Property(c => c.PlateNumber).HasMaxLength(20).IsRequired();
                e.Property(c => c.Brand).HasMaxLength(50).IsRequired();
                e.Property(c => c.Model).HasMaxLength(50).IsRequired();
                e.Property(c => c.Color).HasMaxLength(30).IsRequired();
                e.Property(c => c.DailyRate).HasColumnType("decimal(10,2)");
                e.Property(c => c.Status).HasMaxLength(20).IsRequired();
                e.Property(c => c.ImagePath).HasMaxLength(500);
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.Property(c => c.FullName).HasMaxLength(100).IsRequired();
                e.Property(c => c.ContactNumber).HasMaxLength(30).IsRequired();
                e.Property(c => c.LicenseNumber).HasMaxLength(50).IsRequired();
                e.Property(c => c.Address).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<Rental>(e =>
            {
                e.Property(r => r.DailyRate).HasColumnType("decimal(10,2)");
                e.Property(r => r.TotalAmount).HasColumnType("decimal(10,2)");
                e.Property(r => r.Status).HasMaxLength(20).IsRequired();
                e.HasOne(r => r.Customer).WithMany(c => c.Rentals).HasForeignKey(r => r.CustomerId);
                e.HasOne(r => r.Car).WithMany(c => c.Rentals).HasForeignKey(r => r.CarId);
            });
        }
    }
}