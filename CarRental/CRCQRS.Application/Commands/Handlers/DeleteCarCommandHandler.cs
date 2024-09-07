  using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CRCQRS.Application.Commands.Handlers
{
  public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, ResponseResult>
  {
    private readonly IUserInfoService _userSrv;
    private readonly CRCQRSContext _context;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DeleteCarCommandHandler(CRCQRSContext context, IMediator mediater, IUserInfoService userSrv, IHttpContextAccessor httpContextAccessor)
    {
      _userSrv = userSrv;
      _context = context;
      _mediator = mediater;
      _httpContextAccessor = httpContextAccessor;
    }
    public async Task<ResponseResult> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      var userId = GetUserIdFromToken();

      if (userId == null)
      {
        response.Success = false;
        response.Message = Constants.Messages.USER_AUTHORIZATION;
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }

      // Find the existing car
      var objCar = await _context.Cars.FindAsync(request.CarId, cancellationToken);

      if (objCar == null)
      {
        response.Success = false;
        response.Message = Constants.Messages.CAR_NOTFOUND_MSG;
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }


      _context.Remove(objCar);

      await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      UserInfo userInfo = await _userSrv.GetUserInfo();
      response.Message = Constants.Messages.CAR_ADDED_MSG;
      response.StatusCode = HttpStatusCode.OK;
      response.Data = objCar;
      string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) Delete a car on: {DateTime.Now}";
      await _mediator.Publish(new LoggingEvent(Constants.LogLevels.INFORMATION, statement, DateTime.UtcNow, userInfo.UserID, objCar));
      return response;
    }
    private long GetUserIdFromToken()
    {
      var accessToken = _httpContextAccessor.HttpContext.Request.Headers[Constants.Authorization.AUTHORIZATION_HEADER].ToString().Replace(Constants.Authorization.TOKEN_TYPE, "");
      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

      var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
      return Convert.ToInt64(userIdClaim?.Value);
    }
  }
}
