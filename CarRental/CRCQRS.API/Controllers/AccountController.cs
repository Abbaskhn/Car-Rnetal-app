using AppCOG.Application.Commands;
using CRCQRS.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRCQRS.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccountController : ControllerBase
  {
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
      var result = await _mediator.Send(command);

      return StatusCode((int)result.StatusCode, result);
    }
    [HttpPost("registerVendor")]
    public async Task<IActionResult> RegisterVendor(RegisterVendorCommand command)
    {
      var result = await _mediator.Send(command);

      return StatusCode((int)result.StatusCode, result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
      var result = await _mediator.Send(command);
      return StatusCode((int)result.StatusCode, result);
    }
  }
}
