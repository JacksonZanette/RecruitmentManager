using RecruitmentManager.Domain.Entities;

namespace RecruitmentManager.Domain.Interfaces.Repositories
{
    public interface ICandidatesRepository
    {
        Task Save(Candidate candidate, CancellationToken cancellationToken);
    }
}