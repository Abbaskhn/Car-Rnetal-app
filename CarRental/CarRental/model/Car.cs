using System.ComponentModel.DataAnnotations;

namespace application.model
{
  public class Car
  {
    [Key]
    public int CarId { get; set; } // Updated property name for consistency

    public int Model { get; set; }

    public string CarName { get; set; }

    public int Rentalprice { get; set; }

    public string CarImage { get; set; } // Added property to store the image filename

    public bool IsAvailable { get; set; } // New property to track availability
  }

  public class CarDTO
  {
    public string CarName { get; set; }
    public int Model { get; set; }
    public int Rentalprice { get; set; }
    public IFormFile ImageFile { get; set; } // This will be the image file uploaded by the user
  }

  public class CarUpdateDTO
  {
    public int CarId { get; set; } // ID of the car being updated
    public string CarName { get; set; }
    public int Model { get; set; }
    public int Rentalprice { get; set; }
    public IFormFile ImageFile { get; set; } // Optional image file for updating
    public string CarImage { get; set; } // To store the existing image path if not updating

    public bool? IsAvailable { get; set; } // Optional property to update availability
  }
}
