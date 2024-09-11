using CRCQRS.Common;
using CRCQRS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<ResponseResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      var user = await _userManager.FindByIdAsync(request.UserId.ToString());
      if (user == null)
      {
        response.Success = false;
        response.Message = "User not found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }
      user.UserName = request.UserName;
      user.Email = request.Email;
      user.Name = request.Name;
      user.Phone = request.Phone;
      user.Address = request.Address;


      if (!string.IsNullOrEmpty(request.Password))
      {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);

        if (!passwordResult.Succeeded)
        {
          response.Success = false;
          response.Message = "User password update failed";
          response.StatusCode = HttpStatusCode.BadRequest;
          response.Data = passwordResult.Errors.Select(e => e.Description);
          return response;
        }
      }

      var result = await _userManager.UpdateAsync(user);

      if (result.Succeeded)
      {
        response.Success = true;
        response.Message = "user updated successfully";
        response.StatusCode = HttpStatusCode.OK;
      }
      else
      {
        response.Success = false;
        response.Message = "user update failed";
        response.StatusCode = HttpStatusCode.BadRequest;
        response.Data = result.Errors.Select(e => e.Description);
      }

      return response;
    }
  }
}
