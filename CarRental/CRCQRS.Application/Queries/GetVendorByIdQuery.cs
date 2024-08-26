using CRCQRS.Common;
using MediatR;

namespace CRCQRS.Application.Queries
{

  public class GetVendorByIdQuery : IRequest<ResponseResult>
  {
    public long Id { get; set; }
  }
}
