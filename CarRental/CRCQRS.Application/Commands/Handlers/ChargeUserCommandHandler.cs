  using AppCOG.Application.Commands;
using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Infrastructure.Services;
using MediatR;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class ChargeUserCommandHandler : IRequestHandler<ChargeUserCommand, ResponseResult>
  {
    private readonly IMediator _mediator;
    private readonly IUserInfoService _userSrv;
    private readonly IStripeService _stripeSrv;

    public ChargeUserCommandHandler(IStripeService stripeSrv, IUserInfoService userSrv, IMediator mediator)
    {
      _mediator = mediator;
      _userSrv = userSrv;
      _stripeSrv = stripeSrv;
    }

    public async Task<ResponseResult> Handle(ChargeUserCommand request, CancellationToken cancellationToken)
    {
      var result = await _stripeSrv.ChargeUser(new ChargeRequest()
      {
        Amount = request.Amount,
        Description = request.Description,
        Email = request.Email,
        Name = request.Name,
        StripeToken = request.StripeToken
      });

      var response = new ResponseResult();

      UserInfo userInfo = await _userSrv.GetUserInfo();
      response.Success = true;
      response.Message = Constants.Messages.CAR_ADDED_SUCCESSFULLY;
      response.StatusCode = HttpStatusCode.OK;
      response.Data = result;

      string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) Paid using stripe for car rent, on: {DateTime.Now}";
      await _mediator.Publish(new LoggingEvent(Constants.LogLevels.INFORMATION, statement, DateTime.UtcNow, userInfo.UserID, result));

      return response;
    }


  }
}
