
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
    [HttpPost("AddBooking")]
    public async Task<IActionResult> Register(RegisterBookingCommand command)
    {
      return await RunCommand(command);
    }
    //[HttpGet("GetBookingById/{id}")]
    //public async Task<IActionResult> GetById(int id, GetBookingByIdQuery command)
    //{
    //  command = new GetBookingByIdQuery
    //  {
    //   Id = id
    //  };
    //  var result = await _mediator.Send(command);
    //  return Ok(result);
    //}
    [HttpGet("GetBookingById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
     var command = new GetBookingByIdQuery {
        Id = id
         };
         var result = await _mediator.Send(command);
        return Ok(result);
      }
    [HttpPut("updateBooking/{id}")]
    public async Task<IActionResult> Update(UpdateBookingCommand command)
    {

      var result = await _mediator.Send(command);
      return Ok(result);
    }
    [HttpDelete("deleteBooking/{id}")]
    public async Task<IActionResult> DeleteVendor(int id)
    {
      var command = new DeleteBookingCommand { Id = id };
      var result = await _mediator.Send(command);
      return Ok(result);
    }
  }
  }

