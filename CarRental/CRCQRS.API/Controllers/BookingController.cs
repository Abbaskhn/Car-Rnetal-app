
using CRCQRS.Application.Commands;
using CRCQRS.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRCQRS.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookingController : BaseApi
  {
    public BookingController(IMediator mediator) : base(mediator)
    { }
      [HttpGet("GetAllBooking")]
      public async Task<IActionResult> GetAllBooking()
      {
        var query = new GetAllBookingsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);

      }
    [HttpPost("Booking")]
    public async Task<IActionResult> Register(RegisterBookingCommand command)
    {
      return await RunCommand(command);
    }
  }
  }

