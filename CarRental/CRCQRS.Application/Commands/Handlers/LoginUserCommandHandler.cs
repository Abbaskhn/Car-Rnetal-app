using AppCOG.Application.Commands;
using CRCQRS.Common;
using CRCQRS.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CRCQRS.Application.Commands.Handlers
{
  public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResponseResult>
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public LoginUserCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
      _userManager = userManager;
      _configuration = configuration;
    }

    public async Task<ResponseResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
      var response = new ResponseResult();
      var user = await _userManager.FindByNameAsync(request.UserName);
      if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
      {
        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user);
        response.Success = true;
        response.Message = "Login successful";
        response.StatusCode = HttpStatusCode.OK;
        response.Data = new
        {
          Token = token,
          Roles = roles,
          UserId = user.Id,
          RefreshToken = GenerateRefreshToken()
        };
      }
      else
      {
        response.Success = false;
        response.Message = "Invalid credentials";
        response.StatusCode = HttpStatusCode.Unauthorized;
      }

      return response;
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _configuration["Jwt:Issuer"],
          audience: _configuration["Jwt:Audience"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(30),
          signingCredentials: creds);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
      return Guid.NewGuid().ToString();
    }
  }
}
