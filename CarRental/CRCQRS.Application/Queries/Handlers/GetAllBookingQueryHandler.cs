
using CRCQRS.Application.Queries;
using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace AppCOG.Application.Queries.Handlers
{
  public class GetAllBookingQueryHandler : IRequestHandler<GetAllBookingsQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public GetAllBookingQueryHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      var bookings = await _context.Set<BookingCar>()
          .Include(b => b.Car)
          .Include(b => b.AppUserCustomer)
          .ToListAsync(cancellationToken);

      if (bookings == null || !bookings.Any())
      {
        response.Success = false;
        response.Message = "No bookings found";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      response.Success = true;
      response.Message = "Bookings retrieved successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = bookings;

      return response;
    }
  }
}
