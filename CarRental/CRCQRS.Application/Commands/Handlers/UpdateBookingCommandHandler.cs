using CRCQRS.Application.DTO;
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
  public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public UpdateBookingCommandHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Check if the CarId exists
      var carExists = await _context.Set<Car>().AnyAsync(c => c.CarId == request.CarId, cancellationToken);
      if (!carExists)
      {
        response.Success = false;
        response.Message = "The specified car does not exist";
        response.StatusCode = HttpStatusCode.BadRequest;
        return response;
      }

      // Find the existing booking
      var booking = await _context.Set<BookingCar>()
          .Include(b => b.Car)
          .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

      if (booking == null)
      {
        response.Success = false;
        response.Message = "Booking not found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      // Check if the car is available for the new dates
      var isOverlappingBooking = await _context.Set<BookingCar>()
          .Where(b => b.CarId == request.CarId && b.Id != request.Id)
          .AnyAsync(b => b.StartDate < request.EndDate && request.StartDate < b.EndDate, cancellationToken);

      if (isOverlappingBooking)
      {
        response.Success = false;
        response.Message = "Car is already booked for the requested dates";
        response.StatusCode = HttpStatusCode.BadRequest;
        return response;
      }

      // Update the booking details
      booking.CarId = request.CarId;
      booking.CustomerId = request.CustomerId;
      booking.Address = request.Address;
      booking.StartDate = request.StartDate;
      booking.EndDate = request.EndDate;
      booking.TotalAmount = Convert.ToDecimal((request.EndDate - request.StartDate).TotalDays) * booking.Car.Rentalprice;

      _context.Set<BookingCar>().Update(booking);
      await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      response.Message = "Booking updated successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = booking.Id;

      return response;
    }

  }
}
