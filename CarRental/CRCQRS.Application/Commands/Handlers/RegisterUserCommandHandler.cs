using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseResult>
  {
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserInfoService _userSrv;
    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IUserInfoService userSrv, IMediator mediator)
    {
      _userManager = userManager;
      _userSrv = userSrv;
      _mediator = mediator;
    }

    public async Task<ResponseResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      var user = new ApplicationUser { UserName = request.UserName, Email = request.Email };
      var result = await _userManager.CreateAsync(user, request.Password);



      if (result.Succeeded)
      {
        await _userManager.AddToRoleAsync(user, "Customer");

        response.Success = true;
        response.Message = "User registered successfully";
        response.StatusCode = HttpStatusCode.OK;
        UserInfo userInfo = await _userSrv.GetUserInfo();
        string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) registered a user by role Customer on: {DateTime.Now}";
        await _mediator.Publish(new LoggingEvent("Information", statement, DateTime.UtcNow, userInfo.UserID, user));

        //response.Data = new UserDto
        //{
        //  UserId = user.Id.ToString(),
        //  UserName = user.UserName,
        //  Email = user.Email
        //};
      }
      else
      {
        response.Success = false;
        response.Message = "User registration failed";
        response.StatusCode = HttpStatusCode.BadRequest;
        response.Data = result.Errors;
      }

      return response;
    }

  }
}
