using application.model;

namespace CarRental.Interface
{
  public interface IFileService
  {
    Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
    void DeleteFile(string fileNameWithExtension);
  }
  public interface ICarRepository
  {
    Task<Car> AddCarAsync(Car car);
    Task<Car> UpdateCarAsync(Car car);
    Task<IEnumerable<Car>> GetCarsAsync();
    Task<Car?> FindCarByIdAsync(int id);
    Task DeleteCarAsync(Car car);
  }
}
