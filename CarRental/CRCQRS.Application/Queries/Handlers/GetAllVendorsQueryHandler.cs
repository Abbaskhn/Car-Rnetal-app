
using CRCQRS.Application.Queries;
using CRCQRS.Common;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace AppCOG.Application.Queries.Handlers
{
  public class GetAllVendorQueryHandler : IRequestHandler<GetAllVendorQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;

    public GetAllVendorQueryHandler(CRCQRSContext context)
    {
      _context = context;
    }

    public async Task<ResponseResult> Handle(GetAllVendorQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      var vendor = await _context.Vendors.ToListAsync();

      response.Success = true;
      response.Message = "Users retrieved successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = vendor;
      response.TotalRecords = vendor.Count;

      return response;
    }
  }
}
