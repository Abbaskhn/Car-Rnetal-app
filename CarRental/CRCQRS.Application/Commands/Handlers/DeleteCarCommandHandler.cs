using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CRCQRS.Application.Commands.Handlers
{
  public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, ResponseResult>
  {
    private readonly CRCQRSContext _context;
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserInfoService _userSrv;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteCarCommandHandler(CRCQRSContext context, IMediator mediator, UserManager<ApplicationUser> userManager, IUserInfoService userSrv, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _mediator = mediator;
      _userManager = userManager;
      _userSrv = userSrv;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseResult> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
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

      if (isCustomer)
      {
        response.Success = false;
        response.Message = "Customers are not allowed to delete cars.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }

      if (!isVendor)
      {
        response.Success = false;
        response.Message = "Unauthorized access.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }

      // Find the car to be deleted
      var car = await _context.Cars.FindAsync(request.CarId);
      if (car == null)
      {
        response.Success = false;
        response.Message = "Car not found.";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      if (car.CreatedBy != currentUser.Id)
      {
        response.Success = false;
        response.Message = "You are not allowed to delete this car.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }

      _context.Cars.Remove(car);
      await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      response.Message = "Car deleted successfully.";
      response.StatusCode = HttpStatusCode.OK;

      // Log the delete event
      var userInfo = await _userSrv.GetUserInfo();
      string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) deleted a car on: {DateTime.Now}";
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
