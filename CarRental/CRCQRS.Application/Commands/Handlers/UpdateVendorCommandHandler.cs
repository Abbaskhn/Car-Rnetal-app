using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CRCQRSContext _context;
    public UpdateVendorCommandHandler(UserManager<ApplicationUser> userManager, CRCQRSContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    public async Task<ResponseResult> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      var vendor = await _context.Vendors.FindAsync(request.VendorId, cancellationToken);
      if (vendor == null)
      {
        response.Success = false;
        response.Message = "Vendor registration failed";
        response.StatusCode = HttpStatusCode.BadRequest;
   
      }

      request.UserName = vendor.UserName;
      request.Email = vendor.Email;
      request.Password = vendor.PasswordHash;
      request.Company = vendor.Company;

 

      _context.Vendors.Update(vendor);
      await _context.SaveChangesAsync(cancellationToken);
      await _userManager.AddToRoleAsync(vendor, "Vendor");

      response.Success = true;
        response.Message = "Vendor registered successfully";
        response.StatusCode = HttpStatusCode.OK;
    
   

      return response;
    }

  }
}
