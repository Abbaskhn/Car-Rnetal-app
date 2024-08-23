using AppCOG.Application.Commands;
using CRCQRS.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRCQRS.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccountController : BaseApi
  {
    public AccountController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
      return await RunCommand(command);
    }
    [HttpPost("registerVendor")]
    public async Task<IActionResult> RegisterVendor(RegisterVendorCommand command)
    {
      return await RunCommand(command);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
      return await RunCommand(command);
    }
  }
}
