using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CRCQRS.Application.Commands.Handlers
{
  public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, ResponseResult>
  {
    private readonly CRCQRSContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateCarCommandHandler(CRCQRSContext context, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseResult> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      var userId = GetUserIdFromToken();

      if (userId == null)
      {
        response.Success = false;
        response.Message = "User is not authorized.";
        response.StatusCode = HttpStatusCode.Unauthorized;
        return response;
      }

      
      var objCar = await _context.Cars.FindAsync(new object[] { request.CarId }, cancellationToken);

      if (objCar == null)
      {
        response.Success = false;
        response.Message = "Car not found.";
        response.StatusCode = HttpStatusCode.NotFound;
        return response;
      }

      objCar.CarName = request.CarName;
      objCar.Model = request.Model;
      objCar.Rentalprice = request.Rentalprice;
      objCar.VendorId = userId;

      
      if (request.FileId != null)
      {
        var carFile = await _context.CarFiles.FirstOrDefaultAsync(cf => cf.CarId == objCar.CarId, cancellationToken);
        if (carFile != null)
        {
          carFile.AppFileId = request.FileId;
        }
        else
        {
          CarFile objCarFile = new CarFile() { AppFileId = request.FileId, CarId = objCar.CarId };
          _context.CarFiles.Add(objCarFile);
        }
      }

      // Save changes
      var result = await _context.SaveChangesAsync(cancellationToken);

      response.Success = true;
      response.Message = "Car updated successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = objCar;
      return response;
    }
    private long GetUserIdFromToken()
    {
      var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

      var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
      return Convert.ToInt64(userIdClaim?.Value);
    }
  }
}
