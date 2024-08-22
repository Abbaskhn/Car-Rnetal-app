using application.model;
using Microsoft.EntityFrameworkCore;

namespace application.Dbcontext
{
    
    public class Dbuser : DbContext
    {
        public Dbuser(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public  DbSet<Car> cars { get; set; }
       public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Customers> customers { get; set; }
        public DbSet<BookingCar> BookingCarss { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Vendor>().ToTable("Vendors");
     
      // Configuring the precision and scale for the TotalAmount column
      modelBuilder.Entity<BookingCar>()
                  .Property(b => b.TotalAmount)
                  .HasColumnType("decimal(18, 2)");

      base.OnModelCreating(modelBuilder);
    }
    }

}
