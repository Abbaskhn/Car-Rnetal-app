using CRCQRS.Application.DTO;
using CRCQRS.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRCQRS.Application.Services
{
  public interface IUserInfoService
  {
    public Task<UserInfo> GetUserInfo();
  }
  public class UserInfoService : IUserInfoService
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    public UserInfoService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
      _userManager = userManager;
      _httpContextAccessor = httpContextAccessor;
    }
    public async Task<UserInfo> GetUserInfo()
    {
      var userId = GetUserIdFromToken();
      string username = null;

      if (userId != null)
      {
        // Get the user by ID
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
          username = user.UserName; // Get the username

        }
      }
      return new UserInfo() { UserID = userId.Value, UserName = username };

    }
    private long? GetUserIdFromToken()
    {
      var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

      var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
      if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
      {
        return userId;
      }

      return null;
    }
  }
}
