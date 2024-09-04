using CRCQRS.Common;
using MediatR;

namespace CRCQRS.Application.Queries
{

  public class GetBookingByIdQuery : IRequest<ResponseResult>
  {
    public int Id { get; set; }
  }
}
