using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class RegisterCarCommand : IRequest<ResponseResult>
  {
    public string CarName { get; set; }
    public int Model { get; set; }
    public int Rentalprice { get; set; }
    public bool IsAvailable { get; set; }
    public long? FileId { get; set; }
  }
}
