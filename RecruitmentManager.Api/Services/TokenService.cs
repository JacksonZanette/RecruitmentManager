using Microsoft.IdentityModel.Tokens;
using RecruitmentManager.Api.Configuration;
using RecruitmentManager.Domain.Dtos;
using RecruitmentManager.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecruitmentManager.Api.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(UserDto userDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userDto.Name.ToString()),
                }),

                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiSettings.JWT_SECRET)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}