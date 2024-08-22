using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace application.model
{
    public class BookingCar
    {
        [Key]
        public int Id { get; set; }

        public int CarId { get; set; } // Foreign key

        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
    public Car Car { get; set; }

        // Navigation property (optional, if needed)
        
    }
  public class BookingCarDto
  {
    public int Id { get; set; }

    public int CarId { get; set; } // Foreign key

    public string Name { get; set; }
    public string Email { get; set; }

    [Required]
    [StringLength(250)]
    public string Address { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalAmount { get; set; }
  }

}
