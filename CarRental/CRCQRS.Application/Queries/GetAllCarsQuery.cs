using CRCQRS.Common;
using MediatR;

namespace CRCQRS.Application.Queries
{

  public class GetAllCarsQuery : IRequest<ResponseResult>
  {
  }
}
