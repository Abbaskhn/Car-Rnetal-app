using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class RegisterCarCommand : IRequest<ResponseResult>
  {
    public int Model { get; set; }
    public string CarName { get; set; }
    public int Rentalprice { get; set; }
    public long FileId { get; set; }
  }
}
