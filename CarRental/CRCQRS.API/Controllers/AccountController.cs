using AppCOG.Application.Commands;
using CRCQRS.Application.Commands;
using CRCQRS.Application.Queries;
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
    [HttpPut("updateVendor/{id}")]
    public async Task<IActionResult> Update(UpdateVendorCommand command)
    {
       
      var result = await _mediator.Send(command);
      return Ok(result);
    }
    [HttpPut("updateCustomer/{id}")]
    public async Task<IActionResult> Update(UpdateUserCommand command)
    {

      var result = await _mediator.Send(command);
      return Ok(result);
    }
    [HttpGet("GetAllCustomer")]
    public async Task<IActionResult> GetAllUser()
    {
      var query = new GetAllUserQuery();
      var result = await _mediator.Send(query);
      return Ok(result);

    }
    [HttpGet("CustomerGetById/{id}")]
    public async Task<IActionResult> GetById(int id, GetUserByIdQuery command)
    {
      command = new GetUserByIdQuery
      {
        UserId = id
      };
      var result = await _mediator.Send(command);
      return Ok(result);
    }
    [HttpDelete("deleteCustomer/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var command = new DeleteUserCommand { UserId = id };
      var result = await _mediator.Send(command);
      return Ok(result);
    }
    [HttpGet("GetAllVendor")]
    public async Task<IActionResult> GetAllVendor()
    {
      var query = new GetAllVendorQuery();
      var result = await _mediator.Send(query);
      return Ok(result);
      
    }
    [HttpGet("VendorGetById/{id}")]
    public async Task<IActionResult> GetById( int id, GetVendorByIdQuery command)
    {
     command = new GetVendorByIdQuery
      {
        Id = id
      };
      var result=await _mediator.Send(command);
      return Ok(result);
    }
    [HttpDelete("deleteVendor/{id}")]
    public async Task<IActionResult> DeleteVendor(int id)
    {
      var command = new DeleteVendorCommand { VendorId = id };
      var result = await _mediator.Send(command);
      return Ok(result);
    }

  }
}
