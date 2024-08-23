using CRCQRS.Common;
using CRCQRS.Domain;
using CRCQRS.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CRCQRS.Application.Commands.Handlers
{
  public class RegisterCarCommandHandler : IRequestHandler<RegisterCarCommand, ResponseResult>
  {
    private readonly CRCQRSContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public RegisterCarCommandHandler(CRCQRSContext context, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResponseResult> Handle(RegisterCarCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      Car objCar = new Car() { CarName = request.CarName, Model = request.Model, Rentalprice = request.Rentalprice };
      var userId = GetUserIdFromToken();
      if (userId != null)
      {
        objCar.VendorId = userId; // Assign the User ID to VendorId or wherever necessary
      }
      _context.Cars.Add(objCar);
      var result = await _context.SaveChangesAsync(cancellationToken);
      response.Success = true;
      response.Message = "Car Added successfully";
      response.StatusCode = HttpStatusCode.OK;
      response.Data = new { FileId = objCar.CarId };
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
