using RecruitmentManager.Domain.Entities;

namespace RecruitmentManager.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task CreateAsync(User user);

        Task<IEnumerable<User>> GetAsync();

        Task<bool> ExistsByNameAndPassword(string name, string password);
    }
}