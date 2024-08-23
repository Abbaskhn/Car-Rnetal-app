using CRCQRS.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace CRCQRS.Application.Commands
{
  public class UploadFileCommand : IRequest<ResponseResult>
  {
    public IFormFile File { get; set; }
  }

}
