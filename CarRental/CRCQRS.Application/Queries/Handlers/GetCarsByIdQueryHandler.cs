using CRCQRS.Application.Queries;
using CRCQRS.Common;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AppCOG.Application.Queries.Handlers
{
  public class GetCarsByIdCarsQueryHandler : IRequestHandler<GetCarsByIdQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public GetCarsByIdCarsQueryHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(GetCarsByIdQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Use await to properly handle asynchronous operations
      var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == request.CarId, cancellationToken);

      if (car == null)
      {
        response.Success = false;
        response.Message = "Car not found";
        response.StatusCode = HttpStatusCode.NotFound;
        response.Data = null;
      }
      else
      {
        response.Success = true;
        response.Message = "Car GET By Id successfully";
        response.StatusCode = HttpStatusCode.OK;
        response.Data = car;
      }

      return response;
    }
  }
}
