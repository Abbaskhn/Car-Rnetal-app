using CRCQRS.Common;
using MediatR;
namespace AppCOG.Application.Commands
{
  public class LoginUserCommand : IRequest<ResponseResult>
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}
