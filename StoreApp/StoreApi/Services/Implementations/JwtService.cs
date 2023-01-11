using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Entities;
using StoreApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StoreApi.Services.Implementations
{
    public class JwtService : IJwtService
    {

        
        public string  GenerateJwtToken(AppUser user, IList<string> roles, IConfiguration config)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roleclaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("JWT:secret").Value));
            var creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: creds,
                expires: DateTime.UtcNow.AddHours(8),
                issuer: config.GetSection("JWT:issuer").Value,
                audience: config.GetSection("JWT:audience").Value
                );


            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenStr;
        }
    }
}
