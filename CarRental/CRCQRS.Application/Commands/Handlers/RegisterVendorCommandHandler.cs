using CRCQRS.Common;
using CRCQRS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class RegisterVendorCommandHandler : IRequestHandler<RegisterVendorCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterVendorCommandHandler(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<ResponseResult> Handle(RegisterVendorCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      var vendor = new Vendor
      {
        UserName = request.UserName,
        Email = request.Email,
        Company = request.Company,
      
        CreatedDate = DateTime.Now
      };

      // Create the user in the Identity system
      var result = await _userManager.CreateAsync(vendor, request.Password);

      if (result.Succeeded)
      {
        await _userManager.AddToRoleAsync(vendor, "Vendor");
      
        response.Success = true;
        response.Message = "Vendor registered successfully";
        response.StatusCode = HttpStatusCode.OK;
   
      }
      else
      {
        response.Success = false;
        response.Message = "Vendor registration failed";
        response.StatusCode = HttpStatusCode.BadRequest;
        response.Data = result.Errors;
      }
    
      return response;
    }

  }
}
