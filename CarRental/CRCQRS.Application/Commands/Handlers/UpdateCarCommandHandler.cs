using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateCarCommandHandler(CRCQRSContext context, IMediator mediater, UserManager<ApplicationUser> userManager, IUserInfoService userSrv, IHttpContextAccessor httpContextAccessor)
    {
      _userSrv = userSrv;
      _context = context;
      _mediator = mediater;
      _httpContextAccessor = httpContextAccessor;
      _userManager = userManager;
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

      var currentUser = await _userManager.FindByIdAsync(userId.ToString());
      if (currentUser == null)
      {
        response.Success = false;
        response.Message = "User not found.";
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }

      var isVendor = await _userManager.IsInRoleAsync(currentUser, "Vendor");
      var isCustomer = await _userManager.IsInRoleAsync(currentUser, "Customer");

      if (!isVendor && !isCustomer)
      {
        response.Success = false;
        response.Message = "Unauthorized access.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }

      // Find the existing car
      var car = await _context.Cars.FindAsync(request.CarId);
      if (car == null)
      {
        response.Success = false;
        response.Message = "Car not found.";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }


      if (isVendor && car.CreatedBy != currentUser.Id)
      {
        response.Success = false;
        response.Message = "You are not allowed to update this car.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }


      car.CarName = request.CarName ?? car.CarName;
      car.Model = request.Model;
      car.Rentalprice = request.Rentalprice;
      car.IsAvailable = request.IsAvailable;

      if (request.File != null && request.File.Length > 0)
      {
        var updateFileCommand = new UpdateFileCommand
        {
          AppFileId = request.FileId,

        };

        var fileUpdateResult = await _mediator.Send(updateFileCommand, cancellationToken);

        if (!fileUpdateResult.Success)
        {
          return fileUpdateResult;
        }
      }

      await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      response.Message = "Car updated successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = car;

      // Log the update event
      var userInfo = await _userSrv.GetUserInfo();
      string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) updated a car on: {DateTime.Now}";
      await _mediator.Publish(new LoggingEvent("Information", statement, DateTime.UtcNow, userInfo.UserID, car));

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
