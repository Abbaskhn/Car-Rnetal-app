using AppCOG.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRCQRS.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentController : BaseApi
  {
    public PaymentController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("charge")]
    public async Task<IActionResult> Charge([FromBody] ChargeUserCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return await RunCommand(command);
    }

  }
}
