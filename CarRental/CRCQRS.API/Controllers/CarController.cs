using CRCQRS.Application.Commands;
using CRCQRS.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRCQRS.API.Controllers
{
  //[Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class CarController : BaseApi
  {
    public CarController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add(RegisterCarCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return await RunCommand(command);
    }
    [HttpPut("Update")]
    public async Task<IActionResult> Update(UpdateCarCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return await RunCommand(command);
    }
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(DeleteCarCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return await RunCommand(command);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
      return await RunCommand(new GetAllCarsQuery());
    }
    [HttpGet("GetById{id}")]
    public async Task<IActionResult> GetById(GetCarsByIdQuery command)
    {
     
      return await RunCommand(command);
    }
  }
}
