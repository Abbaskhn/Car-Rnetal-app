using application.Dbcontext;
using application.model;
using CarRental.Interface;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services
{
  public class FileService : IFileService
  {
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
      _environment = environment;
    }

  
      public async Task<string> SaveFileAsync(IFormFile file, string[] allowedExtensions)
      {
        string uploadDir = Path.Combine("wwwroot", "images");
        if (!Directory.Exists(uploadDir))
        {
          Directory.CreateDirectory(uploadDir);
        }

        string fileExtension = Path.GetExtension(file.FileName);
        if (!allowedExtensions.Contains(fileExtension.ToLower()))
        {
          throw new InvalidOperationException("Invalid file extension");
        }

        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        string filePath = Path.Combine(uploadDir, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(fileStream);
        }

        return uniqueFileName;
      }

      public void DeleteFile(string fileName)
      {
        string filePath = Path.Combine("wwwroot", "images", fileName);
        if (File.Exists(filePath))
        {
          File.Delete(filePath);
        }
      }
    }
  }
  public class CarRepository : ICarRepository
  {
    private readonly Dbuser _context;

    public CarRepository(Dbuser context)
    {
      _context = context;
    }

    public async Task<Car> AddCarAsync(Car car)
    {
      _context.cars.Add(car);
      await _context.SaveChangesAsync();
      return car;
    }

    public async Task<Car> UpdateCarAsync(Car car)
    {
      _context.cars.Update(car);
      await _context.SaveChangesAsync();
      return car;
    }

    public async Task DeleteCarAsync(Car car)
    {
      _context.cars.Remove(car);
      await _context.SaveChangesAsync();
    }

    public async Task<Car?> FindCarByIdAsync(int id)
    {
      return await _context.cars.FindAsync(id);
    }

    public async Task<IEnumerable<Car>> GetCarsAsync()
    {
      return await _context.cars.ToListAsync();
    }
  }

