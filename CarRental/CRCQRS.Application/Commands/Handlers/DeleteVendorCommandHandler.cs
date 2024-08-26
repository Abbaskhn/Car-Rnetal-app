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
  public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;

    public DeleteVendorCommandHandler(
        UserManager<ApplicationUser> userManager,
        CRCQRSContext context,
        IMediator mediator,
        IUserInfoService userSrv)
    {
      _userManager = userManager;
      _context = context;
      _mediator = mediator;
      _userSrv = userSrv;
    }

    public async Task<ResponseResult> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Find the vendor using the provided Id
      var vendor = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);

      if (vendor != null)
      {
        // Remove the vendor from the database
        _context.Users.Remove(vendor);
        await _context.SaveChangesAsync(cancellationToken);

        // Fetch user info for logging
        UserInfo userInfo = await _userSrv.GetUserInfo();

        response.Success = true;
        response.Message = "Vendor deleted successfully";
        response.StatusCode = HttpStatusCode.OK;

        // Log the deletion event
        string statement = $"User: {userInfo.UserName} (ID: {userInfo.UserID}) deleted a vendor with ID: {vendor.Id} on: {DateTime.Now}";
        await _mediator.Publish(new LoggingEvent("Information", statement, DateTime.UtcNow, userInfo.UserID, vendor));
      }
      else
      {
        response.Success = false;
        response.Message = "Vendor deletion failed: Vendor not found";
        response.StatusCode = HttpStatusCode.NotFound;  // Use 404 to indicate that the vendor was not found
        response.Data = null;
      }

      return response;
    }
  }
}
