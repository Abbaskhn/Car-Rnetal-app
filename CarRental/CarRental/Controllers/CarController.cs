using Microsoft.AspNetCore.Mvc;
using CarRental.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using CarRental.Services;
using application.model;
using Microsoft.AspNetCore.Authorization;

namespace CarRental.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CarController : ControllerBase
  {
    private readonly IFileService _fileService;
    private readonly ICarRepository _carRepo;
    private readonly ILogger<CarController> _logger;

    public CarController(IFileService fileService, ICarRepository carRepo, ILogger<CarController> logger)
    {
      _fileService = fileService;
      _carRepo = carRepo;
      _logger = logger;
    }

    [HttpPost]
    
    public async Task<IActionResult> CreateCar([FromForm] CarDTO carToAdd)
    {
      try
      {
        if (carToAdd.ImageFile?.Length > 1 * 1024 * 1024)
        {
          return BadRequest("File size should not exceed 1 MB");
        }

        string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
        string createdImageName = await _fileService.SaveFileAsync(carToAdd.ImageFile, allowedFileExtensions);

        var car = new Car
        {
          CarName = carToAdd.CarName,
          Model = carToAdd.Model,
          Rentalprice = carToAdd.Rentalprice,
          CarImage = createdImageName,
        
        };

        var createdCar = await _carRepo.AddCarAsync(car);
        return CreatedAtAction(nameof(CreateCar), new { id = createdCar.CarId }, createdCar);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }

    [HttpPut("{id}")]
    
    public async Task<IActionResult> UpdateCar(int id, [FromForm] CarUpdateDTO carToUpdate)
    {
      try
      {
        if (id != carToUpdate.CarId)
        {
          return BadRequest("ID in URL and form body does not match.");
        }

        var existingCar = await _carRepo.FindCarByIdAsync(id);
        if (existingCar == null)
        {
          return NotFound($"Car with ID {id} not found.");
        }

        string oldImage = existingCar.CarImage;
        if (carToUpdate.ImageFile != null)
        {
          if (carToUpdate.ImageFile.Length > 1 * 1024 * 1024)
          {
            return BadRequest("File size should not exceed 1 MB");
          }

          string[] allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
          string createdImageName = await _fileService.SaveFileAsync(carToUpdate.ImageFile, allowedFileExtensions);
          existingCar.CarImage = createdImageName;
        }

        existingCar.CarName = carToUpdate.CarName;
        existingCar.Model = carToUpdate.Model;
        existingCar.Rentalprice = carToUpdate.Rentalprice;
       
        var updatedCar = await _carRepo.UpdateCarAsync(existingCar);

        if (carToUpdate.ImageFile != null)
        {
          _fileService.DeleteFile(oldImage);
        }

        return Ok(updatedCar);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }

    [HttpDelete("{id}")]
  
    public async Task<IActionResult> DeleteCar(int id)
    {
      try
      {
        var existingCar = await _carRepo.FindCarByIdAsync(id);
        if (existingCar == null)
        {
          return NotFound($"Car with ID {id} not found.");
        }

        await _carRepo.DeleteCarAsync(existingCar);
        _fileService.DeleteFile(existingCar.CarImage);
        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetCar(int id)
    {
      var car = await _carRepo.FindCarByIdAsync(id);
      if (car == null)
      {
        return NotFound($"Car with ID {id} not found.");
      }
      return Ok(car);
    }

    [HttpGet]
    
    public async Task<IActionResult> GetCars()
    {
      var cars = await _carRepo.GetCarsAsync();
      return Ok(cars);
    }

    }
  }
