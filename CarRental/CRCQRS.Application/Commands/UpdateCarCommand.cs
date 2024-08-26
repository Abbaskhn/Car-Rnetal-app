using CRCQRS.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace CRCQRS.Application.Commands
{
  public class UpdateCarCommand : IRequest<ResponseResult>
  { public int CarId { get; set; } 
    public int Model { get; set; }
    public string CarName { get; set; }
    public int Rentalprice { get; set; }
    public long FileId { get; set; }
    public IFormFile File { get; set; }
  }
}
