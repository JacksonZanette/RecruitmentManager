using RecruitmentManager.Domain.Entities;

namespace RecruitmentManager.Domain.Interfaces.Repositories
{
    public interface ICandidatesRepository
    {
        Task CreateAsync(Candidate candidate, CancellationToken cancellationToken);

        Task<Candidate> GetByIdAsync(Guid id, CancellationToken none);

        Task UpdateAsync(Candidate candidate, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<Candidate>> GetAsync(CancellationToken cancellationToken);
    }
}