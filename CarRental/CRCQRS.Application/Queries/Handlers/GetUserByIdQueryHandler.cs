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
  public class GetUserByIdCarsQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public GetUserByIdCarsQueryHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Use await to properly handle asynchronous operations
      var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

      if (user == null)
      {
        response.Success = false;
        response.Message = "Customer not found";
        response.StatusCode = HttpStatusCode.NotFound;
        response.Data = null;
      }
      else
      {
        response.Success = true;
        response.Message = "Customer GET By Id successfully";
        response.StatusCode = HttpStatusCode.OK;
        response.Data = user;
      }

      return response;
    }
  }
}
