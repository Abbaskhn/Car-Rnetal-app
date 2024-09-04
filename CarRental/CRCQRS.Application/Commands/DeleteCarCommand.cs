using CRCQRS.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace CRCQRS.Application.Commands
{
  public class DeleteCarCommand : IRequest<ResponseResult>
  {
    public int CarId { get; set; }

  }
}
