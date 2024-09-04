using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class DeleteBookingCommand : IRequest<ResponseResult>
  {
    public int Id { get; set; }

  }
}
