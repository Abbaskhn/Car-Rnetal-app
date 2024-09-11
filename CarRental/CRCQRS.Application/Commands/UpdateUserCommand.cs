using CRCQRS.Common;
using MediatR;
namespace CRCQRS.Application.Commands
{
  public class UpdateUserCommand : IRequest<ResponseResult>
  { public long UserId { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public int Phone { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }
}
