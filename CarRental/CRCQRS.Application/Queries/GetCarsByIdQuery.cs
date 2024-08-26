using CRCQRS.Common;
using MediatR;

namespace CRCQRS.Application.Queries
{

  public class GetCarsByIdQuery : IRequest<ResponseResult>
  {
    public int CarId { get; set; }
  }
}
