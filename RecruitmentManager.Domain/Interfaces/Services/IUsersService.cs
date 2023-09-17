using RecruitmentManager.Domain.Dtos;

namespace RecruitmentManager.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        Task CreateAsync(UserDto userDto);

        Task<string> LoginAsync(UserDto userDto);

        Task<IEnumerable<string>> GetUserNamesAsync();
    }
}