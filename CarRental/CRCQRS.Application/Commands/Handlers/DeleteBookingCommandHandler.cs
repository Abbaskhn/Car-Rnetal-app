using CRCQRS.Application.DTO;
using CRCQRS.Application.Events;
using CRCQRS.Application.Services;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CRCQRS.Application.Commands.Handlers
{
  public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;

    public DeleteBookingCommandHandler(
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

    public async Task<ResponseResult> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Find the existing booking
      var booking = await _context.Set<BookingCar>()
          .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

      if (booking == null)
      {
        response.Success = false;
        response.Message = "Booking not found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      // Remove the booking
      _context.Set<BookingCar>().Remove(booking);
      await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      response.Message = "Booking deleted successfully";
      response.StatusCode = HttpStatusCode.OK;

      return response;
    }
  }
}
