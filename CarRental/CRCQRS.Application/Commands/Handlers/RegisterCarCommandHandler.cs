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
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public RegisterCarCommandHandler(CRCQRSContext context, IUserInfoService userSrv, UserManager<ApplicationUser> userManager, IMediator mediator)
    {
      _context = context;
      _userSrv = userSrv;
      _userManager = userManager;
      _mediator = mediator;
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

      // Check if the user is a vendor
      if (!await _userManager.IsInRoleAsync(currentUser, "Vendor"))
      {
        response.Success = false;
        response.Message = "Only vendors can register cars.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }

      // Create the car entity
      var car = new Car
      {
        CarName = request.CarName,
        Model = request.Model,
        Rentalprice = request.Rentalprice,
        IsAvailable = request.IsAvailable,
        CreatedBy = currentUser.Id,
        VendorId = currentUser.Id
      };

      _context.Cars.Add(car);

      // Add car file if provided
      if (request.FileId.HasValue)
      {
        var carFile = new CarFile
        {
          AppFileId = request.FileId.Value,
          Car = car
        };

        _context.CarFiles.Add(carFile);
      }

      // Save changes to the database
      await _context.SaveChangesAsync(cancellationToken);

      // Set response
      response.Success = true;
      response.Message = "Car added successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = car;

      return response;
    }
  }
}
