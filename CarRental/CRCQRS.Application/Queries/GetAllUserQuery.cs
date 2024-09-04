using CRCQRS.Common;
using MediatR;

namespace CRCQRS.Application.Queries
{

  public class GetAllUserQuery : IRequest<ResponseResult>
  {
  }
}
