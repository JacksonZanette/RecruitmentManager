using RecruitmentManager.Domain.Dtos;

namespace RecruitmentManager.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(UserDto userDto);
    }
}