using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CRCQRS.Application.Commands.Handlers
{
  public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, ResponseResult>
  {
    IUserInfoService _userSrv;
    private readonly CRCQRSContext _context;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateCarCommandHandler(CRCQRSContext context, IMediator mediater, IUserInfoService userSrv, IHttpContextAccessor httpContextAccessor)
    {
      _userSrv = userSrv;
      _context = context;
      _mediator = mediater;
      _httpContextAccessor = httpContextAccessor;
    }
    public async Task<ResponseResult> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      var userId = GetUserIdFromToken();

      if (userId == null)
      {
        response.Success = false;
        response.Message = "User is not authorized.";
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }

      // Find the existing car
      var objCar = await _context.Cars.FindAsync(new object[] { request.CarId }, cancellationToken);

      if (objCar == null)
      {
        response.Success = false;
        response.Message = "Car not found.";
        response.StatusCode = HttpStatusCode.NotFound;

        return response;
      }

    
      objCar.CarName = request.CarName;
      objCar.Model = request.Model;
      objCar.Rentalprice = request.Rentalprice;
      objCar.VendorId = userId;

      if (request.File != null && request.File.Length > 0)
      {
        var updateFileCommand = new UpdateFileCommand
        {
          AppFileId = request.FileId,
          File = request.File
          
        };

        var fileUpdateResult = await _mediator.Send(updateFileCommand, cancellationToken);

        if (!fileUpdateResult.Success)
        {
          return fileUpdateResult;  
        }

   
      }

      await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      UserInfo userInfo = await _userSrv.GetUserInfo();
      response.Message = "Car updated successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = objCar;

      string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) Update a car on: {DateTime.Now}";
      await _mediator.Publish(new LoggingEvent("Information", statement, DateTime.UtcNow, userInfo.UserID, objCar));
      return response;
    }
    private long GetUserIdFromToken()
    {
      var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

      var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
      return Convert.ToInt64(userIdClaim?.Value);
    }
  }
}
