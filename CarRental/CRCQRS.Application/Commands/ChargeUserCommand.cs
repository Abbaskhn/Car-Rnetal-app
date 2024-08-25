using CRCQRS.Common;
using MediatR;
namespace AppCOG.Application.Commands
{
  public class ChargeUserCommand : IRequest<ResponseResult>
  {
    public string StripeToken { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public long Amount { get; set; } // Amount in cents
    public string Description { get; set; }
  }
}
