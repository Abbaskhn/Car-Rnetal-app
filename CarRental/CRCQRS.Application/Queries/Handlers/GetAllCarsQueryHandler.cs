using CRCQRS.Application.Queries;
using CRCQRS.Common;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CRCQRS.Application.Services;
using CRCQRS.Domain;

namespace AppCOG.Application.Queries.Handlers
{
  public class GetVendorCarsQueryHandler : IRequestHandler<GetAllCarsQuery, ResponseResult>
  {
    private readonly CRCQRSContext _context;
    private readonly IUserInfoService _userSrv;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetVendorCarsQueryHandler(CRCQRSContext context, IUserInfoService userSrv, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userSrv = userSrv;
      _userManager = userManager;
    }

    public async Task<ResponseResult> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();

      // Get the current user's information
      var userInfo = await _userSrv.GetUserInfo();
      if (userInfo == null || userInfo.UserID <= 0)
      {
        response.Success = false;
        response.Message = "User not found.";
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }

      // Fetch the current user using the UserManager
      var currentUser = await _userManager.FindByIdAsync(userInfo.UserID.ToString());
      if (currentUser == null)
      {
        response.Success = false;
        response.Message = "User not found.";
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }

      // Check if the current user is a vendor
      var isVendor = await _userManager.IsInRoleAsync(currentUser, "Vendor");
      var isCustomer = await _userManager.IsInRoleAsync(currentUser, "Customer");

      IQueryable<Car> carsQuery;

      // If the user is a vendor, show only the cars they created
      if (isVendor)
      {
        carsQuery = _context.Cars
            .Include(c => c.CarFiles)
                .ThenInclude(cf => cf.CarAppFiles)
            .Where(car => car.CreatedBy == currentUser.Id); // Filter by CreatedBy (vendor's ID)
      }
      // If the user is a customer, show all available cars
      else if (isCustomer)
      {
        carsQuery = _context.Cars
            .Include(c => c.CarFiles)
                .ThenInclude(cf => cf.CarAppFiles)
            .Where(car => car.IsAvailable); // Show only available cars
      }
      else
      {
        response.Success = false;
        response.Message = "Unauthorized access.";
        response.StatusCode = HttpStatusCode.Forbidden;
        return response;
      }

      // Execute the query and return results
      var cars = await carsQuery
          .Select(car => new
          {
            car.CarId,
            car.CarName,
            car.Model,
            car.Rentalprice,
            car.IsAvailable,
            AppFiles = car.CarFiles.Select(cf => new
            {
              cf.CarAppFiles.FileName,
              FilePath = cf.CarAppFiles.FileName
            })
          })
          .ToListAsync(cancellationToken);

      response.Success = true;
      response.Message = isVendor ? "Vendor's cars retrieved successfully." : "All available cars retrieved successfully.";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = cars;

      return response;
    }
  }
}
