using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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

      // Validate request
      if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
      {
        response.Success = false;
        response.Message = "Invalid input. Please provide all required fields.";
        response.StatusCode = HttpStatusCode.BadRequest;
        return response;
      }

      var user = new ApplicationUser
      {
        UserName = request.UserName,
        Email = request.Email,
        Phone = request.Phone,
        Name = request.Name,
        Address = request.Address,
  

      };

      var result = await _userManager.CreateAsync(user, request.Password);

      if (result.Succeeded)
      {
        await _userManager.AddToRoleAsync(user, "Customer");

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
        response.Data = result.Errors.Select(e => e.Description).ToList();
      }

      return response;
    }
  }
}
