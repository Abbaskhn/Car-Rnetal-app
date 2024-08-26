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

    [HttpPost("registerCustomer")]
    public async Task<IActionResult> RegisterCustomer(RegisterUserCommand command)
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
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(DeleteVendorCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return await RunCommand(command);
    }
  }
}
