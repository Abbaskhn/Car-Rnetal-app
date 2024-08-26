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
  public class GetVendorByIdCarsQueryHandler : IRequestHandler<GetVendorByIdQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public GetVendorByIdCarsQueryHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Use await to properly handle asynchronous operations
      var car = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

      if (car == null)
      {
        response.Success = false;
        response.Message = "Vendor not found";
        response.StatusCode = HttpStatusCode.NotFound;
        response.Data = null;
      }
      else
      {
        response.Success = true;
        response.Message = "Vendor GET By Id successfully";
        response.StatusCode = HttpStatusCode.OK;
        response.Data = car;
      }

      return response;
    }
  }
}
