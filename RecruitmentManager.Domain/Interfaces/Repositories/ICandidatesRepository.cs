using RecruitmentManager.Domain.Entities;

namespace RecruitmentManager.Domain.Interfaces.Repositories
{
    public interface ICandidatesRepository
    {
        Task CreateAsync(Candidate candidate);

        Task<IEnumerable<Candidate>> GetAsync();

        Task<Candidate> GetByIdAsync(int id);

        Task UpdateAsync(Candidate candidate);

        Task DeleteAsync(int id);
    }
}