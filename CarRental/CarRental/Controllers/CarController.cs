using application.Dbcontext;
using application.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Ensure that only authenticated users can access this controller
public class CarController : ControllerBase
{
    private readonly Dbuser _context;
    private readonly IWebHostEnvironment _environment;

    public CarController(Dbuser context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    [AllowAnonymous] // Allow all users to view cars
    public async Task<IActionResult> GetAll()
    {
        var cars = await _context.cars.ToListAsync();
        return Ok(cars);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Allow all users to view car details
    public async Task<IActionResult> Get(int id)
    {
        var car = await _context.cars.FirstOrDefaultAsync(x => x.carid == id);
        if (car == null)
        {
            return NotFound("Car not found");
        }
        return Ok(car);
    }

    [HttpPost]
    [Authorize(Roles = "Vendor, Admin")] // Only Vendors and Admins can create cars
    public async Task<IActionResult> Create([FromForm] CarCreateModel model)
    {
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var allowedFileExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

            if (!allowedFileExtensions.Contains(fileExtension))
            {
                return BadRequest($"Invalid file type. Only {string.Join(", ", allowedFileExtensions)} are allowed.");
            }

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(fileStream);
            }

            model.CarPhoto = Path.Combine("Uploads", uniqueFileName);
        }

        var car = new Car
        {
            CarName = model.CarName,
            Model = model.Model,
            Rentalprice = model.Rentalprice,
            CarPhoto = model.CarPhoto
        };

        _context.cars.Add(car);
        await _context.SaveChangesAsync();
        return Ok(car);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Vendor, Admin")] // Only Vendors and Admins can update cars
    public async Task<IActionResult> Update(int id, [FromForm] Car model)
    {
        var car = await _context.cars.FirstOrDefaultAsync(x => x.carid == id);
        if (car == null)
        {
            return NotFound("Car not found");
        }

        car.CarName = model.CarName;
        car.Model = model.Model;
        car.Rentalprice = model.Rentalprice;

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var allowedFileExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

            if (!allowedFileExtensions.Contains(fileExtension))
            {
                return BadRequest($"Invalid file type. Only {string.Join(", ", allowedFileExtensions)} are allowed.");
            }

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadPath = Path.Combine(_environment.WebRootPath, "Uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(fileStream);
            }

            car.CarPhoto = Path.Combine("Uploads", uniqueFileName);
        }

        await _context.SaveChangesAsync();
        return Ok(car);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can delete cars
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _context.cars.FirstOrDefaultAsync(x => x.carid == id);
        if (car == null)
        {
            return NotFound("Car not found");
        }

        if (!string.IsNullOrEmpty(car.CarPhoto))
        {
            var filePath = Path.Combine(_environment.WebRootPath, car.CarPhoto);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        _context.cars.Remove(car);
        await _context.SaveChangesAsync();
        return Ok(car);
    }
}
