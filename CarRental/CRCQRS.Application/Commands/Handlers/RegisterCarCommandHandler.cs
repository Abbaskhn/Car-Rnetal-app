using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class RegisterCarCommandHandler : IRequestHandler<RegisterCarCommand, ResponseResult>
  {
    private readonly IMediator _mediator;
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterCarCommandHandler(CRCQRSContext context, IUserInfoService userSrv, UserManager<ApplicationUser> userManager, IMediator mediator)
    {
      _context = context;
      _mediator = mediator;
      _userSrv = userSrv;
      _userManager = userManager;
    }

    public async Task<ResponseResult> Handle(RegisterCarCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Fetch the current user from the UserInfoService
      var userInfo = await _userSrv.GetUserInfo();

      if (userInfo == null || userInfo.UserID <= 0)
      {
        response.Success = false;
        response.Message = "User not found.";
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }

      // Fetch the ApplicationUser using the ID
      var currentUser = await _userManager.FindByIdAsync(userInfo.UserID.ToString());
      if (currentUser == null)
      {
        response.Success = false;
        response.Message = "User not found.";
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }


      if (!await _userManager.IsInRoleAsync(currentUser, "Vendor"))
      {
        response.Success = false;
        response.Message = "Only vendors can register cars.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }


      var objCar = new Car
      {
        CarName = request.CarName,
        Model = request.Model,
        Rentalprice = request.Rentalprice,
        VendorId = currentUser.Id
      };

      _context.Cars.Add(objCar);

      
      var objCarFile = new CarFile
      {
        AppFileId = request.FileId,
        Car = objCar
      };

      _context.CarFiles.Add(objCarFile);

      
      await _context.SaveChangesAsync(cancellationToken);

   
      
      response.Success = true;
      response.Message = "Car added successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = objCar;

      string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) registered a car on: {DateTime.Now}";
      await _mediator.Publish(new LoggingEvent("Information", statement, DateTime.UtcNow, userInfo.UserID, objCar));

      return response;
    }
  }
}
