using CRCQRS.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CRCQRS.Infrastructure
{
  public class CRCQRSContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
  {
    public CRCQRSContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<BookingCar> BookingCars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Table names
      modelBuilder.Entity<Vendor>().ToTable("Vendors");
      modelBuilder.Entity<Car>().ToTable("Cars");
      modelBuilder.Entity<BookingCar>().ToTable("BookingCars");

      // Configuring precision and scale for the TotalAmount column in BookingCar
      modelBuilder.Entity<BookingCar>()
                  .Property(b => b.TotalAmount)
                  .HasColumnType("decimal(18, 2)");

      // Configuring relationships

      // BookingCar -> Car (Many-to-One)
      modelBuilder.Entity<BookingCar>()
          .HasOne(b => b.Car)
          .WithMany(c => c.CarBookings)
          .HasForeignKey(b => b.CarId)
          .OnDelete(DeleteBehavior.Restrict);

      // BookingCar -> ApplicationUser (Many-to-One) for Customer
      modelBuilder.Entity<BookingCar>()
          .HasOne(b => b.AppUserCustomer)
          .WithMany(cu => cu.CustomerBookings)
          .HasForeignKey(b => b.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

      // Car -> Vendor (Many-to-One)
      modelBuilder.Entity<Car>()
          .HasOne(c => c.VendorUser)
          .WithMany(v => v.VendorCars)
          .HasForeignKey(c => c.VendorId)
          .OnDelete(DeleteBehavior.Restrict);

      // ApplicationUser -> BookingCar (One-to-Many) for CustomerBookings
      modelBuilder.Entity<ApplicationUser>()
          .HasMany(au => au.CustomerBookings)
          .WithOne(bc => bc.AppUserCustomer)
          .HasForeignKey(bc => bc.CustomerId)
          .OnDelete(DeleteBehavior.Restrict);

      // Vendor -> Car (One-to-Many)
      modelBuilder.Entity<Vendor>()
          .HasMany(v => v.VendorCars)
          .WithOne(c => c.VendorUser)
          .HasForeignKey(c => c.VendorId)
          .OnDelete(DeleteBehavior.Restrict);
    }
  }


}
