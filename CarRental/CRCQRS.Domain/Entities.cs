using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace CRCQRS.Domain
{
  public class BookingCar
  {
    [Key]
    public int Id { get; set; }

    public int CarId { get; set; }
    public virtual Car Car { get; set; }
    public long CustomerId { get; set; }
    public virtual ApplicationUser AppUserCustomer { get; set; }

    [Required]
    [StringLength(250)]
    public string Address { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalAmount { get; set; }


    // Navigation property (optional, if needed)

  }

  public class Car
  {
    /// <summary>
    /// Updated property name for consistency
    /// </summary>
    [Key]
    public int CarId { get; set; }
    public long VendorId { get; set; }
    public virtual Vendor VendorUser { get; set; }
    public int Model { get; set; }

    public string CarName { get; set; }

    public int Rentalprice { get; set; }
    /// <summary>
    /// New property to track availability
    /// </summary>
    public bool IsAvailable { get; set; }
    public ICollection<BookingCar> CarBookings { get; set; }
    public virtual ICollection<CarFile> CarFiles { get; set; }
  }
  public class CarFile
  {
    public int Id { get; set; }

    public int CarId { get; set; }
    public virtual Car Car { get; set; }
    public long AppFileId { get; set; }
    public virtual AppFile CarAppFiles { get; set; }
  }
  public class AppFile
  {
    [Key]
    public long Id { get; set; }

    public string FileName { get; set; }
    /// <summary>
    /// Extension
    /// </summary>
    public string ContentType { get; set; }

    public long FileSize { get; set; }

    public byte[] Data { get; set; }

    public DateTime UploadedOn { get; set; }

  }
  public class Vendor : ApplicationUser
  {

    public string Company { get; set; }
    public ICollection<Car> VendorCars { get; set; }
  }
  public class ApplicationUser : IdentityUser<long>
  {
    public string Name { get; set; }
    public string Address { get; set; }
    public int Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public long? UserImage { get; set; }
    public virtual AppFile UserImageFile { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }

    public ICollection<IdentityUserRole<long>> UserRoles { get; set; }
    public ICollection<BookingCar> CustomerBookings { get; set; }
  }

  public class ApplicationRole : IdentityRole<long>
  {
    public string Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }

    public ICollection<IdentityUserRole<long>> UserRoles { get; set; }
    //public ICollection<Permission> Permissions { get; set; }
  }
  // LogEntry.cs
  public class LogEntry
  {
    public int Id { get; set; }
    public string LogLevel { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public long UserId { get; set; }
    public string Type { get; set; }
    //Object as Json
    public string Source { get; set; }
  }

}
