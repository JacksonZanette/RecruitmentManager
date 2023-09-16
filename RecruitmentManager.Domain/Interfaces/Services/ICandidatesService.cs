using RecruitmentManager.Domain.Entities;

namespace RecruitmentManager.Domain.Interfaces.Services
{
    public interface ICandidatesService
    {
        Task Create(Candidate candidate, CancellationToken cancellationToken);

        Task<Candidate> GetById(Guid id, CancellationToken cancellationToken);

        Task Update(Candidate candidate, CancellationToken cancellationToken);

        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}