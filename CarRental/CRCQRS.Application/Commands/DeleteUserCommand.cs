using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class DeleteUserCommand : IRequest<ResponseResult>
  {
    public long UserId { get; set; }

  }
}
