using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        public IdentityController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        [AllowAnonymous]
        [HttpPost("Open")]
        public IActionResult Open([FromBody] ApiUsers apiUsersInformation)
        {
            var apiUser = IdentityCont(apiUsersInformation);
            if (apiUser == null) return NotFound("User Not Found!");

            var token = TokenCreate(apiUser);
            return Ok(token);
        }

        private string TokenCreate(ApiUsers apiUsers)
        {
            if (_jwtSettings.Key == null) throw new Exception("JWT settings Key cannot be empty");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsoap = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, apiUsers.UserName!),
                new Claim(ClaimTypes.Role, apiUsers.Role!)
            };

            var token = new JwtSecurityToken(_jwtSettings.Issuer, 
                _jwtSettings.Audience,
                claimsoap,
                expires:DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private ApiUsers? IdentityCont(ApiUsers apiUsersInformation)
        {
            return ApiUser
                .Users
                .FirstOrDefault(x =>
                    x.UserName?.ToLower() == apiUsersInformation.UserName
                    && x.Password == apiUsersInformation.Password);
        }
    }   
}
