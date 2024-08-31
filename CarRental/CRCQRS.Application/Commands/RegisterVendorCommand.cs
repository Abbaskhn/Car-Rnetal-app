using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class RegisterVendorCommand : IRequest<ResponseResult>
  {
  
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Company { get; set; }

  }
}
