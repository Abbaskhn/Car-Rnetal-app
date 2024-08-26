
using CRCQRS.Application.Queries;
using CRCQRS.Common;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
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

      var cars = _context.Cars.FirstOrDefaultAsync(c => c.CarId==request.CarId, cancellationToken);
      if(cars == null) {
        response.Success = false;
        response.Message = "Invalid Car";
       
      }
      response.Success = true;
      response.Message = "User Get Car By Id successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = cars;
    

      return response;
    }
  }
}
