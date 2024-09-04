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
  public class RegisterBookingCommandHandler : IRequestHandler<RegisterBookingCommand, ResponseResult>
  {
    private readonly IMediator _mediator;
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterBookingCommandHandler(CRCQRSContext context, IUserInfoService userSrv, UserManager<ApplicationUser> userManager, IMediator mediator)
    {
      _context = context;
      _mediator = mediator;
      _userSrv = userSrv;
      _userManager = userManager;
    }
    public async Task<ResponseResult> Handle(RegisterBookingCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();


      var car = await _context.Set<Car>()
          .Include(c => c.CarBookings)
          .FirstOrDefaultAsync(c => c.CarId == request.CarId, cancellationToken);

      if (car == null)
      {
        response.Success = false;
        response.Message = "Car not found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }


      bool isOverlappingBooking = car.CarBookings.Any(b =>
          b.StartDate < request.EndDate && request.StartDate < b.EndDate);

      if (isOverlappingBooking)
      {
        response.Success = false;
        response.Message = "Car is already booked for the requested dates";
        response.StatusCode = HttpStatusCode.BadRequest;
        return response;
      }


      var days = (request.EndDate - request.StartDate).TotalDays;
      var totalAmount = days * car.Rentalprice;


      var booking = new BookingCar
      {
        CarId = request.CarId,
        CustomerId = request.CustomerId,
        Address = request.Address,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        TotalAmount = request.TotalAmount
      };

      // Save the booking
      await _context.Set<BookingCar>().AddAsync(booking, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      response.Message = "Car booked successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = booking.Id;

      return response;
    }
  }
}
