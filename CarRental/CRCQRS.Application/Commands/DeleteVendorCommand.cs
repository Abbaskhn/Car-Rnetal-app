using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class DeleteVendorCommand : IRequest<ResponseResult>
  {
    public long Id { get; set; }

  }
}
