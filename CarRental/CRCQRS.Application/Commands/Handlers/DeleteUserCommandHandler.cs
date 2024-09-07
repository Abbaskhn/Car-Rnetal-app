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
  public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;

    public DeleteUserCommandHandler(
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

    public async Task<ResponseResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Find the vendor using the provided Id
      var vendor = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

      if (vendor != null)
      {
        _context.Users.Remove(vendor);
        await _context.SaveChangesAsync(cancellationToken);

    

        response.Success = true;
        response.Message = Constants.Messages.CUSTOMER_DELETED_MESSAGE;
        response.StatusCode = HttpStatusCode.OK;

       
      }
      else
      {
        response.Success = false;
        response.Message = Constants.Messages.CUSTOMER_NOT_MESSAGE;
        response.StatusCode = HttpStatusCode.NotFound; 
        response.Data = null;
      }

      return response;
    }
  }
}
