using CRCQRS.Common;
using CRCQRS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateVendorCommandHandler(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<ResponseResult> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Retrieve the user by ID
      var vendor = await _userManager.FindByIdAsync(request.UserId.ToString()) as Vendor;
      if (vendor == null)
      {
        response.Success = false;
        response.Message = "Vendor not found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      // Update vendor properties
      vendor.UserName = request.UserName;
      vendor.Email = request.Email;
      vendor.Company = request.Company;

      // Use UserManager to update the password properly
      if (!string.IsNullOrEmpty(request.Password))
      {
        var token = await _userManager.GeneratePasswordResetTokenAsync(vendor);
        var passwordResult = await _userManager.ResetPasswordAsync(vendor, token, request.Password);

        if (!passwordResult.Succeeded)
        {
          response.Success = false;
          response.Message = "Vendor password update failed";
          response.StatusCode = HttpStatusCode.BadRequest;
          response.Data = passwordResult.Errors.Select(e => e.Description);
          return response;
        }
      }

      // Update the user in the Identity system
      var result = await _userManager.UpdateAsync(vendor);

      if (result.Succeeded)
      {
        response.Success = true;
        response.Message = "Vendor updated successfully";
        response.StatusCode = HttpStatusCode.OK;
      }
      else
      {
        response.Success = false;
        response.Message = "Vendor update failed";
        response.StatusCode = HttpStatusCode.BadRequest;
        response.Data = result.Errors.Select(e => e.Description);
      }

      return response;
    }
  }
}
