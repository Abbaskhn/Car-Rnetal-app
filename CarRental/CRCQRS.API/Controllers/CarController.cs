using CRCQRS.Application.Commands;
using CRCQRS.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRCQRS.API.Controllers
{
  //[Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class CarController : ControllerBase
  {
    private readonly IMediator _mediator;

    public CarController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromBody] RegisterCarCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var result = await _mediator.Send(command);
      return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateCarCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var result = await _mediator.Send(command);
      return Ok(result);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var command = new DeleteCarCommand { CarId = id };
      var result = await _mediator.Send(command);
      return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
      var query = new GetAllCarsQuery();
      var result = await _mediator.Send(query);
      return Ok(result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var query = new GetCarsByIdQuery { CarId = id };
      var result = await _mediator.Send(query);
      return Ok(result);
    }
  }
}
