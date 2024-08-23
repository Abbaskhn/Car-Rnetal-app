using CRCQRS.Application.Commands;
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
  }
}
