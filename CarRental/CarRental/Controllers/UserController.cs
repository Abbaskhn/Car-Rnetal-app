using application.Dbcontext;
using application.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Dbuser _authContext;
        public UserController(Dbuser authContext)
        {
            _authContext = authContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] login userobj)
        {
            if (userobj == null)
                return BadRequest();

            var user = await _authContext.users.FirstOrDefaultAsync(x => x.UserName == userobj.UserName);

            if (user == null)
                return NotFound(new { Message = "User Not Found!" });
            if (!SecurityApi.Verify(userobj.Password, user.Password))
            {
                return BadRequest(new
                {
                    Message = "Password is Incorrect"
                });
            }
            var token = CreateJwt(user);

            return Ok(new
            {
                Token = token,
                Message = "User Login Successfully!"
            });
        }

        [HttpPost("register")]
      
        public async Task<IActionResult> RegisterOrUpdateUser([FromBody] UserVM user)
        {
            if (user == null)
                return BadRequest(new { Message = "Invalid user data!" });

            var existingUser = await _authContext.users
                .FirstOrDefaultAsync(x => x.UserName == user.UserName || x.Email == user.Email);

            if (existingUser != null)
            {
                // Update existing user
                existingUser.Role = user.Role;
                // Handle password update if required
                // existingUser.Password = SecurityApi.Hash(user.Password);

                _authContext.users.Update(existingUser);
                await _authContext.SaveChangesAsync();

                return Ok(new { Message = "User role and registarion successfully!" });
            }
            else
            {
                // Create new user
                if (await CheckUserNameExitAsync(user.UserName))
                    return BadRequest(new { Message = "User already exists!" });
                if (await CheckUserNameEmailAsync(user.Email))
                    return BadRequest(new { Message = "Email already exists!" });

                var newUser = new User
                {
                    UserName = user.UserName,
                    Password = SecurityApi.Hash(user.Password),
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Token = ""
                };

                _authContext.users.Add(newUser);
                await _authContext.SaveChangesAsync();

                return Ok(new { Message = "User registered successfully!" });
            }
        }

    
private Task<bool> CheckUserNameExitAsync(string username) =>
          _authContext.users.AnyAsync(x => x.UserName == username);
        private Task<bool> CheckUserNameEmailAsync(string email) =>
      _authContext.users.AnyAsync(x => x.Email == email);
        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ThisIsSecretKeyForMyApplication0000099992222madebyAbbas");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName}{user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role) // Add role claim,

                // Claim("CreateUser", "CreateUser
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
