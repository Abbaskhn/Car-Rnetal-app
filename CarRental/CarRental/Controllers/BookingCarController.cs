using application.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using application.Dbcontext;
using Microsoft.AspNetCore.Authorization;

namespace application.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookingCarController : ControllerBase
  {
    private readonly Dbuser _context;

    public BookingCarController(Dbuser context)
    {
      _context = context;
    }

    // Get all bookings
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var bookings = await _context.BookingCarss
          .Select(b => new BookingCarDto
          {
            Id = b.Id,
            CarId = b.CarId,
            Name = b.Name,
            Email = b.Email,
            Address = b.Address,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            TotalAmount = b.TotalAmount
          })
          .ToListAsync();

      return Ok(bookings);
    }

    // Get a single booking by id
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var booking = await _context.BookingCarss
          .Where(b => b.Id == id)
          .Select(b => new BookingCarDto
          {
            Id = b.Id,
            CarId = b.CarId,
            Name = b.Name,
            Email = b.Email,
            Address = b.Address,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            TotalAmount = b.TotalAmount
          })
          .FirstOrDefaultAsync();

      if (booking == null)
      {
        return NotFound("Booking not found.");
      }

      return Ok(booking);
    }

    // Create a new booking
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCarDto dto)
    {
      if (dto == null)
      {
        return BadRequest("Invalid data.");
      }

      var car = await _context.cars.FindAsync(dto.CarId);
      if (car == null)
      {
        return BadRequest("Car not found.");
      }

      // Check if the car is available for the selected dates
      var conflictingBooking = await _context.BookingCarss
          .Where(b => b.CarId == dto.CarId && b.StartDate <= dto.EndDate && b.EndDate >= dto.StartDate)
          .AnyAsync();

      if (conflictingBooking)
      {
        return BadRequest("Car is not available for the selected dates.");
      }

      var booking = new BookingCar
      {
        CarId = dto.CarId,
        Name = dto.Name,
        Email = dto.Email,
        Address = dto.Address,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        TotalAmount = (decimal)(dto.EndDate - dto.StartDate).TotalDays * car.Rentalprice
      };

      _context.BookingCarss.Add(booking);

      // Mark car as unavailable
      car.IsAvailable = false;
      _context.cars.Update(car);

      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(Get), new { id = booking.Id }, booking);
    }

    // Update an existing booking
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookingCarDto dto)
    {
      if (id != dto.Id)
      {
        return BadRequest("Booking ID mismatch.");
      }

      var booking = await _context.BookingCarss.FindAsync(id);
      if (booking == null)
      {
        return NotFound("Booking not found.");
      }

      var car = await _context.cars.FindAsync(dto.CarId);
      if (car == null)
      {
        return BadRequest("Car not found.");
      }

      // Update booking details
      booking.CarId = dto.CarId;
      booking.Name = dto.Name;
      booking.Email = dto.Email;
      booking.Address = dto.Address;
      booking.StartDate = dto.StartDate;
      booking.EndDate = dto.EndDate;
      booking.TotalAmount = (decimal)(dto.EndDate - dto.StartDate).TotalDays * car.Rentalprice;

      await _context.SaveChangesAsync();
      return Ok(booking);
    }

    // Delete a booking
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var booking = await _context.BookingCarss.FindAsync(id);
      if (booking == null)
      {
        return NotFound("Booking not found.");
      }

      var car = await _context.cars.FindAsync(booking.CarId);
      if (car == null)
      {
        return BadRequest("Car not found.");
      }

      // Mark car as available
      car.IsAvailable = true;
      _context.cars.Update(car);

      _context.BookingCarss.Remove(booking);
      await _context.SaveChangesAsync();
      return NoContent();
    }

    [HttpGet("check-availability")]
    public IActionResult CheckAvailability([FromQuery] int carId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
      // Validate input
      if (carId <= 0 || startDate >= endDate)
      {
        return BadRequest("Invalid parameters. Ensure carId is positive and startDate is before endDate.");
      }

      // Check car availability for the given dates
      var bookings = _context.BookingCarss
          .Where(b => b.CarId == carId &&
                      b.EndDate >= startDate &&
                      b.StartDate <= endDate)
          .ToList();

      if (bookings.Any())
      {
        var bookedPeriod = bookings.First();
        return Ok(new
        {
          available = false,
          message = $"Car is already booked from {bookedPeriod.StartDate:yyyy-MM-dd HH:mm:ss} to {bookedPeriod.EndDate:yyyy-MM-dd HH:mm:ss}."
        });
      }

      return Ok(new { available = true });
    }


    // Return a car and optionally remove the booking
    [HttpPut("car/{id}/return")]
    public async Task<IActionResult> ReturnCar(int id)
    {
      var booking = await _context.BookingCarss
          .Where(b => b.CarId == id && b.EndDate < DateTime.Now) // Find the booking that has ended
          .FirstOrDefaultAsync();

      if (booking == null)
      {
        return NotFound("Booking not found or the car is still booked.");
      }

      var car = await _context.cars.FindAsync(booking.CarId);
      if (car == null)
      {
        return BadRequest("Car not found.");
      }

      // Mark car as available
      car.IsAvailable = true;
      _context.cars.Update(car);

      // Optionally, remove the booking
      _context.BookingCarss.Remove(booking);

      await _context.SaveChangesAsync();
      return Ok("Car has been marked as available.");
    }
  }
}
