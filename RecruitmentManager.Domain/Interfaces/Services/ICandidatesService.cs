using RecruitmentManager.Domain.Dtos;

namespace RecruitmentManager.Domain.Interfaces.Services
{
    public interface ICandidatesService
    {
        Task CreateAsync(CandidateSaveDto candidateSaveDto, CancellationToken cancellationToken);

        Task<IEnumerable<CandidateDto>> GetAsync(CancellationToken cancellationToken);

        Task<CandidateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task UpdateAsync(Guid id, CandidateSaveDto candidateSaveDto, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}