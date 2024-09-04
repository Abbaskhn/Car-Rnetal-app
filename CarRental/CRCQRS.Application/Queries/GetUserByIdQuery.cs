using CRCQRS.Common;
using MediatR;

namespace CRCQRS.Application.Queries
{

  public class GetUserByIdQuery : IRequest<ResponseResult>
  {
    public long UserId { get; set; }
  }
}
