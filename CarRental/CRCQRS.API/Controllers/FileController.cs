using CRCQRS.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CRCQRS.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FileController : BaseApi
  {
    public FileController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadFileCommand command)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      return await RunCommand(command);
    }
  }
}
