using CRCQRS.Common;
using CRCQRS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<ResponseResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      var user = new ApplicationUser { UserName = request.UserName, Email = request.Email };
      var result = await _userManager.CreateAsync(user, request.Password);



      if (result.Succeeded)
      {
        await _userManager.AddToRoleAsync(user, request.Role);

        response.Success = true;
        response.Message = "User registered successfully";
        response.StatusCode = HttpStatusCode.OK;
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
