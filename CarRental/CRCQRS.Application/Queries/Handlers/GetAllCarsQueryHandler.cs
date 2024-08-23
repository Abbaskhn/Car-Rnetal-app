
using CRCQRS.Application.Queries;
using CRCQRS.Common;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace AppCOG.Application.Queries.Handlers
{
  public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public GetAllCarsQueryHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      var cars = _context.Cars.Include(c => c.CarFiles).ThenInclude(x => x.CarAppFiles).ToList();

      response.Success = true;
      response.Message = "Users retrieved successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = cars;
      response.TotalRecords = cars.Count;

      return response;
    }
  }
}
