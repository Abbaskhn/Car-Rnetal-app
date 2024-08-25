using CRCQRS.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace CRCQRS.Application.Commands
{
  public class UpdateFileCommand : IRequest<ResponseResult>
  { public long AppFileId { get; set; }
    public IFormFile File { get; set; }
  }

}
