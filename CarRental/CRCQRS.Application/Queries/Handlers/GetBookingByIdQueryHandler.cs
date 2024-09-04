using CRCQRS.Application.DTO;
using CRCQRS.Application.Queries;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AppCOG.Application.Queries.Handlers
{
  public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public GetBookingByIdQueryHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      var booking = await _context.Set<BookingCar>()
          .Include(b => b.Car) // Include related car data if needed
          .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

      if (booking == null)
      {
        response.Success = false;
        response.Message = "Booking not found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      var bookingDto = new BookingCarDto
      {
        Id = booking.Id,
        CarId = booking.CarId,
        CustomerId = booking.CustomerId,
        Address = booking.Address,
        StartDate = booking.StartDate,
        EndDate = booking.EndDate,
        TotalAmount = booking.TotalAmount,
        CarName = booking.Car.CarName
      };

      response.Success = true;
      response.Message = "Booking retrieved successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = bookingDto;

      return response;
    }
  }
}
