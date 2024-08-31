using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class UpdateVendorCommand : IRequest<ResponseResult>
  { public long UserId { get; set; }    
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Company { get; set; }

  }
}
