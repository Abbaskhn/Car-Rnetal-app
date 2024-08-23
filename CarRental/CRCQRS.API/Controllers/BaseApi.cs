using CRCQRS.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRCQRS.API.Controllers
{
  public class BaseApi : ControllerBase
  {
    public readonly IMediator _mediator;

    public BaseApi(IMediator mediator)
    {
      _mediator = mediator;
    }
    [NonAction]
    public async Task<IActionResult> RunCommand(IRequest<ResponseResult> command)
    {
      var result = await _mediator.Send(command);
      return StatusCode((int)result.StatusCode, result);
    }
  }
}
