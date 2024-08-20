using application.Dbcontext;
using application.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingCarcontroller : ControllerBase
    {
        private readonly Dbuser _context;

        public BookingCarcontroller(Dbuser context)
        {
            _context = context;
        }

        // Get all bookings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.BookingCarss.ToListAsync();
            return Ok(bookings);
        }

        // Get a single booking by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _context.BookingCarss.FindAsync(id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }

        // Create a new booking
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingCar data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data.");
            }

            _context.BookingCarss.Add(data);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = data.Id }, data);
        }

        // Update an existing booking
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookingCar data)
        {
            if (id != data.Id)
            {
                return BadRequest("Booking ID mismatch.");
            }

            var booking = await _context.BookingCarss.FindAsync(id);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            booking.CarId = data.CarId;
            booking.Name = data.Name;
            booking.Email = data.Email;
            booking.Address = data.Address;
            booking.StartDate = data.StartDate;
            booking.EndDate = data.EndDate;
            booking.TotalAmount = data.TotalAmount;

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

            _context.BookingCarss.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}